using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorSivir
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Sivir")
            {
                return;
            }

            Sivir.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Sivir</b></font> Loaded!");
        }
    }
}