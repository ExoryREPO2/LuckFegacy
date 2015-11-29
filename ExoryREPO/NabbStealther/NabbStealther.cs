// Credits for ~70% of the code, which is:
// OnIntegerPropertyChange Event;
// OnStealth Event;
// And relative arguments, go to [member=Asuna], from LeagueSharp forums.

namespace NabbStealther
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LeagueSharp;
    using LeagueSharp.Common;
    using SharpDX;
    using System.Reflection;
    
    /// <summary>
    ///     The main class.
    /// </summary>
    class Stealther
    {
        public static Menu Menu, StealthMenu;
        public static string[] StealthChampions;
        
        private static readonly Items.Item Trinket = new Items.Item(3364, 600f);
        private static readonly Items.Item Ward = new Items.Item(2043, 600f);
  
        /// <summary>
        ///    Called when the Assembly is loaded.
        /// </summary>
        public static void OnLoad()
        {
            // Loads the main menu.
            LoadMenu();
            
            // Triggers on property change.
            GameObject.OnIntegerPropertyChange += GameObject_OnIntegerPropertyChange;
            
            // Triggers on stealth events.
            OnStealth += On_Stealth;
        }
        
        /// <summary>
        ///    The menu.
        /// </summary>
        private static void LoadMenu()        
        {
            (Menu = new Menu("NabbStealther", "nabbstealther.menu", true)).AddToMainMenu();
            Menu.AddItem(new MenuItem("enable", "Enable").SetValue(true));
            
            // Loads the stealth menu.
            //BuildStealthMenu();
        }
        
        /*
        private static void BuildStealthMenu()
        {
            StealthChampions = new[]
            {
                "Akali", "Evelynn", "KhaZix",
                "LeBlanc", "Rengar", "Shaco",
                "Talon", "Teemo", "Twitch",
                "Vayne", "Wukong"
            };
            
            StealthMenu = new Menu("Stealth Menu", "nabbstealther.menu.stealthmenu");
            foreach (var i in StealthChampions)
            {
                StealthMenu.AddItem(
                    new MenuItem("nabbstealther.menu.stealthmenu.{StealthChampions[i].ToLowerInvariant()}", i)).SetValue(true);
            }
            
            Menu.AddSubMenu(StealthMenu);
        }
        */
        
        /// <summary>
        ///     Function is called when a <see cref="GameObject" /> gets an integer property change and is called by an event.
        /// </summary>
        /// <param name="sender">GameObject</param>
        /// <param name="args">Integer Property Change Data</param>
        private static void GameObject_OnIntegerPropertyChange(GameObject sender, GameObjectIntegerPropertyChangeEventArgs args)
        {
            if (!args.Property.Equals("ActionState") || !(sender is Obj_AI_Hero))
            {
                return;
            }

            var oldState = (GameObjectCharacterState)args.OldValue;
            var newState = (GameObjectCharacterState)args.NewValue;

            if (!oldState.HasFlag(GameObjectCharacterState.IsStealth) &&
                newState.HasFlag(GameObjectCharacterState.IsStealth))
            {
                FireOnStealth
                (
                    new OnStealthEventArgs
                    {
                        Sender = (Obj_AI_Hero)sender,
                        Time = Game.Time,
                        IsStealthed = true
                    }
                );
            }
            
            else if (oldState.HasFlag(GameObjectCharacterState.IsStealth) &&
                !newState.HasFlag(GameObjectCharacterState.IsStealth))
            {
                FireOnStealth
                (
                    new OnStealthEventArgs
                    {
                        Sender = (Obj_AI_Hero)sender,
                        IsStealthed = false
                    }
                );
            }
        }
        
        /// <summary>
        ///     Gets fired when any hero is invisible.
        /// </summary>
        public static event Action<OnStealthEventArgs> OnStealth;

        /// <summary>
        /// </summary>
        /// <param name="args">OnStealthEventArgs <see cref="OnStealthEventArgs" /></param>
        private static void FireOnStealth(OnStealthEventArgs args)
        {
            if (OnStealth != null)
            {
                OnStealth(args);
            }
        }

        /// <summary>
        ///     On Stealth Event Data, contains useful information that is passed with OnStealth
        ///     <see cref="OnStealth" />
        /// </summary>
        public struct OnStealthEventArgs
        {
            /// <summary>
            ///     Returns if the unit is stealthed or not.
            /// </summary>
            public bool IsStealthed;

            /// <summary>
            ///     The stealth sender.
            /// </summary>
            public Obj_AI_Hero Sender;

            /// <summary>
            ///     The spell start time.
            /// </summary>
            public float Time;
        }
        
        /// <summary>
        /// Called when an unit goes on stealth.
        /// </summary>
        /// <param name="obj">The Event's arguments</param>
        private static void On_Stealth(OnStealthEventArgs obj)
        {
            if (!Menu.Item("enable").GetValue<bool>())
            {
                return;
            }

            if (/* obj.IsStealthed 
                && */ obj.Sender.IsEnemy 
                && obj.Sender.ServerPosition.Distance(ObjectManager.Player.ServerPosition) <= 600f)
            {
                var objectPosition = obj.Sender.ServerPosition;
                if (Trinket.IsOwned() && Trinket.IsReady())
                {
                    var extend = ObjectManager.Player.ServerPosition.Extend(objectPosition, 400f);
                    Trinket.Cast(extend);
                    return;
                }

                if (Ward.IsOwned() && Ward.IsReady())
                {
                    var extend = ObjectManager.Player.ServerPosition.Extend(objectPosition, 400f);
                    Ward.Cast(extend);
                    return;
                }
            }
        }  
    }
}
