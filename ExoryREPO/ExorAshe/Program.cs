using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorAshe
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Ashe")
            {
                return;
            }

            Ashe.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Ashe</b></font> Loaded!");
        }
    }
}
