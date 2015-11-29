namespace ExorAshe
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
                Q Combo Logic;
                Q Farm Logic;
            */
            if (Variables.Q.IsReady() &&
                ObjectManager.Player.HasBuff("AsheQCastReady") &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqcombo").GetValue<bool>() && Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqharassfarm").GetValue<bool>() && ObjectManager.Player.ManaPercent > ManaManager.NeededQMana))
            {
                Variables.Q.Cast();
                return;
            }

            /*
                W Immobile Harass Logic;
                W KillSteal Logic;
            */
            if (Variables.W.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewks").GetValue<bool>() && Targets.Target.Health < Variables.W.GetDamage(Targets.Target) ||
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewimmobile").GetValue<bool>() && Bools.IsImmobile(Targets.Target))
            {
                Variables.W.Cast(Targets.Target.Position);
            }
            
            /*
                R Doublelift Mechanic;
                R KillSteal Logic;
            */
            if (Variables.R.IsReady())
            {
                if (Variables.E.IsReady() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.usermechanic").GetValue<bool>() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.rwhitelist.{Targets.Target.ChampionName.ToLower()}").GetValue<bool>())
                {
                    Variables.E.Cast(Targets.Target.Position);
                    Variables.R.Cast(Targets.Target.Position);
                    return;
                }

                if (Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userks").GetValue<bool>() &&
                    Targets.Target.Health < Variables.R.GetDamage(Targets.Target) &&
                    (!Variables.W.IsReady() || !Targets.Target.IsValidTarget(Variables.W.Range)))
                {
                    Variables.R.CastIfHitchanceEquals(Targets.Target, HitChance.VeryHigh, false);
                }
            }
        }

        public static void ExecuteModes(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /*
                W Combo Logic;
            */
            if (Variables.W.IsReady() && 
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewcombo").GetValue<bool>())
            {
                Variables.W.Cast(args.Target.Position);
                return;
            }
            
            /*
                R Combo Logic;
            */
            if (Variables.R.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.usercombo").GetValue<bool>() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.rwhitelist.{((Obj_AI_Hero)args.Target).ChampionName.ToLower()}").GetValue<bool>())
            {
                Variables.R.Cast(args.Target.Position);
            }
        }

        public static void ExecuteFarm(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /*
                W Farm Logic;
            */
            if (Variables.W.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewharassfarm").GetValue<bool>() &&
                ObjectManager.Player.ManaPercent > ManaManager.NeededWMana)
            {
                Variables.W.Cast(args.Target.Position);
            }
        }
    }
}
