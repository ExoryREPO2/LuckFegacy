using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorGraves
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Graves")
            {
                return;
            }

            Graves.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Graves</b></font> Loaded!");
        }
    }
}