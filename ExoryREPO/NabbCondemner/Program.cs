using System;

using LeagueSharp;
using LeagueSharp.Common;

namespace NabbCondemner
{
    class Program
    {
        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnLoad;
        }

        private static void Game_OnLoad(EventArgs args)
        {
            Condemner.OnLoad();
            Game.PrintChat("Nabb<font color=\"#696969\">Condemner</font> - Loaded!");
            VersionUpdater.UpdateCheck();
        }
    }
}