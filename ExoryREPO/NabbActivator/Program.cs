using System;

using LeagueSharp;
using LeagueSharp.Common;

namespace NabbActivator
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            Activator.OnLoad();
            Game.PrintChat("Nabb<font color=\"#FF0000\">Activator</font> - Loaded!");
        }
    }
}
