using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace ExorRenekton
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.CharData.BaseSkinName != "Renekton")
            {
                return;
            }

            Renekton.OnLoad();
            Game.PrintChat("Exor<font color='#FFF000'><b>Renekton</b></font> Loaded!");
        }
    }
}