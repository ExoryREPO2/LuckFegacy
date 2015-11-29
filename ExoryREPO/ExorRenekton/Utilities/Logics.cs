namespace ExorRenekton
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ItemData = LeagueSharp.Common.Data.ItemData;

    public class Logics
    {
        public static void ExecuteAuto(EventArgs args)
        {
            /*
                Tiamat/Ravenous/Titanic Logic;
                Q Combo Logic;
                E Combo Logic;
            */
            if (!Variables.W.IsReady() &&
                !ObjectManager.Player.HasBuff("renektonpreexecute") &&
                Variables.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                if (Variables.Menu.Item($"{Variables.MainMenuName}.miscsettings.useresetters").GetValue<bool>())
                {
                    if (ItemData.Tiamat_Melee_Only.GetItem().IsReady())
                    {
                        ItemData.Tiamat_Melee_Only.GetItem().Cast();
                    }

                    if (ItemData.Ravenous_Hydra_Melee_Only.GetItem().IsReady())
                    {
                        ItemData.Ravenous_Hydra_Melee_Only.GetItem().Cast();
                    }

                    /*
                    if (ItemData.Titanic_Hydra_Melee_Only.GetItem().IsReady())
                    {
                        ItemData.Titanic_Hydra_Melee_Only.GetItem().Cast();
                    }
                    */
                }

                if (Variables.Q.IsReady() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqcombo").GetValue<bool>())
                {
                    Variables.Q.Cast();
                }
            }

            /*
                Q Harass/Farm Logic;
                Q KillSteal Logic;
                Q Immobile Harass Logic;
            */
            if (Variables.Q.IsReady() &&
                Targets.Target.IsValidTarget(Variables.Q.Range) &&
                ((Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqautoharass").GetValue<bool>() && ObjectManager.Player.ManaPercent >= 50) ||
                (Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqks").GetValue<bool>() && Variables.Q.GetDamage(Targets.Target) > Targets.Target.Health)))
            {
                Variables.Q.Cast();
            }

            /*
                R LifeSaver Logic;
            */
            if (Variables.R.IsReady() &&
                ObjectManager.Player.CountEnemiesInRange(700) > 0 &&
                (Variables.Menu.Item($"{Variables.MainMenuName}.rsettings.userlifesaver").GetValue<bool>() &&
                    HealthPrediction.GetHealthPrediction(ObjectManager.Player, (int)(250 + Game.Ping / 2f)) <= ObjectManager.Player.MaxHealth/4))
            {
                Variables.R.Cast();
            }
        }

        public static void ExecuteModes(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /*
                E Combo Logic;
            */
            if (Variables.E.IsReady() &&
                ((Obj_AI_Hero)args.Target).IsValidTarget(Variables.E.Range) &&
                !ObjectManager.Player.HasBuff("renektonsliceanddicedelay") &&
                Variables.Menu.Item($"{Variables.MainMenuName}.esettings.useecombo").GetValue<bool>())
            {
                Variables.E.Cast(Targets.Target.Position);
                return;
            }

            /*
                W Combo Logic;
            */
            if (Variables.W.IsReady() &&
                ((Obj_AI_Hero)args.Target).IsValidTarget(Variables.W.Range) &&
                Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.usewcombo").GetValue<bool>())
            {
                Orbwalking.ResetAutoAttackTimer();
                Variables.W.Cast();
            }
        }

        public static void ExecuteFarm(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            /*
                Q Farm Logic;
            */
            if (Variables.Q.IsReady() &&
                Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.useqfarm").GetValue<bool>() &&
                Targets.Minions.Count() >= 3)
            {
                Variables.Q.Cast();
            }
        }
    }
}
