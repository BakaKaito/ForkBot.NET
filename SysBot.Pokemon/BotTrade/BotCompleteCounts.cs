﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SysBot.Pokemon
{
    public class BotCompleteCounts
    {
        private readonly CountSettings Config;

        private int CompletedTrades;
        private int CompletedGiveaways;
        private int CompletedEggs;
        private int CompletedFossils;
        private int CompletedEncounters;
        private int CompletedLegends;
        private int CompletedSeedChecks;
        private int CompletedSurprise;
        private int CompletedDistribution;
        private int CompletedClones;
        private int CompletedFixOTs;
        private int CompletedSpecialRequests;
        private int CompletedTradeCords;
        private int CompletedDumps;
        private int CompletedRaids;
        private int CompletedAdventures;

        public BotCompleteCounts(CountSettings config)
        {
            Config = config;
            LoadCountsFromConfig();
        }

        public void LoadCountsFromConfig()
        {
            CompletedTrades = Config.CompletedTrades;
            CompletedGiveaways = Config.CompletedGiveaways;
            CompletedEggs = Config.CompletedEggs;
            CompletedFossils = Config.CompletedFossils;
            CompletedEncounters = Config.CompletedEncounters;
            CompletedLegends = Config.CompletedLegends;
            CompletedSeedChecks = Config.CompletedSeedChecks;
            CompletedSurprise = Config.CompletedSurprise;
            CompletedDistribution = Config.CompletedDistribution;
            CompletedClones = Config.CompletedClones;
            CompletedFixOTs = Config.CompletedFixOTs;
            CompletedSpecialRequests = Config.CompletedSpecialRequests;
            CompletedTradeCords = Config.CompletedTradeCords;
            CompletedDumps = Config.CompletedDumps;
            CompletedRaids = Config.CompletedRaids;
            CompletedAdventures = Config.CompletedAdventures;
        }

        public void AddCompletedTrade()
        {
            Interlocked.Increment(ref CompletedTrades);
            Config.CompletedTrades = CompletedTrades;
        }

        public void AddCompletedGiveaways()
        {
            Interlocked.Increment(ref CompletedGiveaways);
            Config.CompletedGiveaways = CompletedGiveaways;
        }
        public void AddCompletedEggs()
        {
            Interlocked.Increment(ref CompletedEggs);
            Config.CompletedEggs = CompletedEggs;
        }

        public void AddCompletedFossils()
        {
            Interlocked.Increment(ref CompletedFossils);
            Config.CompletedFossils = CompletedFossils;
        }

        public void AddCompletedEncounters()
        {
            Interlocked.Increment(ref CompletedEncounters);
            Config.CompletedEncounters = CompletedEncounters;
        }

        public void AddCompletedLegends()
        {
            Interlocked.Increment(ref CompletedLegends);
            Config.CompletedLegends = CompletedLegends;
        }

        public void AddCompletedSeedCheck()
        {
            Interlocked.Increment(ref CompletedSeedChecks);
            Config.CompletedSeedChecks = CompletedSeedChecks;
        }

        public void AddCompletedSurprise()
        {
            Interlocked.Increment(ref CompletedSurprise);
            Config.CompletedSurprise = CompletedSurprise;
        }

        public void AddCompletedDistribution()
        {
            Interlocked.Increment(ref CompletedDistribution);
            Config.CompletedDistribution = CompletedDistribution;
        }

        public void AddCompletedClones()
        {
            Interlocked.Increment(ref CompletedClones);
            Config.CompletedClones = CompletedClones;
        }

        public void AddCompletedFixOTs()
        {
            Interlocked.Increment(ref CompletedFixOTs);
            Config.CompletedFixOTs = CompletedFixOTs;
        }

        public void AddCompletedSpecialRequests()
        {
            Interlocked.Increment(ref CompletedSpecialRequests);
            Config.CompletedSpecialRequests = CompletedSpecialRequests;
        }

        public void AddCompletedTradeCords()
        {
            Interlocked.Increment(ref CompletedTradeCords);
            Config.CompletedTradeCords = CompletedTradeCords;
        }

        public void AddCompletedRaids()
        {
            Interlocked.Increment(ref CompletedRaids);
            Config.CompletedRaids = CompletedRaids;
        }
        public void AddCompletedAdventures()
        {
            Interlocked.Increment(ref CompletedAdventures);
            Config.CompletedAdventures = CompletedAdventures;
        }

        public void AddCompletedDumps()
        {
            Interlocked.Increment(ref CompletedDumps);
            Config.CompletedDumps = CompletedDumps;
        }

        public IEnumerable<string> Summary()
        {
            if (CompletedSeedChecks != 0)
                yield return $"Seed Check Trades: {CompletedSeedChecks}";
            if (CompletedClones != 0)
                yield return $"Clone Trades: {CompletedClones}";
            if (CompletedFixOTs != 0)
                yield return $"FixOT Trades: {CompletedFixOTs}";
            if (CompletedSpecialRequests != 0)
                yield return $"SpecialRequest Trades: {CompletedSpecialRequests}";
            if (CompletedTradeCords != 0)
                yield return $"TradeCord Trades: {CompletedTradeCords}";
            if (CompletedDumps != 0)
                yield return $"Dump Trades: {CompletedDumps}";
            if (CompletedTrades != 0)
                yield return $"Link Trades: {CompletedTrades}";
            if (CompletedGiveaways != 0)
                yield return $"Giveaways: {CompletedGiveaways}";
            if (CompletedDistribution != 0)
                yield return $"Distribution Trades: {CompletedDistribution}";
            if (CompletedSurprise != 0)
                yield return $"Surprise Trades: {CompletedSurprise}";
            if (CompletedEggs != 0)
                yield return $"Eggs Received: {CompletedEggs}";
            if (CompletedRaids != 0)
                yield return $"Completed Raids: {CompletedRaids}";
            if (CompletedAdventures != 0)
                yield return $"Completed Adventures: {CompletedAdventures}";
            if (CompletedFossils != 0)
                yield return $"Completed Fossils: {CompletedFossils}";
            if (CompletedEncounters != 0)
                yield return $"Wild Encounters: {CompletedEncounters}";
            if (CompletedLegends != 0)
                yield return $"Legendary Encounters: {CompletedLegends}";
        }
    }
}