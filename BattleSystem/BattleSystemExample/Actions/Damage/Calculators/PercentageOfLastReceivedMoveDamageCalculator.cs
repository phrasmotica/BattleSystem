﻿using System;
using System.Collections.Generic;
using BattleSystem.Actions.Damage;
using BattleSystem.Actions.Damage.Calculators;
using BattleSystem.Characters;

namespace BattleSystemExample.Actions.Damage.Calculators
{
    /// <summary>
    /// Calculates damage equal to a percentage of the amount of the last damage
    /// action from a move that affected the user.
    /// </summary>
    public class PercentageOfLastReceivedMoveDamageCalculator : IDamageCalculator
    {
        /// <summary>
        /// The percentage of the damage to deal.
        /// </summary>
        private readonly int _percentage;

        /// <summary>
        /// The action history.
        /// </summary>
        private readonly ActionHistory _actionHistory;

        /// <summary>
        /// Creates a new <see cref="PercentageOfLastReceivedMoveDamageCalculator"/> instance.
        /// </summary>
        /// <param name="percentage">The percentage of damage to deal.</param>
        /// <param name="actionHistory">The action history.</param>
        public PercentageOfLastReceivedMoveDamageCalculator(
            int percentage,
            ActionHistory actionHistory)
        {
            _percentage = percentage;
            _actionHistory = actionHistory;
        }

        /// <inheritdoc/>
        public IEnumerable<DamageCalculation> Calculate(Character user, DamageAction damage, IEnumerable<Character> targets)
        {
            var result = _actionHistory.LastMoveDamageResultAgainst(user);

            var calculations = new List<DamageCalculation>();

            foreach (var target in targets)
            {
                if (result is null)
                {
                    calculations.Add(new DamageCalculation
                    {
                        Target = target,
                        Success = false,
                        Amount = 0,
                    });
                }
                else
                {
                    calculations.Add(new DamageCalculation
                    {
                        Target = target,
                        Success = true,
                        Amount = Math.Max(1, result.Amount * _percentage / 100),
                    });
                }
            }

            return calculations;
        }
    }
}
