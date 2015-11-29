namespace NabbActivator
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using LeagueSharp;
    using LeagueSharp.Common;
    
    using ItemData = LeagueSharp.Common.Data.ItemData;
 
    /// <summary>
    /// The cleansers class.
    /// </summary>
    public class Cleansers
    {        
        public static void Execute(EventArgs args)
        {
            if (Bools.ShouldUseCleanser() ||
                (Bools.ShouldUseCleanse(ObjectManager.Player) && !Bools.IsSpellAvailable(SpellSlots.Cleanse)))
            {
                /// <summary>
                /// The Dervish Blade.
                /// </summary>
                if (ItemData.Dervish_Blade.GetItem().IsReady())
                {
                    Utility.DelayAction.Add(
                        Bools.HasZedMark(ObjectManager.Player) ?
                            1500 : Bools.MustRandomize() ?
                                WeightedRandom.Next(100, 200) : 0,
                        () =>
                        {
                            ItemData.Dervish_Blade.GetItem().Cast();
                            return;
                        }
                    );
                }

                /// <summary>
                /// The Mercurial Scimitar.
                /// </summary>
                if (ItemData.Mercurial_Scimitar.GetItem().IsReady())
                {
                    Utility.DelayAction.Add(
                        Bools.HasZedMark(ObjectManager.Player) ?
                            1500 : Bools.MustRandomize() ?
                                WeightedRandom.Next(100, 200) : 0,
                        () =>
                        {
                            ItemData.Mercurial_Scimitar.GetItem().Cast();
                            return;
                        }
                    );
                }

                /// <summary>
                /// The Quicksilver Sash.
                /// </summary>
                if (ItemData.Quicksilver_Sash.GetItem().IsReady())
                {
                    Utility.DelayAction.Add(
                        Bools.HasZedMark(ObjectManager.Player) ?
                            1500 : Bools.MustRandomize() ?
                                WeightedRandom.Next(100, 200) : 0,
                        () =>
                        {
                            ItemData.Quicksilver_Sash.GetItem().Cast();
                            return;
                        }
                    );
                }

                /// <summary>
                /// The Mikaels Crucible.
                /// </summary>
                if (ItemData.Mikaels_Crucible.GetItem().IsReady())
                {
                    foreach (var Ally in HeroManager.Allies
                        .Where(
                            h =>
                                h.IsValidTarget(750f, false) &&
                                Bools.ShouldUseCleanse(h) &&
                                Bools.HasNoProtection(h)))
                    {
                        Utility.DelayAction.Add(
                            Bools.MustRandomize() ?
                                WeightedRandom.Next(100, 200) : 0,
                            () =>
                            {
                                ItemData.Mikaels_Crucible.GetItem().Cast(Ally);
                                return;
                            }
                        );
                    }
                }
            }
        }
    }
}
