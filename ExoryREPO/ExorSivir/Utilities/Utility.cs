namespace ExorSivir
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
            Variables.Q = new Spell(SpellSlot.Q, 1250f);
            Variables.W = new Spell(SpellSlot.W);
            Variables.E = new Spell(SpellSlot.E);

            Variables.Q.SetSkillshot(0.25f, 90f, 1350f, false, SkillshotType.SkillshotLine);
        }

        /// <summary>
        /// The menu.
        /// </summary>
        public static void SetMenu()
        {
            /// <summary>
            /// The main menu.
            /// </summary>
            Variables.Menu = new Menu("ExorSivir", $"{Variables.MainMenuName}", true);
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
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqimmobile", "Use Q to Harass Immobile Champions")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqharassfarm", "Use Q in Harass/Farm")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.qmana", "Use Q in Harass/Farm only if Mana >= x%"))
                            .SetValue(new Slider(50, 0, 99));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.QMenu);

                    /// <summary>
                    /// The settings menu for the W spell.
                    /// </summary>
                    Variables.WMenu = new Menu("W Settings", $"{Variables.MainMenuName}.wsettingsmenu");
                    {
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewcombo", "Use W in Combo")).SetValue(true);
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewharassfarm", "Use W in Harass/Farm")).SetValue(true);
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.wmana", "Use E in Harass/Farm only if Mana >= x%"))
                            .SetValue(new Slider(50, 0, 99));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.WMenu);

                    /// <summary>
                    /// The settings menu for the E spell.
                    /// </summary>
                    Variables.EMenu = new Menu("E Settings", $"{Variables.MainMenuName}.esettingsmenu");
                    {
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useelogic", "Use Shield Logic")).SetValue(true);
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.EMenu);
                }
                Variables.Menu.AddSubMenu(Variables.SettingsMenu);

                /// <summary>
                /// The drawings menu.
                /// </summary>
                Variables.DrawingsMenu = new Menu("Drawings Menu", $"{Variables.MainMenuName}.drawingsmenu");
                {
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.q", "Show Q Range")).SetValue(true);
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
            Game.OnUpdate += Sivir.Game_OnGameUpdate;
            Obj_AI_Hero.OnProcessSpellCast += Sivir.Obj_AI_Hero_OnProcessSpellCast;
            Obj_AI_Base.OnDoCast += Sivir.Obj_AI_Base_OnDoCast;
        }
    }

    /// <summary>
    ///    The variables class.
    /// </summary>
    public class Variables
    {
        public static Menu Menu, OrbwalkerMenu, SettingsMenu, QMenu, WMenu, EMenu, DrawingsMenu;
        public static Spell Q, W, E;
        public static Orbwalking.Orbwalker Orbwalker { get; set; }
        public static string MainMenuName => $"exor.{ObjectManager.Player.ChampionName.ToLower()}";
    }

    /// <summary>
    ///    The Mana manager class.
    /// </summary>
    public class ManaManager
    {
        public static int NeededQMana => Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.qmana").GetValue<Slider>().Value;
        public static int NeededWMana => Variables.Menu.Item($"{Variables.MainMenuName}.wsettings.wmana").GetValue<Slider>().Value;
    }

    /// <summary>
    ///    The targets class.
    /// </summary>
    public class Targets
    {
        public static Obj_AI_Hero Target => TargetSelector.GetTarget(Variables.Q.Range, TargetSelector.DamageType.Physical);
        public static List<LeagueSharp.Obj_AI_Base> Minions => 
            MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition,
                Variables.Q.Range,
                MinionTypes.All,
                MinionTeam.Enemy,
                MinionOrderTypes.Health
            );
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
            };
        }
    }
}
