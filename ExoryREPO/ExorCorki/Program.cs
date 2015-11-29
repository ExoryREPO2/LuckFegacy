using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorCorki
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Corki")
            {
                return;
            }

            Corki.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Corki</b></font> Loaded!");
        }
    }
}