namespace ExorTristana
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
            Variables.Q = new Spell(SpellSlot.Q, ObjectManager.Player.AttackRange);
            Variables.E = new Spell(SpellSlot.E, 550 + (7 * (ObjectManager.Player.Level - 1)));
            Variables.R = new Spell(SpellSlot.R, 550 + (7 * (ObjectManager.Player.Level - 1)));
        }

        /// <summary>
        /// The menu.
        /// </summary>
        public static void SetMenu()
        {
            /// <summary>
            /// The main menu.
            /// </summary>
            Variables.Menu = new Menu("ExorTristana", $"{Variables.MainMenuName}", true);
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
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.qsettings.useqauto", "Use Smart Q")).SetValue(true);
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.QMenu);

                    /// <summary>
                    /// The settings menu for the E spell.
                    /// </summary>
                    Variables.EMenu = new Menu("E Settings", $"{Variables.MainMenuName}.esettingsmenu");
                    {
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useeauto", "Use E Combo")).SetValue(true);
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useefarm", "Use E to Farm")).SetValue(true);
                        Variables.EMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.useebuildings", "Use E against Buildings")).SetValue(true);
                        Variables.QMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.emana", "Use E in Farm only if Mana >= x%"))
                            .SetValue(new Slider(50, 0, 99));
                        {
                            /// <summary>
                            /// The whitelist menu for the E spell.
                            /// </summary>
                            Variables.WhiteListMenu = new Menu("Condemn Whitelist Menu", $"{Variables.MainMenuName}.esettings.ewhitelist");
                            {
                                foreach (Obj_AI_Hero champ in HeroManager.Enemies)
                                {
                                    Variables.WhiteListMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.esettings.ewhitelist.{champ.ChampionName.ToLower()}", $"Condemn Only: {champ.ChampionName}").SetValue(true));
                                }
                            }
                            Variables.EMenu.AddSubMenu(Variables.WhiteListMenu);
                        }
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.EMenu);

                    /// <summary>
                    /// The settings menu for the R spell.
                    /// </summary>
                    Variables.RMenu = new Menu("R Settings", $"{Variables.MainMenuName}.rsettingsmenu");
                    {
                        Variables.RMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.rsettings.userks", "Use Smart E & R KillSteal")).SetValue(true);
                    }
                    Variables.SettingsMenu.AddSubMenu(Variables.RMenu);
                }
                Variables.Menu.AddSubMenu(Variables.SettingsMenu);

                /// <summary>
                /// The drawings menu.
                /// </summary>
                Variables.DrawingsMenu = new Menu("Drawings Menu", $"{Variables.MainMenuName}.drawingsmenu");
                {
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.e", "Show E Range")).SetValue(true);
                    Variables.DrawingsMenu.AddItem(new MenuItem($"{Variables.MainMenuName}.drawings.r", "Show R Range")).SetValue(true);
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
            Orbwalking.BeforeAttack += Tristana.Orbwalking_BeforeAttack;
            Game.OnUpdate += Tristana.Game_OnGameUpdate;
            Obj_AI_Base.OnDoCast += Tristana.Obj_AI_Base_OnDoCast;
        }
    }

    /// <summary>
    ///    The variables class.
    /// </summary>
    public class Variables
    {
        public static Menu Menu, OrbwalkerMenu, SettingsMenu, QMenu, EMenu, RMenu, WhiteListMenu, DrawingsMenu;
        public static Spell Q, E, R;
        public static Orbwalking.Orbwalker Orbwalker { get; set; }
        public static string MainMenuName => $"exor.{ObjectManager.Player.ChampionName.ToLower()}";
    }

    /// <summary>
    ///    The Mana manager class.
    /// </summary>
    public class ManaManager
    {
        public static int NeededQMana => Variables.Menu.Item($"{Variables.MainMenuName}.qsettings.qmana").GetValue<Slider>().Value;
        public static int NeededRMana => Variables.Menu.Item($"{Variables.MainMenuName}.esettings.emana").GetValue<Slider>().Value;
    }

    /// <summary>
    ///    The targets class.
    /// </summary>
    public class Targets
    {
        public static Obj_AI_Hero Target => TargetSelector.GetTarget(Variables.E.Range, TargetSelector.DamageType.Physical);
        public static IOrderedEnumerable<Obj_AI_Minion> EMinions => GameObjects.EnemyMinions
            .Where(
                eminion
                =>
                    eminion.IsValidTarget(Variables.E.Range) &&
                    GameObjects.EnemyMinions.Count(minions => minions.Distance(eminion) < 150f) > 2)
            .OrderBy(
                minion 
                =>
                    minion.Health);

        public static Obj_AI_Minion EMinion => Targets.EMinions.FirstOrDefault();
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

        public static bool IsCharged(Obj_AI_Base obj)
        =>
            obj.HasBuff("TristanaECharge");
    }

    /// <summary>
    ///    The killsteal class.
    /// </summary>
    public class KillSteal
    {
        public static int Damage(Obj_AI_Hero target)
        =>
            Bools.IsCharged(target) ?
                (int)(Variables.E.GetDamage(target) *
                    ((0.30f * target.GetBuffCount("TristanaECharge")) + 1f) + 
                    Variables.R.GetDamage(target))
            :
                (int)Variables.R.GetDamage(target);
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