using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorTristana
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Tristana")
            {
                return;
            }

            Tristana.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Tristana</b></font> Loaded!");
        }
    }
}