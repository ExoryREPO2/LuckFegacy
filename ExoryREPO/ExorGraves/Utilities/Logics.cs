namespace ExorGraves
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
                W Harass Logic;
                W KillSteal Logic;
            */
            if (Variables.W.IsReady() &&
                ((Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewharass").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededWMana) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewks").GetValue<bool>() && Variables.W.GetDamage(Targets.Target) > Targets.Target.Health)))
            {
                Variables.W.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
            }

            /*
                R KillSteal Logic;
            */
            if (Variables.R.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userks").GetValue<bool>() && Variables.R.GetDamage(Targets.Target) > Targets.Target.Health)
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
                W Combo Logic;
            */
            if (Variables.W.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewcombo").GetValue<bool>())
            {
                Variables.W.CastIfHitchanceEquals(tg, HitChance.VeryHigh, false);
            }

            /*
                E Reset Ammos + AAs Logic;
            */
            if (Variables.E.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useereset").GetValue<bool>() && ObjectManager.Player.HasBuff("gravesbasicattackammo1"))
            {
                Orbwalking.ResetAutoAttackTimer();
                Variables.E.Cast(Game.CursorPos);
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
        }
    }
}
