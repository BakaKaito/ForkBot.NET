﻿using PKHeX.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SysBot.Base.SwitchButton;
using static SysBot.Pokemon.PokeDataOffsets;

namespace SysBot.Pokemon
{
    public class FossilBot : PokeRoutineExecutor
    {
        private readonly PokeTradeHub<PK8> Hub;
        private readonly BotCompleteCounts Counts;
        private readonly IDumper DumpSetting;
        private readonly int[] DesiredIVs;

        public FossilBot(PokeBotState cfg, PokeTradeHub<PK8> hub) : base(cfg)
        {
            Hub = hub;
            Counts = Hub.Counts;
            DumpSetting = Hub.Config.Folder;
            DesiredIVs = StopConditionSettings.InitializeTargetIVs(Hub);
        }

        private int encounterCount;

        private const int InjectBox = 0;
        private const int InjectSlot = 0;

        private static readonly PK8 Blank = new();

        public override async Task MainLoop(CancellationToken token)
        {
            Log("Identifying trainer data of the host console.");
            await IdentifyTrainer(token).ConfigureAwait(false);

            await SetCurrentBox(0, token).ConfigureAwait(false);

            var existing = await ReadBoxPokemon(InjectBox, InjectSlot, token).ConfigureAwait(false);
            if (existing.Species != 0 && existing.ChecksumValid)
            {
                Log("Destination slot is occupied! Dumping the Pokémon found there...");
                DumpPokemon(DumpSetting.DumpFolder, "saved", existing);
            }
            Log("Clearing destination slot to start the bot.");
            await SetBoxPokemon(Blank, InjectBox, InjectSlot, token).ConfigureAwait(false);

            Log("Checking item counts...");
            var pouchData = await Connection.ReadBytesAsync(ItemTreasureAddress, 80, token).ConfigureAwait(false);
            var counts = FossilCount.GetFossilCounts(pouchData);
            int reviveCount = counts.PossibleRevives(Hub.Config.Fossil.Species);
            if (reviveCount == 0)
            {
                Log("Insufficient fossil pieces. Please obtain at least one of each required fossil piece first.");
                return;
            }

            Log("Starting main FossilBot loop.");
            Config.IterateNextRoutine();
            while (!token.IsCancellationRequested && Config.NextRoutineType == PokeRoutineType.FossilBot)
            {
                if (encounterCount != 0 && encounterCount % reviveCount == 0)
                {
                    Log($"Ran out of fossils to revive {Hub.Config.Fossil.Species}.");
                    if (Hub.Config.Fossil.InjectWhenEmpty)
                    {
                        Log("Restoring original pouch data.");
                        await Connection.WriteBytesAsync(pouchData, ItemTreasureAddress, token).ConfigureAwait(false);
                        await Task.Delay(500, token).ConfigureAwait(false);
                    }
                    else
                    {
                        Log("Restart the game and the bot(s) or set \"Inject Fossils\" to True in the config.");
                        return;
                    }
                }

                await ReviveFossil(counts, token).ConfigureAwait(false);
                Log("Fossil revived. Checking details...");

                var pk = await ReadBoxPokemon(InjectBox, InjectSlot, token).ConfigureAwait(false);
                if (pk.Species == 0 || !pk.ChecksumValid)
                {
                    Log("Invalid data detected in destination slot. Restarting loop.");
                    continue;
                }

                encounterCount++;
                Log($"Encounter: {encounterCount}:{Environment.NewLine}{ShowdownParsing.GetShowdownText(pk)}{Environment.NewLine}{Environment.NewLine}");
                if (DumpSetting.Dump)
                    DumpPokemon(DumpSetting.DumpFolder, "fossil", pk);

                Counts.AddCompletedFossils();
                TradeExtensions.EncounterLogs(pk);

                if (StopConditionSettings.EncounterFound(pk, DesiredIVs, Hub.Config.StopConditions))
                {
                    if (Hub.Config.StopConditions.CaptureVideoClip)
                    {
                        await Task.Delay(Hub.Config.StopConditions.ExtraTimeWaitCaptureVideo, token).ConfigureAwait(false);
                        await PressAndHold(CAPTURE, 2_000, 1_000, token).ConfigureAwait(false);
                    }

                    if (Hub.Config.Fossil.ContinueAfterMatch)
                    {
                        Log($"{(!Hub.Config.StopConditions.PingOnMatch.Equals(string.Empty) ? $"<@{Hub.Config.StopConditions.PingOnMatch}>\n" : "")}Result found! Continuing to collect more fossils.");
                    }
                    else
                    {
                        Log($"{(!Hub.Config.StopConditions.PingOnMatch.Equals(string.Empty) ? $"<@{Hub.Config.StopConditions.PingOnMatch}>\n" : "")}Result found! Stopping routine execution; restart the bot(s) to search again.");
                        await DetachController(token).ConfigureAwait(false);
                        return;
                    }
                }

                Log("Clearing destination slot.");
                await SetBoxPokemon(Blank, InjectBox, InjectSlot, token).ConfigureAwait(false);
            }
        }

        private async Task ReviveFossil(FossilCount count, CancellationToken token)
        {
            Log("Starting fossil revival routine...");
            if (GameLang == LanguageID.Spanish)
                await Click(A, 0_900, token).ConfigureAwait(false);

            await Click(A, 1_100, token).ConfigureAwait(false);

            // French is slightly slower.
            if (GameLang == LanguageID.French)
                await Task.Delay(0_200, token).ConfigureAwait(false);

            await Click(A, 1_300, token).ConfigureAwait(false);

            // Selecting first fossil.
            if (count.UseSecondOption1(Hub.Config.Fossil.Species))
                await Click(DDOWN, 0_300, token).ConfigureAwait(false);
            await Click(A, 1_300, token).ConfigureAwait(false);

            // Selecting second fossil.
            if (count.UseSecondOption2(Hub.Config.Fossil.Species))
                await Click(DDOWN, 300, token).ConfigureAwait(false);

            // A spam through accepting the fossil and agreeing to revive.
            for (int i = 0; i < 8; i++)
                await Click(A, 0_400, token).ConfigureAwait(false);

            // Safe to mash B from here until we get out of all menus.
            while (!await IsOnOverworld(Hub.Config, token).ConfigureAwait(false))
                await Click(B, 0_400, token).ConfigureAwait(false);
        }
    }
}
