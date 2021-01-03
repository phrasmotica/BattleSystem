﻿using BattleSystem.Actions.Damage;
using BattleSystemExample.Actions;
using BattleSystemExample.Actions.Damage.Calculators;
using BattleSystemExample.Actions.Tags;
using BattleSystemExample.Actions.Targets;
using BattleSystemExample.Input;
using BattleSystemExample.Output;

namespace BattleSystemExample.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DamageActionBuilder"/>.
    /// </summary>
    public static class DamageActionBuilderExtensions
    {
        /// <summary>
        /// Sets the built damage action to use percentage-of-last-received-move damage.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="defaultAmount">The default amount of damage to deal.</param>
        /// <param name="actionHistory">The action history.</param>
        public static DamageActionBuilder PercentageOfLastReceivedMoveDamage(
            this DamageActionBuilder builder,
            int defaultAmount,
            ActionHistory actionHistory)
        {
            return builder.WithDamageCalculator(new PercentageOfLastReceivedMoveDamageCalculator(defaultAmount, actionHistory));
        }

        /// <summary>
        /// Sets the built damage action to target a single character chosen by the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public static DamageActionBuilder UserSelectsSingleTarget(this DamageActionBuilder builder, IUserInput userInput, IGameOutput gameOutput)
        {
            return builder.WithActionTargetCalculator(new SingleActionTargetCalculator(userInput, gameOutput));
        }

        /// <summary>
        /// Sets the built damage action to target a single other character chosen by the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public static DamageActionBuilder UserSelectsSingleOtherTarget(this DamageActionBuilder builder, IUserInput userInput, IGameOutput gameOutput)
        {
            return builder.WithActionTargetCalculator(new SingleOtherActionTargetCalculator(userInput, gameOutput));
        }

        /// <summary>
        /// Sets the built damage action to target a single ally chosen by the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public static DamageActionBuilder UserSelectsSingleAlly(this DamageActionBuilder builder, IUserInput userInput, IGameOutput gameOutput)
        {
            return builder.WithActionTargetCalculator(new SingleAllyActionTargetCalculator(userInput, gameOutput));
        }

        /// <summary>
        /// Sets the built damage action to target a single enemy chosen by the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public static DamageActionBuilder UserSelectsSingleEnemy(this DamageActionBuilder builder, IUserInput userInput, IGameOutput gameOutput)
        {
            return builder.WithActionTargetCalculator(new SingleEnemyActionTargetCalculator(userInput, gameOutput));
        }

        /// <summary>
        /// Sets the built damage action to target the user of the move
        /// containing the action that last affected the user.
        /// </summary>
        /// <param name="builder">The damage action builder.</param>
        /// <param name="actionHistory">The action history.</param>
        public static DamageActionBuilder Retaliates(
            this DamageActionBuilder builder,
            ActionHistory actionHistory)
        {
            return builder.WithActionTargetCalculator(new RetaliationActionTargetCalculator(actionHistory))
                          .WithTag(DamageTags.Retaliation);
        }
    }
}