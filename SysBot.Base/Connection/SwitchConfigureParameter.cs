﻿namespace SysBot.Base
{
    public enum SwitchConfigureParameter
    {
        /// <summary>
        /// Amount of time (milliseconds) the sysmodule sleeps between loops.
        /// </summary>
        mainLoopSleepTime,

        /// <summary>
        /// Amount of time (milliseconds) the sysmodule holds a button before releasing when <see cref="SwitchCommand.Click"/> is requested.
        /// </summary>
        buttonClickSleepTime,

        /// <summary>
        /// sys-botbase Echoes the command request back after processing the command if this is set.
        /// </summary>
        echoCommands,
    }
}