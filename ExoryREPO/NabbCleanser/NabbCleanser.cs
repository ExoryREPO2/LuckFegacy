namespace NabbCleanser
{    
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.IO;

    using LeagueSharp;
    using LeagueSharp.Common;
    
    /// <summary>
    ///    The main class.
    /// </summary>
    internal class Cleanse
    {
        /// <summary>
        ///    The cleanser.
        /// </summary>
        public Cleanse()
        {
            (Variables.Menu = new Menu("NabbCleanser", "NabbCleanser", true)).AddToMainMenu();
            {
                Variables.Menu.AddItem(new MenuItem("use.cleanse", "Use Cleanse").SetValue(true));
                Variables.Menu.AddItem(new MenuItem("use.cleansers", "Use Cleansers").SetValue(true));
                Variables.Menu.AddItem(new MenuItem("use.cleansevsignite", "Cleanse Ignite?").SetValue(true));
                Variables.Menu.AddItem(new MenuItem("use.separator1", ""));
                Variables.Menu.AddItem(new MenuItem("panic_key", "Only Cleanse when pressed button enable").SetValue(true));
                Variables.Menu.AddItem(new MenuItem("use.panic_key", "Only Cleanse when pressed button").SetValue(new KeyBind(32, KeyBindType.Press)));
                Variables.Menu.AddItem(new MenuItem("use.delay", "Delay cleanse/cleansers usage by X ms").SetValue(new Slider(500, 0, 2000)));
                Variables.Menu.AddItem(new MenuItem("use.delay_rand", "Randomize cleanse delay").SetValue(true));
            }

            Others.BuildMikaelsMenu(Variables.Menu);
            Game.OnUpdate += Game_OnGameUpdate;
        }
        
        /// <summary>
        ///    Called when the game updates itself.
        /// </summary>
        /// <param name="args">
        ///    The <see cref="EventArgs" /> instance containing the event data.
        /// </param>
        private void Game_OnGameUpdate(EventArgs args)
        {
            if (ObjectManager.Player.IsDead || !Bools.HasNoProtection(ObjectManager.Player))
            {
                return;
            }

            if (Variables.Menu.Item("panic_key").GetValue<bool>())
            {
                if (!Variables.Menu.Item("use.panic_key").GetValue<KeyBind>().Active)
                {
                    return;
                }
            }
            
            // Cleanse Logic.
            if (Bools.ShouldUseCleanse(ObjectManager.Player))
            {
                Utility.DelayAction.Add(Variables.Menu.Item("use.delay").GetValue<Slider>().Value,
                    () => 
                    {
                        ObjectManager.Player.Spellbook.CastSpell(Variables.CleanseSpellSlot, ObjectManager.Player);
                    }
                );
            }
            
            // Cleansers Logic.
            if ((Bools.ShouldUseCleanser() ||
                (Bools.ShouldUseCleanse(ObjectManager.Player) && Bools.IsCleanseNotAvailable())) &&
                    Others.GetCleanserItem() != 0)
            {
                Utility.DelayAction.Add(
                    ObjectManager.Player.HasBuff("zedulttargetmark") ?
                        1500 :
                        Variables.Menu.Item("use.delay_rand").GetValue<bool>() ?
                            Variables.rand.Next(0, Variables.Menu.Item("use.delay").GetValue<Slider>().Value) :
                            Variables.Menu.Item("use.delay").GetValue<Slider>().Value,
                    () =>
                    {
                        Items.UseItem(Others.GetCleanserItem(), ObjectManager.Player);
                        return;
                    }
                );
            }

            if (!Items.HasItem(Variables.Mikaels_Crucible))
            {
                return;
            }
            
            // Mikaels Logic.
            foreach (var Ally in ObjectManager.Get<Obj_AI_Hero>()
                .Where(h => h.IsAlly
                    && Variables.Menu.Item("use.mikaels.{h.CharData.BaseSkinName.ToLowerInvariant()}").GetValue<bool>()
                    && Variables.Menu.Item("enable.mikaels").GetValue<bool>()))
            {
                if (Bools.ShouldUseCleanse(Ally))
                {
                    Utility.DelayAction.Add(Variables.Menu.Item("use.delay").GetValue<Slider>().Value,
                        () =>
                        {
                            Items.UseItem(Variables.Mikaels_Crucible, Ally);
                            return;
                        }
                    );
                }
            }
        }
    }
}
