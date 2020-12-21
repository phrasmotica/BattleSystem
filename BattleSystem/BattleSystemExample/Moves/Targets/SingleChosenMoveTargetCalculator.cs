﻿using System;
using System.Collections.Generic;
using System.Linq;
using BattleSystem.Characters;
using BattleSystem.Moves.Targets;
using BattleSystemExample.Input;
using BattleSystemExample.Output;

namespace BattleSystemExample.Moves.Targets
{
    /// <summary>
    /// Lets the user choose a single move target.
    /// </summary>
    public class SingleChosenMoveTargetCalculator : IMoveTargetCalculator
    {
        /// <summary>
        /// The user input.
        /// </summary>
        private readonly IUserInput _userInput;

        /// <summary>
        /// The game output.
        /// </summary>
        private readonly IGameOutput _gameOutput;

        /// <summary>
        /// Creates a new <see cref="SingleChosenMoveTargetCalculator"/> instance.
        /// </summary>
        /// <param name="userInput">The user input.</param>
        /// <param name="gameOutput">The game output.</param>
        public SingleChosenMoveTargetCalculator(IUserInput userInput, IGameOutput gameOutput)
        {
            _userInput = userInput;
            _gameOutput = gameOutput;
        }

        /// <inheritdoc />
        public IEnumerable<Character> Calculate(Character user, IEnumerable<Character> otherCharacters)
        {
            var targets = otherCharacters.Prepend(user);

            _gameOutput.WriteLine($"Select a target for the move:");
            _gameOutput.WriteLine(GetChoices(targets));

            return new[] { SelectTarget(targets) };
        }

        /// <summary>
        /// Returns a string describing the given characters as potential move targets.
        /// </summary>
        /// <param name="targets">The move targets.</param>
        private static string GetChoices(IEnumerable<Character> targets)
        {
            var choices = targets.Select((c, i) => $"{i + 1}: {c.Name}");
            return string.Join("\n", choices);
        }

        /// <summary>
        /// Lets the player select a move target and returns it.
        /// </summary>
        /// <param name="targets">The move targets.</param>
        private Character SelectTarget(IEnumerable<Character> targets)
        {
            Character character = null;

            var validIndexes = targets.Select((_, i) => i + 1);
            int chosenIndex = -1;

            while (!validIndexes.Contains(chosenIndex))
            {
                chosenIndex = _userInput.SelectIndex();

                if (!validIndexes.Contains(chosenIndex))
                {
                    _gameOutput.WriteLine($"Invalid choice! Please enter one of: {string.Join(", ", validIndexes)}");
                    continue;
                }

                // subtract 1 to avoid off-by-one errors
                character = targets.ToArray()[chosenIndex - 1];
            }

            return character;
        }
    }
}
