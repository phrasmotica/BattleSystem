﻿using System.Collections.Generic;
using System.Linq;
using BattleSystem.Core.Actions.Targets;
using BattleSystem.Core.Characters;

namespace BattleSystemExample.Actions.Targets
{
    /// <summary>
    /// Calculates the action target as the user of the move containing the
    /// action that last affected the user of this action.
    /// </summary>
    public class RetaliationActionTargetCalculator : IActionTargetCalculator
    {
        /// <summary>
        /// The action history.
        /// </summary>
        private readonly ActionHistory _actionHistory;

        /// <summary>
        /// Creates a new <see cref="RetaliationActionTargetCalculator"/> instance.
        /// </summary>
        /// <param name="actionHistory">The action history.</param>
        public RetaliationActionTargetCalculator(ActionHistory actionHistory)
        {
            _actionHistory = actionHistory;
        }

        /// <inheritdoc />
        public bool IsReactive => true;

        /// <inheritdoc />
        public (bool success, IEnumerable<Character> targets) Calculate(Character user, IEnumerable<Character> otherCharacters)
        {
            var result = _actionHistory.LastMoveDamageResultAgainst(user);

            if (result is null)
            {
                return (false, Enumerable.Empty<Character>());
            }

            if (result.User == user)
            {
                // cannot retaliate against oneself
                return (false, Enumerable.Empty<Character>());
            }

            return (true, new[] { result.User });
        }
    }
}
