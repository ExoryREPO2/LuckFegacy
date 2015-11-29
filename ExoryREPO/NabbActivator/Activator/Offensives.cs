namespace NabbActivator
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;
    
    using ItemData = LeagueSharp.Common.Data.ItemData;

    /// <summary>
    /// The offensive items class.
    /// </summary>
    public class Offensives
    {
        public static void Execute(EventArgs args)
        {
            /// <summary>
            /// The Muramana.
            /// </summary>
            if (ItemData.Muramana.GetItem().IsReady() &&
                ((!ObjectManager.Player.HasBuff("muramana") && ObjectManager.Player.IsWindingUp) ||
                (ObjectManager.Player.HasBuff("muramana") && !ObjectManager.Player.IsWindingUp)))
            {
                ItemData.Muramana.GetItem().Cast();
            }
            
            /// <summary>
            /// The Frost Queen's Claim.
            /// </summary>
            if (ItemData.Frost_Queens_Claim.GetItem().IsReady() &&
                ObjectManager.Player.CountAlliesInRange(1000) > 2 &&
                ObjectManager.Player.CountEnemiesInRange(1000) > 1)
            {
                ItemData.Frost_Queens_Claim.GetItem().Cast();
            }

            /// <summary>
            /// The Youmuu's Ghostblade.
            /// </summary>
            if (ItemData.Youmuus_Ghostblade.GetItem().IsReady() &&
                ObjectManager.Player.IsWindingUp)
            {
                ItemData.Youmuus_Ghostblade.GetItem().Cast();
            }

            /// <summary>
            /// The Bilgewater Cutlass.
            /// </summary>
            if (ItemData.Bilgewater_Cutlass.GetItem().IsReady() &&
                Targets.target.IsValidTarget(550f))
            {
                ItemData.Bilgewater_Cutlass.GetItem().Cast(Targets.target);
            }

            /// <summary>
            /// The Hextech Gunblade.
            /// </summary>
            if (ItemData.Hextech_Gunblade.GetItem().IsReady() &&
                Targets.target.IsValidTarget(550f))
            {
                ItemData.Hextech_Gunblade.GetItem().Cast(Targets.target);
            }

            /// <summary>
            /// The Blade of the Ruined King.
            /// </summary>    
            if (ItemData.Blade_of_the_Ruined_King.GetItem().IsReady() &&
                Targets.target.IsValidTarget(550f) &&
                ObjectManager.Player.HealthPercent <= 90)
            {
                ItemData.Blade_of_the_Ruined_King.GetItem().Cast(Targets.target);
            }

            /// <summary>
            /// The Entropy.
            /// </summary>     
            if (ItemData.Entropy.GetItem().IsReady() &&
                ObjectManager.Player.IsWindingUp)
            {            
                ItemData.Entropy.GetItem().Cast();
            }

            /// <summary>
            /// The Odyn's Veil.
            /// </summary>  
            if (ItemData.Odyns_Veil.GetItem().IsReady() &&
                HealthPrediction.GetHealthPrediction(ObjectManager.Player, (int)(500 + Game.Ping / 2f)) <= ObjectManager.Player.Health - 300)
            {
                ItemData.Odyns_Veil.GetItem().Cast();
            }
        }
    }
}
