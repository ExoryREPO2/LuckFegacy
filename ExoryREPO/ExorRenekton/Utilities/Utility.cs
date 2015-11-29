namespace ExorRenekton
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;

    /// <summary>
    ///    The settings class.
    /// </summary>
    public class Settings
    {
        public static void SetSpells()
        {
            Variables.Q = new Spell(SpellSlot.Q, 225f);
            Variables.W = new Spell(SpellSlot.W, 176f);
            Variables.E = new Spell(SpellSlot.E, 450f);
            Variables.R = new Spell(SpellSlot.R, 175f);
        }

        public static void SetMenu()
        {
            // Main Menu
            Variables.Menu = new Menu("ExorRenekton", $"{Variables.MainMenuName}", true);
            {
                //Orbwalker
                Variables.OrbwalkerMenu = new Menu("Orbwalker", $"{Variables.MainMenuName}.orbwalkermenu");
                {
                    Variables.Orbwalker = new Orbwalking.Orbwalker(Variables.OrbwalkerMenu);
                }
                Variables.Menu.AddSubMenu(Variables.OrbwalkerMenu);

                //Settings Menu
                Variables.SettingsMenu = new Menu("Spell Menu", $"{Variables.MainMenuName}.settingsmenu");
                {
                    // Q Options
                    Variables.QMenu = new Menu("Q Settings", $"{Variables.MainMenuName}.qsettingsmenu");
                    {
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqcombo", "Use Q in Combo")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqks", "Use Q to Automatically KillSteal")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqautoharass", "Use Q AutoHarass")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqfarm", "Use Q to Farm"))
                            .SetValue(new Slider(50, 0, 99));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.QMenu);

                    // W Options
                    Variables.WMenu = new Menu("W Settings", $"{Variables.MainMenuName}.wsettingsmenu");
                    {
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewcombo", "Use W in Combo")).SetValue(true);
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.WMenu);

                    // E Options
                    Variables.EMenu = new Menu("E Settings", $"{Variables.MainMenuName}.esettingsmenu");
                    {
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useecombo", "Use E in Combo (Only the 1st stage)")).SetValue(true);
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.EMenu);

                    // R Options
                    Variables.RMenu = new Menu("R Settings", $"{Variables.MainMenuName}.rsettingsmenu");
                    {
                        Variables.RMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.rsettings.userlifesaver", "Use R LifeSaver")).SetValue(true);
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.RMenu);

                    // Miscellaneous Options
                    Variables.MiscMenu = new Menu("Misc. Settings", $"{Variables.MainMenuName}.miscsettingsmenu");
                    {
                        Variables.MiscMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.miscsettings.useresetters", "Use Smart Tiamat/Ravenous/Titanic")).SetValue(true);
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.MiscMenu);
                }
                Variables.Menu.AddSubMenu(Variables.SettingsMenu);

                //Drawings Menu
                Variables.DrawingsMenu = new Menu("Drawings Menu", $"{Variables.MainMenuName}.drawingsmenu");
                {
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.q", "Show Q Range")).SetValue(true);
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.w", "Show W Range")).SetValue(true);
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.e", "Show E Range")).SetValue(true);
                }
                Variables.Menu.AddSubMenu(Variables.DrawingsMenu);
            }
            Variables.Menu.AddToMainMenu();
        }

        public static void SetMethods()
        {
            Game.OnUpdate += Renekton.Game_OnGameUpdate;
            Obj_AI_Base.OnDoCast += Renekton.Obj_AI_Base_OnDoCast;
        }
    }

    /// <summary>
    ///    The variables class.
    /// </summary>
    public class Variables
    {
        public static Menu Menu, OrbwalkerMenu, SettingsMenu, QMenu, WMenu, EMenu, RMenu, MiscMenu, DrawingsMenu;
        public static Spell Q, W, E, R;
        public static Orbwalking.Orbwalker Orbwalker { get; set; }
        public static string MainMenuName => $"exor.{ObjectManager.Player.ChampionName.ToLower()}";
    }

    /// <summary>
    ///    The Mana manager class.
    /// </summary>
    public class ManaManager
    {
    }

    /// <summary>
    ///    The targets class.
    /// </summary>
    public class Targets
    {
        public static Obj_AI_Hero Target => TargetSelector.GetTarget(Variables.W.Range, TargetSelector.DamageType.Physical);
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

                if (Variables.W.IsReady() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.w").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.W.Range, System.Drawing.Color.Purple);
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
