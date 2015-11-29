namespace ExorCorki
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
            /*
                Q Harass/Farm Logic;
                Q KillSteal Logic;
                Q Immobile Harass Logic;
            */
            if (Variables.Q.IsReady() &&
                ((Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqharassfarm").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededQMana) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqks").GetValue<bool>() && Variables.Q.GetDamage(Targets.Target) > Targets.Target.Health) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqimmobile").GetValue<bool>() && Bools.IsImmobile(Targets.Target))))
            {
                Variables.Q.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
            }

            /*
                E Combo Logic;
                E Harass Logic;
            */
            if (Variables.E.IsReady() &&
                ((Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useecombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useeharassfarm").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededEMana)))
            {
                Variables.E.Cast();
            }

            /*
                R KillSteal Logic;
                R AutoHarass Logic;
            */
            if (Variables.R.IsReady() &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userks").GetValue<bool>() && Variables.R.GetDamage(Targets.Target) > Targets.Target.Health) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userharass").GetValue<bool>() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.rwhitelist.{Targets.Target.ChampionName.ToLower()}").GetValue<bool>() &&
                    ObjectManager.Player.ManaPercent > ManaManager.NeededRMana))
            {
                Variables.R.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
            }
        }

        public static void ExecuteModes(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var tg = args.Target as Obj_AI_Hero;

            /*
                Q Combo Logic;
            */
            if (Variables.Q.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqcombo").GetValue<bool>())
            {
                Variables.Q.CastIfHitchanceEquals(tg, HitChance.VeryHigh, false);
            }

            /*
                R Combo Logic;
            */
            if (Variables.R.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.usercombo").GetValue<bool>())
            {
                Variables.R.CastIfHitchanceEquals(tg, HitChance.VeryHigh, false);
            }
        }

        public static void ExecuteFarm(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /*
                Q Farm Logic;
            */
            if (Variables.Q.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqharassfarm").GetValue<bool>() &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededQMana &&
                Variables.Q.GetLineFarmLocation(Targets.Minions, Variables.Q.Width).MinionsHit >= 3)
            {
                var QFarmPosition = Variables.Q.GetLineFarmLocation(Targets.Minions, Variables.Q.Width).Position;
                Variables.Q.Cast(QFarmPosition);
            }

            /*
                E Farm Logic;
            */
            if (Variables.E.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useeharassfarm").GetValue<bool>() &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededEMana &&
                Variables.E.GetLineFarmLocation(Targets.Minions, Variables.E.Width).MinionsHit >= 3)
            {
                Variables.E.Cast();
            }

            /*
                R Farm Logic;
            */
            if (Variables.R.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userfarm").GetValue<bool>() &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededRMana &&
                Variables.R.GetLineFarmLocation(Targets.Minions, Variables.R.Width).MinionsHit >= 2)
            {
                var RFarmPosition = Variables.R.GetLineFarmLocation(Targets.Minions, Variables.R.Width).Position;
                Variables.Q.Cast(RFarmPosition);
            }
        }
    }
}
