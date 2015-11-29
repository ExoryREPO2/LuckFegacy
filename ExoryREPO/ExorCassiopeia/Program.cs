using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorCassiopeia
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Cassiopeia")
            {
                return;
            }

            Cassiopeia.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Cassiopeia</b></font> Loaded!");
        }
    }
}