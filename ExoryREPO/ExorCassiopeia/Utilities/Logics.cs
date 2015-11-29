namespace ExorCassiopeia
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    public class Logics
    {
        public static void ExecuteTearStacking(EventArgs args)
        {
            /*
                Tear Stacking Logic;
            */
            if (Variables.Q.IsReady() &&
                Bools.HasTear() &&
                !ObjectManager.Player.IsRecalling() &&
                ObjectManager.Player.CountEnemiesInRange(1500) == 0 &&
                Variables.Menu.Item($"{Variables.MainMenuName}.misc.stacktear").GetValue<bool>() &&
                ObjectManager.Player.ManaPercent > Variables.Menu.Item($"{Variables.MainMenuName}.misc.stacktearmana").GetValue<Slider>().Value)
            {
                Variables.Q.Cast(Game.CursorPos);
            }
        }

        public static void ExecuteModes(EventArgs args)
        {
            /*
                No AA In combo.
            */
            if (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                Variables.Orbwalker.SetAttack(Variables.Menu.Item($"{Variables.MainMenuName}.misc.aa").GetValue<bool>());
            }
            
            /*
                E Combo Logic;
            */
            if (Variables.E.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useecombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                Targets.Target.HasBuffOfType(BuffType.Poison))
            {
                Utility.DelayAction.Add(Variables.Menu.Item($"{Variables.MainMenuName}.esettings.edelay").GetValue<Slider>().Value,
                () =>
                    {
                        Variables.E.CastOnUnit(Targets.Target);
                    }
                ); 
            }

            /*
                R Combo Logic;
            */
            if (Variables.R.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.usercombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                (ObjectManager.Player.HealthPercent < 20 || Targets.RTargets.Count() >= Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userminenemies").GetValue<Slider>().Value))
            {
                Variables.R.Cast(Targets.Target.Position);
            }

            /*
                Q Combo Logic;
            */
            if (Variables.Q.IsReady() &&
                !Targets.Target.HasBuffOfType(BuffType.Poison) &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqcombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqharassfarm").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededQMana))
            {
                Variables.Q.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
                return;
            }

            /*
                W Combo Logic;
            */
            if (Variables.W.IsReady() &&
                !Variables.Q.IsReady() &&
                !Targets.Target.HasBuffOfType(BuffType.Poison) &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewcombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewharassfarm").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededWMana))
            {
                Variables.W.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
            }
        }

        public static void ExecuteFarm(EventArgs args)
        {
            /*
                Q Farm Logic;
            */
            if (Variables.Q.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqharassfarm").GetValue<bool>() &&
                Variables.Q.GetCircularFarmLocation(Targets.Minions, 100).MinionsHit >= 3 &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededQMana &&
                !Targets.Minion.HasBuffOfType(BuffType.Poison))
            {
                var QFarmPosition = Variables.Q.GetLineFarmLocation(Targets.Minions, Variables.Q.Width).Position;
                Variables.Q.Cast(QFarmPosition);
            }

            /*
                W Farm Logic;
            */
            if (Variables.W.IsReady() &&
                !Variables.Q.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewharassfarm").GetValue<bool>() &&
                Variables.W.GetCircularFarmLocation(Targets.Minions, 150).MinionsHit >= 3 &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededWMana &&
                !Targets.Minion.HasBuffOfType(BuffType.Poison))
            {
                var WFarmPosition = Variables.W.GetCircularFarmLocation(Targets.Minions, Variables.W.Width).Position;
                Variables.W.Cast(WFarmPosition);
            }   

            /*
                E Farm Logic;
            */
            if (Variables.E.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useefarm").GetValue<bool>() &&
                Variables.E.GetDamage(Targets.Minion) > Targets.Minion.Health &&
                (Targets.Minion.HasBuffOfType(BuffType.Poison) || Variables.Menu.Item($"{Variables.MainMenuName}.misc.lasthitnopoison").GetValue<bool>()))
            {
                Utility.DelayAction.Add(Variables.Menu.Item($"{Variables.MainMenuName}.esettings.edelay").GetValue<Slider>().Value, 
                () =>
                    {
                        Variables.E.CastOnUnit(Targets.Minion);
                    }
                );
            }
        }
    }
}
