﻿using System.Collections.Generic;
using System.Linq;
using BattleSystem.Core.Characters;

namespace BattleSystem.Core.Actions.Damage.Calculators
{
    /// <summary>
    /// Calculates damage equal to the damage action's power.
    /// </summary>
    public class AbsoluteDamageCalculator : IDamageCalculator
    {
        /// <summary>
        /// The amount of damage to deal.
        /// </summary>
        private readonly int _amount;

        /// <summary>
        /// Creates a new <see cref="AbsoluteDamageCalculator"/> instance.
        /// </summary>
        /// <param name="amount">The amount of damage to deal.</param>
        public AbsoluteDamageCalculator(int amount)
        {
            _amount = amount;
        }

        /// <inheritdoc/>
        public IEnumerable<DamageCalculation> Calculate(Character user, DamageAction damage, IEnumerable<Character> targets)
        {
            return targets.Select(target => new DamageCalculation
            {
                Target = target,
                Success = true,
                Amount = _amount,
            });
        }
    }
}
