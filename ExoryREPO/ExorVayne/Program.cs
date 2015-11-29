using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorVayne
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Vayne")
            {
                return;
            }

            Vayne.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Vayne</b></font> Loaded!");
        }
    }
}
