using System;

using LeagueSharp;
using LeagueSharp.Common;

namespace NabbStealther
{
    class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnLoad;
        }

        private static void Game_OnLoad(EventArgs args)
        {
            Stealther.OnLoad();
            Game.PrintChat("Nabb<font color=\"#ee82ee\">Stealther</font> - Loaded!");
            VersionUpdater.UpdateCheck();
        }
    }
}