﻿using System.Linq;
using BattleSystem.Core.Moves;
using NUnit.Framework;

namespace BattleSystem.Core.Tests.Moves
{
    /// <summary>
    /// Unit tests for <see cref="MoveUse"/>.
    /// </summary>
    [TestFixture]
    public class MoveUseTests
    {
        [Test]
        public void Apply_ReturnsResult()
        {
            // Arrange
            var move = TestHelpers.CreateMove(moveActions: TestHelpers.CreateDamageAction());

            var moveUse = new MoveUse
            {
                Move = move,
                User = TestHelpers.CreateBasicCharacter(),
                OtherCharacters = new[]
                {
                    TestHelpers.CreateBasicCharacter()
                }
            };

            moveUse.SetTargets();

            // Act
            moveUse.Apply();

            // Assert
            Assert.That(moveUse.ActionsResults.ToArray().Length, Is.EqualTo(1));
        }
    }
}
