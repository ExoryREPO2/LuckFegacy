namespace ExorTristana
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///     The main class.
    /// </summary>
    public class Tristana
    {
        public static void OnLoad()
        {
            Settings.SetSpells();
            Settings.SetMenu();
            Settings.SetMethods();
            Drawings.Load();
        }

        /// <summary>
        /// Called before the next aa is fired.
        /// </summary>
        /// <param name="args">The <see cref="Orbwalking.BeforeAttackEventArgs"/> instance containing the beforeattack data.</param>
        public static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (args.Target != null &&
                args.Target.IsValid<Obj_AI_Base>())
            {    
                Logics.ExecuteBeforeAttack(args);
            }
        }

        /// <summary>
        /// Called when the game updates itself.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void Game_OnGameUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead &&
                Targets.Target != null &&
                Targets.Target.IsValid)
            {
                Logics.ExecuteAuto(args);
            }
        }

        /// <summary>
        /// Called on do-cast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        public static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None &&
                sender.IsMe &&
                Orbwalking.IsAutoAttack(args.SData.Name))
            {
                if (args.Target.IsValid<Obj_AI_Hero>())
                {
                    Logics.ExecuteModes(sender, args);
                }
                else if (args.Target.IsValid<Obj_AI_Base>() &&
                    !(args.Target is Obj_AI_Hero))
                {
                    Logics.ExecuteFarm(sender, args);
                }
            }
        }
    }
}
