namespace NabbActivator
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    /// The settings class.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The spells.
        /// </summary>
        public static void SetSpells()
        {
            Variables.W = new Spell(SpellSlot.W);
        }

        /// <summary>
        /// The menu.
        /// </summary>
        public static void SetMenu()
        {
            // Main Menu
            Variables.Menu = new Menu("NabbActivator", $"{Variables.MainMenuName}", true);
            {
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.offensives", "Enable Offensive Items")).SetValue(true);
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.defensives", "Enable Defensive Items")).SetValue(true);
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.spells", "Enable Spells")).SetValue(true);
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.cleansers", "Enable Cleansers")).SetValue(true);
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.consumables", "Enable Potions")).SetValue(true);
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.randomizer", "Enable NabbHumanizer")).SetValue(true);
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.separator", ""));
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.combo_button1", "!(Must be the same as your Combo key)!"));
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.combo_button", "Combo")).SetValue(new KeyBind(32, KeyBindType.Press));
                Variables.Menu.AddItem(new MenuItem($"{Variables.MainMenuName}.combo_button2", "!(Must be the same as your Combo key)!"));
                Variables.SliderMenu = new Menu("Consumables Options", $"{Variables.MainMenuName}.consumables.options");
                {
                    Variables.SliderMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.consumables.on_health_percent", "Use Health Potions if Health < x%").SetValue(new Slider(50, 0, 100)));
                    Variables.SliderMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.consumables.on_mana_percent", "Use Mana Potions if Mana < x%").SetValue(new Slider(50, 0, 100)));
                }
                Variables.Menu.AddSubMenu(Variables.SliderMenu);
            }
            Variables.Menu.AddToMainMenu();
        }

        /// <summary>
        /// The methods.
        /// </summary>
        public static void SetMethods()
        {
            Game.OnUpdate += Activator.Game_OnGameUpdate;
            Obj_AI_Base.OnDoCast += Activator.Obj_AI_Base_OnDoCast;
        }
    }
    /// <summary>
    /// The variables.
    /// </summary>
    public class Variables
    {
        public static Menu Menu, SliderMenu;
        public static Spell W;
        public static int MinHealthPercent => Variables.Menu.Item($"{Variables.MainMenuName}.consumables.on_health_percent").GetValue<Slider>().Value;
        public static int MinManaPercent => Variables.Menu.Item($"{Variables.MainMenuName}.consumables.on_mana_percent").GetValue<Slider>().Value;
        public static string MainMenuName => "nabbactivator.menu";
    }
    
    /// <summary>
    /// The spellslots.
    /// </summary>
    public class SpellSlots
    {
        public static SpellSlot Cleanse => ObjectManager.Player.GetSpellSlot("summonerboost");
        public static SpellSlot Heal => ObjectManager.Player.GetSpellSlot("summonerheal");
        public static SpellSlot Ignite => ObjectManager.Player.GetSpellSlot("summonerdot");
        public static SpellSlot Barrier => ObjectManager.Player.GetSpellSlot("summonerbarrier");
        public static SpellSlot Clarity => ObjectManager.Player.GetSpellSlot("summonermana");
        public static SpellSlot Exhaust => ObjectManager.Player.GetSpellSlot("summonerexhaust");
        public static SpellSlot Ghost => ObjectManager.Player.GetSpellSlot("summonerhaste");
    }
    
    /// <summary>
    /// The targets.
    /// </summary>
    public class Targets
    {
        public static Obj_AI_Hero target => TargetSelector.GetTarget(850f, TargetSelector.DamageType.Physical);
        public static Obj_AI_Minion banTarget => ObjectManager.Get<Obj_AI_Minion>()
            .Where(
                x =>
                    x.IsValidTarget(1200f, false) &&
                    x.CharData.BaseSkinName.Contains("siege"))
            .FirstOrDefault();
    }
    
    /// <summary>
    /// The bools.
    /// </summary>
    public class Bools
    {
        public static bool IsHealthPotRunning()
        =>
            ObjectManager.Player.HasBuff("ItemMiniRegenPotion") ||
            ObjectManager.Player.HasBuff("ItemCrystalFlask") ||
            ObjectManager.Player.HasBuff("RegenerationPotion");

        public static bool IsManaPotRunning()
        =>
            ObjectManager.Player.HasBuff("ItemCrystalFlask") ||
            ObjectManager.Player.HasBuff("FlaskOfCrystalWater");

        public static bool HasNoProtection(Obj_AI_Hero target)
        =>
            !target.HasBuffOfType(BuffType.SpellShield) &&
            !target.HasBuffOfType(BuffType.SpellImmunity);

        public static bool ShouldUseCleanse(Obj_AI_Hero target)
        =>  
            target.HasBuffOfType(BuffType.Charm) ||
            target.HasBuffOfType(BuffType.Flee) ||
            target.HasBuffOfType(BuffType.Polymorph) ||
            target.HasBuffOfType(BuffType.Snare) ||
            target.HasBuffOfType(BuffType.Stun) ||
            target.HasBuffOfType(BuffType.Taunt) ||
            target.HasBuff("summonerexhaust") ||
            target.HasBuff("summonerdot");

        public static bool ShouldUseCleanser()
        =>
            ObjectManager.Player.HasBuff("zedulttargetmark") ||
            ObjectManager.Player.HasBuff("VladimirHemoplague") ||
            ObjectManager.Player.HasBuff("MordekaiserChildrenOfTheGrave") ||
            ObjectManager.Player.HasBuff("PoppyDiplomaticImmunity") ||
            ObjectManager.Player.HasBuff("FizzMarinerDoom") ||
            ObjectManager.Player.HasBuffOfType(BuffType.Suppression);

        public static bool IsSpellAvailable(SpellSlot arg)
        =>
            arg != SpellSlot.Unknown &&
            ObjectManager.Player.Spellbook.CanUseSpell(arg) == SpellState.Ready;

        public static bool HasZedMark(Obj_AI_Hero target)
        =>
            target.HasBuff("zedulttargetmark");

        public static bool MustRandomize()
        => 
            Variables.Menu.Item($"{Variables.MainMenuName}.randomizer").GetValue<bool>();
    }
}
