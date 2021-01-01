﻿using BattleSystem.Actions;
using BattleSystem.Items;
using BattleSystemExample.Constants;

namespace BattleSystemExample.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="ItemBuilder"/>.
    /// </summary>
    public static class ItemBuilderExtensions
    {
        /// <summary>
        /// Adds an end-of-turn action to the built item.
        /// </summary>
        /// <param name="action">The action.</param>
        public static ItemBuilder WithEndTurnAction(this ItemBuilder builder, IAction action)
        {
            return builder.WithTaggedAction(action, EffectTags.EndTurn);
        }
    }
}