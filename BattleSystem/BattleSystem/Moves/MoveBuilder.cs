﻿using BattleSystem.Moves.Actions;

namespace BattleSystem.Moves
{
    /// <summary>
    /// Builder class for moves.
    /// </summary>
    public class MoveBuilder
    {
        /// <summary>
        /// The move to build.
        /// </summary>
        private readonly Move _move;

        /// <summary>
        /// Creates a new <see cref="MoveBuilder"/> instance.
        /// </summary>
        public MoveBuilder()
        {
            _move = new Move();
        }

        /// <summary>
        /// Names the built move.
        /// </summary>
        /// <param name="name">The built move's name.</param>
        public MoveBuilder Name(string name)
        {
            _move.SetName(name);
            return this;
        }

        /// <summary>
        /// Describes the built move.
        /// </summary>
        /// <param name="description">The built move's description.</param>
        public MoveBuilder Describe(string description)
        {
            _move.SetDescription(description);
            return this;
        }

        /// <summary>
        /// Sets the built move's max uses.
        /// </summary>
        /// <param name="maxUses">The built move's max uses.</param>
        public MoveBuilder WithMaxUses(int maxUses)
        {
            _move.SetMaxUses(maxUses);
            return this;
        }

        /// <summary>
        /// Adds the given action to the built move.
        /// </summary>
        /// <param name="action">The action to add to the built move.</param>
        public MoveBuilder WithAction(IMoveAction action)
        {
            _move.AddAction(action);
            return this;
        }

        /// <summary>
        /// Returns the built move.
        /// </summary>
        public Move Build()
        {
            return _move;
        }
    }
}