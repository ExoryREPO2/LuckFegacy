namespace ExorSivir
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The main class.
    /// </summary>
    public class Sivir
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
            if (!ObjectManager.Player.IsDead &&
                Targets.Target != null &&
                Targets.Target.IsValid &&
                Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None)
            {
                Logics.ExecuteAuto(args);
            }
        }

        /// <summary>
        /// Called while processing Spellcast operations.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        public static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Variables.E.IsReady() &&
                sender.IsValid<Obj_AI_Hero>() &&
                sender != null &&
                sender.IsEnemy)
            {
                Logics.ExecuteShield(sender, args);
            }
        }

        /// <summary>
        /// Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        public static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe &&
                Orbwalking.IsAutoAttack(args.SData.Name) &&
                Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None)
            {
                if (args.Target.IsValid<Obj_AI_Hero>())
                {
                    Logics.ExecuteModes(sender, args);
                }
                else if (args.Target.IsValid<Obj_AI_Minion>())
                {
                    Logics.ExecuteFarm(sender, args);
                }
            }
        }
    }
}
