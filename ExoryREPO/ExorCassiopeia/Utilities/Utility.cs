namespace ExorCassiopeia
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
            Variables.Q = new Spell(SpellSlot.Q, 850f);
            Variables.W = new Spell(SpellSlot.W, 850f);
            Variables.E = new Spell(SpellSlot.E, 750f);
            Variables.R = new Spell(SpellSlot.R, 800f);

            Variables.Q.SetSkillshot(0.75f, Variables.Q.Instance.SData.CastRadius, float.MaxValue, false, SkillshotType.SkillshotCircle);
            Variables.W.SetSkillshot(0.5f, Variables.W.Instance.SData.CastRadius, Variables.W.Instance.SData.MissileSpeed, false, SkillshotType.SkillshotCircle);
            Variables.R.SetSkillshot(0.3f, (float)(80 * Math.PI / 180), float.MaxValue, false, SkillshotType.SkillshotCone);
        }

        public static void SetMenu()
        {
            // Main Menu
            Variables.Menu = new Menu("ExorCassiopeia", $"{Variables.MainMenuName}", true);
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
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqharassfarm", "Use Q in Harass/Farm")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.qmana", "Use Q in Harass/Farm only if Mana >= x%"))
                            .SetValue(new Slider(50, 0, 99));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.QMenu);

                    // W Options
                    Variables.WMenu = new Menu("W Settings", $"{Variables.MainMenuName}.wsettingsmenu");
                    {
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewcombo", "Use W in Combo")).SetValue(true);
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewharassfarm", "Use W in Harass/Farm")).SetValue(true);
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.wmana", "Use W in Harass/Farm only if Mana >= x%"))
                            .SetValue(new Slider(50, 0, 99));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.WMenu);

                    // E Options
                    Variables.EMenu = new Menu("E Settings", $"{Variables.MainMenuName}.esettingsmenu");
                    {
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useecombo", "Use E in Combo")).SetValue(true);
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useefarm", "Use E to LastHit")).SetValue(true);
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.edelay", "E Delay (ms)"))
                            .SetValue(new Slider(0, 0, 250));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.EMenu);

                    // R Options
                    Variables.RMenu = new Menu("R Settings", $"{Variables.MainMenuName}.rsettingsmenu");
                    {
                        Variables.RMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.rsettings.usercombo", "Use R in Combo")).SetValue(true);
                        Variables.RMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.rsettings.userminenemies", "Use R if Enemies facing >=")).SetValue(new Slider(1, 1, 5));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.RMenu);
                }
                Variables.Menu.AddSubMenu(Variables.SettingsMenu);

                //Miscellaneous Menu
                Variables.MiscMenu = new Menu("Miscellaneous Menu", $"{Variables.MainMenuName}.miscmenu");
                {
                    Variables.MiscMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.misc.lasthitnopoison", "LastHit with E Non-Poisoned Minions too")).SetValue(true);
                    Variables.MiscMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.misc.aa", "Use AA in Combo")).SetValue(true);
                    Variables.MiscMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.misc.stacktear", "Stack Tear of the Goddess")).SetValue(true);
                    Variables.MiscMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.misc.stacktearmana", "Stack Tear only if Mana > x%"))
                        .SetValue(new Slider(80, 1, 95));
                }
                Variables.Menu.AddSubMenu(Variables.MiscMenu);

                //Drawings Menu
                Variables.DrawingsMenu = new Menu("Drawings Menu", $"{Variables.MainMenuName}.drawingsmenu");
                {
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.q", "Show Q Range")).SetValue(true);
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.w", "Show W Range")).SetValue(true);
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.e", "Show E Range")).SetValue(true);
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.r", "Show R Range")).SetValue(true);
                }
                Variables.Menu.AddSubMenu(Variables.DrawingsMenu);
            }
            Variables.Menu.AddToMainMenu();
        }

        public static void SetMethods()
        {
            Game.OnUpdate += Cassiopeia.Game_OnGameUpdate;
        }
    }

    /// <summary>
    ///    The variables class.
    /// </summary>
    public class Variables
    {
        public static Menu Menu, OrbwalkerMenu, SettingsMenu, QMenu, WMenu, EMenu, RMenu, MiscMenu, DrawingsMenu;
        public static Spell Q, W, E, R;
        public static Orbwalking.Orbwalker Orbwalker;
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
        public static Obj_AI_Hero Target => TargetSelector.GetTarget(Variables.R.Range, TargetSelector.DamageType.Magical);
        public static List<LeagueSharp.Obj_AI_Base> Minions => 
            MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition,
                Variables.E.Range,
                MinionTypes.All,
                MinionTeam.Enemy,
                MinionOrderTypes.Health
            );
        public static Obj_AI_Base Minion => Targets.Minions.First();
        public static IEnumerable<LeagueSharp.Obj_AI_Hero> RTargets = HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(Variables.R.Range) && enemy.IsFacing(ObjectManager.Player));
    }

    /// <summary>
    ///    The bools class.
    /// </summary>
    public class Bools
    {
        public static bool HasTear() 
        =>
            ObjectManager.Player.InventoryItems.Any(
                item =>
                    item.Id.Equals(ItemId.Tear_of_the_Goddess) ||
                    item.Id.Equals(ItemId.Archangels_Staff) ||
                    item.Id.Equals(ItemId.Manamune) ||
                    item.Id.Equals(ItemId.Tear_of_the_Goddess_Crystal_Scar) ||
                    item.Id.Equals(ItemId.Archangels_Staff_Crystal_Scar) ||
                    item.Id.Equals(ItemId.Manamune_Crystal_Scar));
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

                if (Variables.R.IsReady() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.r").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.R.Range, System.Drawing.Color.Red);
                }
            };
        }
    }
}
