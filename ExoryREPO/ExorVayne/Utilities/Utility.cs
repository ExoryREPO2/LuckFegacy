namespace ExorVayne
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
            Variables.Q = new Spell(SpellSlot.Q, ObjectManager.Player.AttackRange + 300f);
            Variables.E = new Spell(SpellSlot.E, 650f);
        }

        /// <summary>
        /// The menu.
        /// </summary>
        public static void SetMenu()
        {
            /// <summary>
            /// The main menu.
            /// </summary>
            Variables.Menu = new Menu("ExorVayne", $"{Variables.MainMenuName}", true);
            {
                /// <summary>
                /// The orbwalker menu.
                /// </summary>
                Variables.OrbwalkerMenu = new Menu("Orbwalker", $"{Variables.MainMenuName}.orbwalkermenu");
                {
                    Variables.Orbwalker = new Orbwalking.Orbwalker(Variables.OrbwalkerMenu);
                }
                Variables.Menu.AddSubMenu(Variables.OrbwalkerMenu);

                /// <summary>
                /// The settings menu.
                /// </summary>
                Variables.SettingsMenu = new Menu("Spell Menu", $"{Variables.MainMenuName}.settingsmenu");
                {
                    /// <summary>
                    /// The settings menu for the Q spell.
                    /// </summary>
                    Variables.QMenu = new Menu("Q Settings", $"{Variables.MainMenuName}.qsettingsmenu");
                    {
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqcombo", "Use Q in Combo")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqks", "Use Q to Automatically KillSteal")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqfarm", "Use Q LastHit Helper")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.qmana", "Use Q in Harass/Farm only if Mana >= x%"))
                            .SetValue(new Slider(50, 0, 99));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.QMenu);

                    /// <summary>
                    /// The settings menu for the E spell.
                    /// </summary>
                    Variables.EMenu = new Menu("E Settings", $"{Variables.MainMenuName}.esettingsmenu");
                    {
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useeauto", "Use E")).SetValue(true);
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useeks", "Use E to Automatically KillSteal")).SetValue(false);
                        {
                            /// <summary>
                            /// The whitelist menu for the E spell.
                            /// </summary>
                            Variables.WhiteListMenu = new Menu("Condemn Whitelist Menu", $"{Variables.MainMenuName}.esettings.ewhitelist");
                            {
                                foreach (Obj_AI_Hero champ in HeroManager.Enemies)
                                {
                                    Variables.WhiteListMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.ewhitelist.{champ.ChampionName.ToLower()}", $"Condemn: {champ.ChampionName}").SetValue(true));
                                }
                            }
                            Variables.EMenu.AddSubMenu(Variables.WhiteListMenu);
                        }
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.EMenu);
                }
                Variables.Menu.AddSubMenu(Variables.SettingsMenu);

                /// <summary>
                /// The settings menu for the E spell.
                /// </summary>
                Variables.MiscMenu = new Menu("E Settings", $"{Variables.MainMenuName}.miscsettingsmenu");
                {
                    Variables.MiscMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.miscsettings.noaastealth", "Use Don't AA when Stealthed Logic")).SetValue(false);
                }
                Variables.SettingsMenu.AddSubMenu(Variables.MiscMenu);

                /// <summary>
                /// The drawings menu.
                /// </summary>
                Variables.DrawingsMenu = new Menu("Drawings Menu", $"{Variables.MainMenuName}.drawingsmenu");
                {
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.q", "Show Q Range")).SetValue(true);
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.e", "Show E Range")).SetValue(true);
                }
                Variables.Menu.AddSubMenu(Variables.DrawingsMenu);
            }
            Variables.Menu.AddToMainMenu();
        }

        /// <summary>
        /// The methods.
        /// </summary>
        public static void SetMethods()
        {
            Game.OnUpdate += Vayne.Game_OnGameUpdate;
            Obj_AI_Base.OnDoCast += Vayne.Obj_AI_Base_OnDoCast;
        }
    }

    /// <summary>
    ///    The variables class.
    /// </summary>
    public class Variables
    {
        public static Menu Menu, OrbwalkerMenu, SettingsMenu, QMenu, EMenu, MiscMenu, WhiteListMenu, DrawingsMenu;
        public static Spell Q, E;
        public static Orbwalking.Orbwalker Orbwalker { get; set; }
        public static string MainMenuName => $"exor.{ObjectManager.Player.ChampionName.ToLower()}";
        public static float TotalAttackDamage(Obj_AI_Hero target)
        =>
            target.BaseAttackDamage + target.FlatPhysicalDamageMod;
    }

    /// <summary>
    ///    The Mana manager class.
    /// </summary>
    public class ManaManager
    {
        public static int NeededQMana => Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.qmana").GetValue<Slider>().Value;
    }

    /// <summary>
    ///    The targets class.
    /// </summary>
    public class Targets
    {
        public static Obj_AI_Hero Target => TargetSelector.GetTarget(Variables.E.Range, TargetSelector.DamageType.Physical);
        public static Obj_AI_Base FarmMinion
        =>
            MinionManager.GetMinions(ObjectManager.Player.Position, Variables.Q.Range)
                .Where(
                    minions =>
                        minions.Health < Variables.TotalAttackDamage(ObjectManager.Player) * 1.3)
                .First();
    }

    /// <summary>
    ///    The bools class.
    /// </summary>
    public class Bools
    {
        public static bool IsImmobile(Obj_AI_Hero Target)
        => 
            Target.HasBuffOfType(BuffType.Stun) ||
            Target.HasBuffOfType(BuffType.Snare) ||
            Target.HasBuffOfType(BuffType.Knockup) ||
            Target.HasBuffOfType(BuffType.Charm) ||
            Target.HasBuffOfType(BuffType.Flee) || 
            Target.HasBuffOfType(BuffType.Taunt) ||
            Target.HasBuffOfType(BuffType.Suppression);

        public static bool Has2WStacks(Obj_AI_Base target)
        =>
            target.HasBuff("vaynesilvereddebuff") &&
            target.GetBuffCount("vaynesilvereddebuff") == 2;
        
        public static bool ShouldStayFaded()
        =>
            Variables.Menu.Item($"{Variables.MainMenuName}.miscsettings.noaastealth").GetValue<bool>() &&
            ObjectManager.Player.HasBuff("vayneinquisition") &&
            ObjectManager.Player.HasBuff("vaynetumblefade") &&
            ObjectManager.Player.CountEnemiesInRange(1000) > 2;
    }

    /// <summary>
    ///    The killsteal class.
    /// </summary>
    public class KillSteal
    {
        public static int Damage()
        => 
            Bools.Has2WStacks(Targets.Target) ?
                (int)(ObjectManager.Player.GetSpellDamage(Targets.Target, SpellSlot.E) + ObjectManager.Player.GetSpellDamage(Targets.Target, SpellSlot.W))
                :
                (int)(ObjectManager.Player.GetSpellDamage(Targets.Target, SpellSlot.E));
    }

    /// <summary>
    ///    The drawings class.
    /// </summary>
    public class Drawings
    {
        public static void Load()
        {
            Drawing.OnDraw += delegate
            {
                if (Variables.Q.IsReady() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.q").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.Q.Range, System.Drawing.Color.Green);
                }

                if (Variables.E.IsReady() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.e").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.E.Range, System.Drawing.Color.Cyan);
                }
            };
        }
    }
}
