namespace NabbActivator
{
    using System;
    using System.Linq;
    
    using LeagueSharp;
    using LeagueSharp.Common;

    using ItemData = LeagueSharp.Common.Data.ItemData;

    /// <summary>
    ///     The main class.
    /// </summary>
    public class Activator
    {
        /// <summary>
        /// Called when the game loads itself.
        /// </summary>
        public static void OnLoad()
        {
            Settings.SetSpells();
            Settings.SetMenu();
            Settings.SetMethods();
        }

        /// <summary>
        /// Called when the game updates itself.
        /// </summary>
        /// <param name="args">The <see cref="EventArgs"/> instance containing the event data.</param>
        public static void Game_OnGameUpdate(EventArgs args)
        {
            if (!ObjectManager.Player.IsDead)
            {
                if (Variables.Menu.Item($"{Variables.MainMenuName}.defensives").GetValue<bool>()) 
                {
                    Defensives.Execute(args);
                }

                if (Variables.Menu.Item($"{Variables.MainMenuName}.spells").GetValue<bool>())
                {
                    Spells.Execute(args);
                }

                if (Variables.Menu.Item($"{Variables.MainMenuName}.cleansers").GetValue<bool>())
                {
                    Cleansers.Execute(args);
                }

                if (Variables.Menu.Item($"{Variables.MainMenuName}.consumables").GetValue<bool>())
                {
                    Consumables.Execute(args);
                }

                if (Variables.Menu.Item($"{Variables.MainMenuName}.combo_button").GetValue<KeyBind>().Active &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.offensives").GetValue<bool>())
                {
                    Offensives.Execute(args);
                }
            }
        }
        
        /// <summary>
        /// Called when a cast has been executed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The args.</param>
        public static void Obj_AI_Base_OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (ItemData.Ohmwrecker.GetItem().IsReady())
            {
                if (sender.IsValid<Obj_AI_Turret>() &&
                    args.Target.IsValid<Obj_AI_Hero>() &&
                    args.Target.IsAlly &&
                    Variables.Menu.Item($"{Variables.MainMenuName}.defensives").GetValue<bool>())
                {
                    ItemData.Ohmwrecker.GetItem().Cast((Obj_AI_Turret)args.Target);
                }
            }
        }
    }
}
