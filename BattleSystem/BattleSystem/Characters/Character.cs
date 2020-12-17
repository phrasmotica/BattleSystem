﻿using System;
using System.Collections.Generic;
using BattleSystem.Moves;
using BattleSystem.Stats;

namespace BattleSystem.Characters
{
    /// <summary>
    /// Abstract class representing a character.
    /// </summary>
    public abstract class Character
    {
        /// <summary>
        /// Gets or sets the character's ID.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Gets or sets the character's name.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets the character's max health.
        /// </summary>
        public int MaxHealth { get; protected set; }

        /// <summary>
        /// Gets or sets the character's current health.
        /// </summary>
        public int CurrentHealth { get; protected set; }

        /// <summary>
        /// Gets or sets the character's stats.
        /// </summary>
        public StatSet Stats { get; protected set; }

        /// <summary>
        /// Gets or sets the character's moves.
        /// </summary>
        public MoveSet Moves { get; protected set; }

        /// <summary>
        /// Gets the character's current speed.
        /// </summary>
        public int CurrentSpeed => Stats.Speed.CurrentValue;

        /// <summary>
        /// Gets whether the character is dead.
        /// </summary>
        public bool IsDead => CurrentHealth <= 0;

        /// <summary>
        /// Creates a new character with the given name, max health, stats and moves.
        /// </summary>
        public Character(string name, int maxHealth, StatSet stats, MoveSet moves)
        {
            Name = name;

            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;

            Stats = stats;
            Moves = moves;

            Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Returns a move to use in battle.
        /// </summary>
        /// <param name="otherCharacters">The other characters in the battle.</param>
        public abstract MoveUse ChooseMove(IEnumerable<Character> otherCharacters);

        /// <summary>
        /// Takes damage from the given attack.
        /// </summary>
        /// <param name="attack">The incoming attack.</param>
        /// <param name="userAttackStat">The attack stat of the user of the attack.</param>
        public virtual void ReceiveAttack(Attack attack, Stat userAttackStat)
        {
            var damage = ComputeDamage(attack, userAttackStat);
            CurrentHealth -= damage;
        }

        /// <summary>
        /// Computes the damage this character takes from the given attack.
        /// </summary>
        /// <param name="attack">The incoming attack.</param>
        /// <param name="userAttackStat">The attack stat of the user of the attack.</param>
        protected virtual int ComputeDamage(Attack attack, Stat userAttackStat)
        {
            // damage is offset by defence to a minimum of 1
            return Math.Max(1, attack.Power * (userAttackStat.CurrentValue - Stats.Defence.CurrentValue));
        }

        /// <summary>
        /// Receives effects from the given buff.
        /// </summary>
        /// <param name="multipliers">The effects of incoming buff.</param>
        public virtual void ReceiveBuff(IDictionary<StatCategory, double> multipliers)
        {
            foreach (var mult in multipliers)
            {
                var statCategory = mult.Key;

                switch (statCategory)
                {
                    case StatCategory.Attack:
                        Stats.Attack.Multiplier += mult.Value;
                        break;

                    case StatCategory.Defence:
                        Stats.Defence.Multiplier += mult.Value;
                        break;

                    case StatCategory.Speed:
                        Stats.Speed.Multiplier += mult.Value;
                        break;

                    default:
                        throw new ArgumentException($"Unrecognised stat category {statCategory}!");
                }
            }
        }

        /// <summary>
        /// Restores health from the given heal.
        /// </summary>
        /// <param name="heal">The incoming heal.</param>
        public virtual void ReceiveHeal(Heal heal)
        {
            var healAmount = ComputeHealAmount(heal);
            CurrentHealth += healAmount;
        }

        /// <summary>
        /// Computes the amount of health this character receives from the given heal.
        /// </summary>
        /// <param name="heal">The incoming heal.</param>
        protected virtual int ComputeHealAmount(Heal heal)
        {
            var rawAmount = heal.Amount;

            if (heal.HealingMode == HealingMode.Percentage)
            {
                rawAmount = (int) (MaxHealth * (heal.Amount / 100d));
            }

            // ensure character cannot over-heal
            return Math.Min(MaxHealth - CurrentHealth, rawAmount);
        }
    }
}
