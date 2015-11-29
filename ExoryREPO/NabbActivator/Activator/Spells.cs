namespace NabbActivator
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;
    
    using ItemData = LeagueSharp.Common.Data.ItemData;
 
    /// <summary>
    /// The spells class.
    /// </summary>
    public class Spells
    {        
        public static void Execute(EventArgs args)
        {
            /// <summary>
            /// The Cleanse Logic.
            /// </summary>    
            if (Bools.HasNoProtection(ObjectManager.Player) &&
                Bools.ShouldUseCleanse(ObjectManager.Player))
            {
                if (ObjectManager.Player.ChampionName == "Gangplank" &&
                    Variables.W.IsReady())
                {
                    Variables.W.Cast();
                    return;
                }
                else if (Bools.IsSpellAvailable(SpellSlots.Cleanse))
                {
                    Utility.DelayAction.Add(
                        Bools.MustRandomize() ?
                            WeightedRandom.Next(100, 200) : 0,
                        () => 
                        {
                            ObjectManager.Player.Spellbook.CastSpell(SpellSlots.Cleanse);
                            return;
                        }
                    );
                }
            }

            /// <summary>
            /// The Clarity logic.
            /// </summary>
            if (Bools.IsSpellAvailable(SpellSlots.Clarity) &&
                ObjectManager.Player.ManaPercent <= 40)
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlots.Clarity);
            }

            /// <summary>
            /// The Ghost Logic.
            /// </summary>
            if (Bools.IsSpellAvailable(SpellSlots.Ghost) &&
                (ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange+300) < ObjectManager.Player.CountAlliesInRange(ObjectManager.Player.AttackRange+300)) ||
                (ObjectManager.Player.CountEnemiesInRange(ObjectManager.Player.AttackRange+300) > ObjectManager.Player.CountAlliesInRange(ObjectManager.Player.AttackRange+300)))
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlots.Ghost);
            }
            
            /*
            /// <summary>
            /// The Ignite Logic.
            /// </summary>
            if (Bools.IsSpellAvailable(SpellSlots.Ignite) &&
                Targets.target != null &&
                Targets.target.IsValidTarget(600f) &&
                ObjectManager.Player.GetSummonerSpellDamage(Targets.target, Damage.SpellSlot.Ignite) > Targets.target.Health)
            {
                ObjectManager.Player.Spellbook.CastSpell(SpellSlots.Ignite, Targets.target);
            }
            */
        }
    }
}
