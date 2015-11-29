namespace NabbActivator
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;
    
    using ItemData = LeagueSharp.Common.Data.ItemData;
 
    /// <summary>
    /// The defensive items class.
    /// </summary>
    public class Defensives
    {        
        public static void Execute(EventArgs args)
        {
            /// <summary>
            /// The Guardian's Horn.
            /// </summary>
            if (ItemData.Guardians_Horn.GetItem().IsReady() &&
                ObjectManager.Player.CountAlliesInRange(600) > 2 &&
                ObjectManager.Player.CountEnemiesInRange(1000) > 1)
            {
                ItemData.Guardians_Horn.GetItem().Cast();
            }

            /// <summary>
            /// The Twin Shadows.
            /// </summary>
            if (ItemData.Twin_Shadows2.GetItem().IsReady() &&
                ObjectManager.Player.CountAlliesInRange(1000) > 2 &&
                ObjectManager.Player.CountEnemiesInRange(1000) > 1)
            {
                ItemData.Twin_Shadows2.GetItem().Cast();
            }

            /// <summary>
            /// The Seraph's Embrace.
            /// </summary>    
            if (ItemData.Seraphs_Embrace.GetItem().IsReady() &&
                HealthPrediction.GetHealthPrediction(ObjectManager.Player, (int)(250 + Game.Ping / 2f)) <= ObjectManager.Player.MaxHealth/4)
            {
                ItemData.Seraphs_Embrace.GetItem().Cast();
            }

            /// <summary>
            /// The Banner of Command.
            /// </summary> 
            if (ItemData.Banner_of_Command.GetItem().IsReady() &&
                Targets.banTarget != null)
            {
                ItemData.Banner_of_Command.GetItem().Cast(Targets.banTarget);
            }

            /// <summary>
            /// The Talisman of Ascension.
            /// </summary> 
            if (ItemData.Talisman_of_Ascension.GetItem().IsReady() &&
                (ObjectManager.Player.CountEnemiesInRange(700) < ObjectManager.Player.CountAlliesInRange(700)) ||
                (ObjectManager.Player.CountEnemiesInRange(700) > ObjectManager.Player.CountAlliesInRange(700)))
            {
                ItemData.Talisman_of_Ascension.GetItem().Cast();
            }

            /// <summary>
            /// The Wooglet's Witchcap.
            /// </summary>             
            if (ItemData.Wooglets_Witchcap.GetItem().IsReady() &&
                HealthPrediction.GetHealthPrediction(ObjectManager.Player, (int)(250 + Game.Ping / 2f)) <= ObjectManager.Player.MaxHealth/4)
            {
                ItemData.Wooglets_Witchcap.GetItem().Cast();
            }

            /// <summary>
            /// The Randuin's Omen.
            /// </summary>             
            if (ItemData.Randuins_Omen.GetItem().IsReady() &&
                ObjectManager.Player.CountEnemiesInRange(500) > 1)
            {
                ItemData.Randuins_Omen.GetItem().Cast();
            }

            /// <summary>
            /// The Zhonya's Hourglass.
            /// </summary>             
            if (ItemData.Zhonyas_Hourglass.GetItem().IsReady() &&
                HealthPrediction.GetHealthPrediction(ObjectManager.Player, (int)(250 + Game.Ping / 2f)) <= ObjectManager.Player.MaxHealth/4)
            {
                ItemData.Zhonyas_Hourglass.GetItem().Cast();
            }

            /// <summary>
            /// The Locket of the Iron Solari.
            /// </summary>             
            if (ItemData.Locket_of_the_Iron_Solari.GetItem().IsReady() &&
                ObjectManager.Player.CountAlliesInRange(600) > 2 &&
                ObjectManager.Player.CountEnemiesInRange(1000) > 1)
            {
                ItemData.Locket_of_the_Iron_Solari.GetItem().Cast();
            }

            /// <summary>
            /// The Righteous Glory.
            /// </summary>             
            if (ItemData.Righteous_Glory.GetItem().IsReady() &&
                ObjectManager.Player.CountAlliesInRange(600) > 2 &&
                ObjectManager.Player.CountEnemiesInRange(1000) > 0)
            {
                ItemData.Righteous_Glory.GetItem().Cast();
            }
        }
    }
}
