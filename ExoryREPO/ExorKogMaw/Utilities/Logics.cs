namespace ExorKogMaw
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
            // W Orbwalking Limiter
            Variables.Orbwalker.SetMovement(Bools.ShouldOrbwalk());

            // Q KillSteal Logic + Immobile Harass Logic.
            if (Variables.Q.IsReady() &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqks").GetValue<bool>() && Targets.Target.Health < Variables.Q.GetDamage(Targets.Target)) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqimmobile").GetValue<bool>() && Bools.IsImmobile(Targets.Target)))
            {
                Variables.Q.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
            }

            // E KillSteal Logic + Immobile Harass Logic.
            if (Variables.E.IsReady() &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useeks").GetValue<bool>() && Targets.Target.Health < Variables.E.GetDamage(Targets.Target)) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useeimmobile").GetValue<bool>() && Bools.IsImmobile(Targets.Target)))
            {
                Variables.E.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
            }

            // R Combo Logic + KillSteal Logic + Farm Logic.
            if (Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.user").GetValue<bool>() &&
                Variables.R.IsReady() &&
                !ObjectManager.Player.IsWindingUp)
            {
                if (Targets.Target.HealthPercent < 50 &&
                    (Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                        Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.rcombokeepstacks").GetValue<Slider>().Value >= Variables.GetRStacks()) ||
                        (Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userks").GetValue<bool>() && Targets.Target.Health < Variables.R.GetDamage(Targets.Target)))
                {
                    Variables.R.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
                    return;
                }
                else if (Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userharassfarm").GetValue<bool>() &&
                    Variables.Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo &&
                    Variables.R.GetCircularFarmLocation(Targets.Minions, Variables.R.Width).MinionsHit >= 3 &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.rfarmkeepstacks").GetValue<Slider>().Value >= Variables.GetRStacks())
                {
                    var RFarm = Variables.R.GetCircularFarmLocation(Targets.Minions, Variables.R.Width);
                    {
                        Variables.R.Cast(RFarm.Position);
                        return;
                    }
                }
            }
        }

        public static void ExecuteModes(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            // Q Combo Logic.
            var tg = args.Target as Obj_AI_Hero;
            if (Variables.Q.IsReady() &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqcombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqharass").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededQMana))
            {
                Variables.Q.CastIfHitchanceEquals(tg, HitChance.VeryHigh, false);
            }

            // W Combo Logic.
            if (Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewcombo").GetValue<bool>() &&
                Variables.W.IsReady() &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                Variables.W.Cast();
                return;
            }

            // E Combo Logic.
            if (Variables.E.IsReady() &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useecombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useeharassfarm").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededEMana))
            {
                Variables.E.CastIfHitchanceEquals(tg, HitChance.VeryHigh, false);
            }
        }

        public static void ExecuteFarm(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            // E Farming Logic.
            if (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useeharassfarm").GetValue<bool>() &&
                Variables.E.GetLineFarmLocation(Targets.Minions, Variables.E.Width).MinionsHit >= 3 &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededEMana)
            {
                var EFarm = Variables.E.GetLineFarmLocation(Targets.Minions, Variables.E.Width);
                {
                    Variables.E.Cast(EFarm.Position);
                    return;
                }
            }
        }
    }    
}
