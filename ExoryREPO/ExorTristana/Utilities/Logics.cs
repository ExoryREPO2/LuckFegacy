namespace ExorTristana
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using SharpDX;

    public class Logics
    {
        public static void ExecuteBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            /// <summary>
            /// The E Logic.
            /// </summary>
            if (Variables.E.IsReady() &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useeauto").GetValue<bool>() &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.ewhitelist.{((Obj_AI_Hero)args.Target).ChampionName.ToLower()}").GetValue<bool>()) ||

                (Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useebuildings").GetValue<bool>() &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear &&
                args.Target.IsValid<Obj_AI_Turret>()))
            {
                Orbwalking.ResetAutoAttackTimer();
                Variables.E.CastOnUnit((Obj_AI_Base)args.Target);
            }
        }

        public static void ExecuteAuto(EventArgs args)
        {
            /// <summary>
            /// The Q Combo Logic.
            /// </summary>
            if (Variables.Q.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqauto").GetValue<bool>() &&
                (Bools.IsCharged(Targets.Target) || !Variables.E.IsReady()))
            {
                Variables.Q.Cast();
            }
        }

        public static void ExecuteModes(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /// <summary>
            /// The R KillSteal Logic.
            /// </summary>
            if (Variables.R.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userks").GetValue<bool>() &&
                KillSteal.Damage((Obj_AI_Hero)args.Target) > ((Obj_AI_Hero)args.Target).Health)
            {
                Orbwalking.ResetAutoAttackTimer();
                Variables.R.Cast((Obj_AI_Hero)args.Target);
                return;
            }
        }

        public static void ExecuteFarm(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /// <summary>
            /// The E Farm Logic.
            /// </summary>
            if (Variables.E.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useefarm").GetValue<bool>() &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear &&
                Targets.EMinions.Any())
            {
                Variables.E.Cast(Targets.EMinion);
            }
        }
    }
}
