namespace NabbCleanser
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;

    using LeagueSharp;
    using LeagueSharp.Common;
    
    /// <summary>
    ///    The variables.
    /// </summary>
    public class Variables
    {
        public static Menu Menu { get; set; }
        public static Menu MikaelsMenu { get; set; }
        public static Random rand;
        public static SpellSlot CleanseSpellSlot = ObjectManager.Player.GetSpellSlot("summonerboost");   
        
        public static readonly int Mikaels_Crucible = 3222;
        public static int[] CleanserItems = new[]
        {
            3137, // Dervish Blade,
            3139, // Mercurial Scimitar,
            3140, // Quicksilver Sash,
            3222  // Mikaels Crucible.
        };
    }

    /// <summary>
    ///    The bools.
    /// </summary>
    public class Bools
    {
        public static bool HasNoProtection(Obj_AI_Hero target)
        {
            return !target.HasBuffOfType(BuffType.SpellShield) 
                && !target.HasBuffOfType(BuffType.SpellImmunity);
        }

        public static bool ShouldUseCleanse(Obj_AI_Hero target)
        {
            return target.HasBuffOfType(BuffType.Charm)
                || target.HasBuffOfType(BuffType.Flee)
                || target.HasBuffOfType(BuffType.Polymorph)
                || target.HasBuffOfType(BuffType.Snare)
                || target.HasBuffOfType(BuffType.Stun)
                || target.HasBuffOfType(BuffType.Taunt)
                || target.HasBuff("summonerexhaust")
                || target.HasBuff("summonerdot");
        }

        public static bool ShouldUseCleanser()
        {
            return ObjectManager.Player.HasBuff("zedulttargetmark")
                || ObjectManager.Player.HasBuff("VladimirHemoplague")
                || ObjectManager.Player.HasBuff("MordekaiserChildrenOfTheGrave")
                || ObjectManager.Player.HasBuff("PoppyDiplomaticImmunity")
                || ObjectManager.Player.HasBuff("FizzMarinerDoom")
                || ObjectManager.Player.HasBuffOfType(BuffType.Suppression);
        }

        public static bool IsCleanseNotAvailable()
        {
            return (Variables.CleanseSpellSlot == SpellSlot.Unknown || ObjectManager.Player.Spellbook.CanUseSpell(Variables.CleanseSpellSlot) != SpellState.Ready);
        }
    }
    
    /// <summary>
    ///    Other utilities.
    /// </summary>
    public class Others
    {
        public static int GetCleanserItem()
        {
            for (int i = 1; i < Variables.CleanserItems.Length; i++)
            {
                if (Items.HasItem(Variables.CleanserItems[i]))
                {
                    return Variables.CleanserItems[i];
                }
            }
            
            return 0;
        }

        public static void BuildMikaelsMenu(Menu Menu)
        {
            Variables.MikaelsMenu = new Menu("Mikaels Options", "use.mikaelsmenu");
            {
                foreach (var Allies in HeroManager.Allies.Select(hero => hero.CharData.BaseSkinName).ToList())
                {
                    Variables.MikaelsMenu.AddItem(new MenuItem("use.mikaels.{Allies.ToLowerInvariant()}", Allies).SetValue(true));
                }
                Variables.MikaelsMenu.AddItem(new MenuItem("enable.mikaels", "Enable Mikaels Usage").SetValue(true));
            }
            
            Variables.Menu.AddSubMenu(Variables.MikaelsMenu);
        }
    }    
}
