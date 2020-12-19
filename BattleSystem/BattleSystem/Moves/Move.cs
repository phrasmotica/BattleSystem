﻿using System.Collections.Generic;
using BattleSystem.Characters;
using BattleSystem.Moves.Actions;

namespace BattleSystem.Moves
{
    /// <summary>
    /// Represents a move that may comprise multiple actions in order.
    /// </summary>
    public class Move
    {
        /// <summary>
        /// Gets or sets the name of the move.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the description of the move.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets or sets the max uses of the move.
        /// </summary>
        public int MaxUses { get; private set; }

        /// <summary>
        /// Gets or sets the remaining uses of the move.
        /// </summary>
        public int RemainingUses { get; private set; }

        /// <summary>
        /// Gets or sets a summary of the move.
        /// </summary>
        public string Summary => $"{Name} ({RemainingUses}/{MaxUses} uses) - {Description}";

        /// <summary>
        /// The actions this move will apply in order.
        /// </summary>
        private readonly IList<IMoveAction> _moveActions;

        /// <summary>
        /// Creates a new <see cref="Move"/> instance.
        /// </summary>
        public Move()
        {
            _moveActions = new List<IMoveAction>();
        }

        /// <summary>
        /// Sets the name for this move.
        /// </summary>
        /// <param name="name">The name.</param>
        public void SetName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Sets the description for this move.
        /// </summary>
        /// <param name="description">The description.</param>
        public void SetDescription(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Sets the max uses for this move. Optionally ignores the value of the remaining uses.
        /// </summary>
        /// <param name="maxUses"></param>
        /// <param name="ignoreRemainingUses"></param>
        public void SetMaxUses(int maxUses, bool ignoreRemainingUses = false)
        {
            MaxUses = maxUses;

            if (!ignoreRemainingUses || RemainingUses > maxUses)
            {
                // ensure remaining uses is capped if new max uses is lower
                RemainingUses = maxUses;
            }
        }

        /// <summary>
        /// Adds the given move action to the move.
        /// </summary>
        /// <param name="action">The move action to add.</param>
        public void AddAction(IMoveAction action)
        {
            _moveActions.Add(action);
        }

        /// <summary>
        /// Returns whether the move can be used.
        /// </summary>
        public bool CanUse()
        {
            return RemainingUses > 0;
        }

        /// <summary>
        /// Applies the effects of the move.
        /// </summary>
        /// <param name="user">The user of the move.</param>
        /// <param name="otherCharacters">The other characters.</param>
        public void Use(Character user, IEnumerable<Character> otherCharacters)
        {
            foreach (var action in _moveActions)
            {
                action.Use(user, otherCharacters);
            }

            RemainingUses--;
        }
    }
}