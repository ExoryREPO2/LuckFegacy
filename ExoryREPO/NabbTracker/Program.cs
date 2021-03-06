﻿using System;

using LeagueSharp;
using LeagueSharp.Common;

namespace NabbTracker
{
    class Program
    {
        public static Track Track;
        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            Track = new Track();
            Game.PrintChat("Nabb<font color=\"#228B22\">Tracker</font> - Loaded!");
            VersionUpdater.UpdateCheck();
        }
    }
}