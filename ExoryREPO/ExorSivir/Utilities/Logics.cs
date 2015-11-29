namespace ExorSivir
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    public class Logics
    {
        public static void ExecuteAuto(EventArgs args)
        {
            /// <summary>
            /// The Q Harass Logic.
            /// The Q KillSteal Logic.
            /// The Q Immobile Harass Logic.
            /// </summary>
            if (Variables.Q.IsReady() &&
                ((Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqharassfarm").GetValue<bool>() &&
                    ObjectManager.Player.ManaPercent > ManaManager.NeededQMana &&
                    Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo) ||
                    
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqks").GetValue<bool>() &&
                    (Variables.Q.GetDamage(Targets.Target)*2) > Targets.Target.Health) ||
                    
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqimmobile").GetValue<bool>() &&
                    Bools.IsImmobile(Targets.Target))))
            {
                Variables.Q.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
            }
        }

        public static void ExecuteShield(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /// <summary>
            /// The E Logic.
            /// </summary>
            if (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useelogic").GetValue<bool>())
            {
                
            }
        }

        public static void ExecuteModes(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /// <summary>
            /// The W Combo Logic.
            /// The W Harass Logic.
            /// </summary>
            if (Variables.W.IsReady() &&
                ((Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewcombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewharassfarm").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededWMana)))
            {
                Orbwalking.ResetAutoAttackTimer();
                Variables.W.Cast();
            }

            /// <summary>
            /// The Q Combo Logic.
            /// </summary>
            if (Variables.Q.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqcombo").GetValue<bool>())
            {
                Variables.Q.CastIfHitchanceEquals((Obj_AI_Hero)args.Target, HitChance.VeryHigh, false);
            }

        }

        public static void ExecuteFarm(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /// <summary>
            /// The W Farm Logic.
            /// </summary>
            if (Variables.W.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewharassfarm").GetValue<bool>() &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededWMana &&
                Variables.Q.GetLineFarmLocation(Targets.Minions, Variables.Q.Width).MinionsHit >= 3)
            {
                Orbwalking.ResetAutoAttackTimer();
                Variables.W.Cast();
            }

            /// <summary>
            /// The Q Farm Logic.
            /// </summary>
            if (Variables.Q.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqharassfarm").GetValue<bool>() &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededQMana &&
                Variables.Q.GetLineFarmLocation(Targets.Minions, Variables.Q.Width).MinionsHit >= 3)
            {
                var QFarmPosition = Variables.Q.GetLineFarmLocation(Targets.Minions, Variables.Q.Width).Position;
                Variables.Q.Cast(QFarmPosition);
            }
        }
    }
}
