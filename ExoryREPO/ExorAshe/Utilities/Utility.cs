namespace ExorAshe
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
            Variables.Q = new Spell(SpellSlot.Q);
            Variables.W = new Spell(SpellSlot.W, 1200f);
            Variables.E = new Spell(SpellSlot.E, 20000f);
            Variables.R = new Spell(SpellSlot.R, 4000f);
            
            Variables.W.SetSkillshot(250f, (float)(45f * Math.PI / 180), 900f, true, SkillshotType.SkillshotCone);
            Variables.R.SetSkillshot(250f, 130f, 1600f, false, SkillshotType.SkillshotLine);
        }

        public static void SetMenu()
        {
            // Main Menu
            Variables.Menu = new Menu("ExorAshe", $"{Variables.MainMenuName}", true);
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
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqharassfarm", "Use Q in Harass/Farm")).SetValue(false);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.qmana", "Use Q in Harass/Farm only if Mana >= x%"))
                            .SetValue(new Slider(50, 0, 99));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.QMenu);

                    // W Options
                    Variables.WMenu = new Menu("W Settings", $"{Variables.MainMenuName}.wsettingsmenu");
                    {
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewcombo", "Use W in Combo")).SetValue(true);
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewks", "Use W to Automatically KillSteal")).SetValue(true);
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewimmobile", "Use W to Harass Immobile Champions")).SetValue(true);
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.usewharassfarm", "Use W in Harass/Farm")).SetValue(true);
                        Variables.WMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.wsettings.wmana", "Use W in Harass/Farm only if Mana >= x%"))
                            .SetValue(new Slider(50, 0, 99));
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.WMenu);

                    // R Options
                    Variables.RMenu = new Menu("R Settings", $"{Variables.MainMenuName}.rsettingsmenu");
                    {
                        Variables.RMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.rsettings.usercombo", "Use R in Combo")).SetValue(true);
                        Variables.RMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.rsettings.userks", "Use R to KillSteal")).SetValue(true);
                        Variables.RMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.rsettings.usermechanic", "Use Doublelift's E->R Mechanic")).SetValue(true);
                        {
                            //Ult Whitelist Menu
                            Variables.WhiteListMenu = new Menu("Ultimate Whitelist Menu", $"{Variables.MainMenuName}.rsettings.rwhitelist");
                            {
                                foreach (Obj_AI_Hero champ in HeroManager.Enemies)
                                {
                                    Variables.WhiteListMenu.AddItem(
                                        new MenuItem($"{Variables.MainMenuName}.rsettings.rwhitelist.{champ.ChampionName.ToLower()}",
                                            $"Ult Only: {champ.ChampionName}").SetValue(true));
                                }
                            }
                            Variables.RMenu.AddSubMenu(Variables.WhiteListMenu);
                        }
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.RMenu);
                }
                Variables.Menu.AddSubMenu(Variables.SettingsMenu);

                //Drawings Menu
                Variables.DrawingsMenu = new Menu("Drawings Menu", $"{Variables.MainMenuName}.drawingsmenu");
                {
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.w", "Show W Range")).SetValue(true);
                }
                Variables.Menu.AddSubMenu(Variables.DrawingsMenu);
            }
            Variables.Menu.AddToMainMenu();
        }

        public static void SetMethods()
        {
            Game.OnUpdate += Ashe.Game_OnGameUpdate;
            Obj_AI_Base.OnDoCast += Ashe.Obj_AI_Base_OnDoCast;
        }
    }

    /// <summary>
    ///    The variables class.
    /// </summary>
    public class Variables
    {
        public static Menu Menu, OrbwalkerMenu, SettingsMenu, QMenu, WMenu, EMenu, RMenu, WhiteListMenu, MiscMenu, DrawingsMenu;
        public static Spell Q, W, E, R;
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
        public static Obj_AI_Hero Target => TargetSelector.GetTarget(Orbwalking.GetRealAutoAttackRange(ObjectManager.Player), TargetSelector.DamageType.Physical);
        public static List<LeagueSharp.Obj_AI_Base> Minions => 
            MinionManager.GetMinions(
                ObjectManager.Player.ServerPosition,
                Variables.W.Range,
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
                if (Variables.W.IsReady() &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.drawings.w").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, Variables.W.Range, System.Drawing.Color.Purple);
                }
            };
        }
    }
}
