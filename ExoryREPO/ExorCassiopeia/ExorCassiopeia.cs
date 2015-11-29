namespace ExorCassiopeia
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The main class.
    /// </summary>
    public class Cassiopeia
    {   
        /// <summary>
        /// Called when the game loads itself.
        /// </summary>
        public static void OnLoad()
        {
            Settings.SetSpells();
            Settings.SetMenu();
            Settings.SetMethods();
            Drawings.Load();
        }

        /// <summary>
        /// Called when the game updates itself.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void Game_OnGameUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead)
            {
                if (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
                {
                    Logics.ExecuteTearStacking(args);
                }

                if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                    Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.LaneClear &&
                    Targets.Target != null &&
                    Targets.Target.IsValid)
                {
                    Logics.ExecuteModes(args);
                }

                if (Targets.Minions != null &&
                    Targets.Minions.Any() &&
                    Targets.Minion.IsValid &&
                    Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                    Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
                {
                    Logics.ExecuteFarm(args);
                }
            }
        }
    }
}
