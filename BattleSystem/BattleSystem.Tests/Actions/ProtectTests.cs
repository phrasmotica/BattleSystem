﻿using System;
using System.Collections.Generic;
using System.Linq;
using BattleSystem.Actions;
using BattleSystem.Actions.Targets;
using BattleSystem.Characters;
using Moq;
using NUnit.Framework;

namespace BattleSystem.Tests.Actions
{
    /// <summary>
    /// Unit tests for <see cref="Protect"/>.
    /// </summary>
    [TestFixture]
    public class ProtectTests
    {
        [Test]
        public void Use_CalculationSuccessfulWithTargets_BumpsTargetsProtectCounter()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter()
            };

            var protect = TestHelpers.CreateProtect(new OthersActionTargetCalculator());

            protect.SetTargets(user, otherCharacters);

            // Act
            _ = protect.Use<string>(user, otherCharacters);

            // Assert
            Assert.That(otherCharacters[0].ProtectCount, Is.EqualTo(1));
        }

        [Test]
        public void Use_CalculationSuccessfulWithTargets_SucceedsAndAppliesActions()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter()
            };

            var protect = TestHelpers.CreateProtect(new OthersActionTargetCalculator());

            protect.SetTargets(user, otherCharacters);

            // Act
            var (success, results) = protect.Use<string>(user, otherCharacters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(results, Is.Not.Empty);
            });
        }

        [Test]
        public void Use_CalculationSuccessfulAllTargetsDead_SucceedsAndAppliesNoActions()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();

            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(maxHealth: 0)
            };

            var protect = TestHelpers.CreateProtect(new OthersActionTargetCalculator());

            protect.SetTargets(user, otherCharacters);

            // Act
            var (success, results) = protect.Use<string>(user, otherCharacters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(results, Is.Empty);
            });
        }

        [Test]
        public void Use_CalculationSuccessfulNoTargets_SucceedsAndAppliesNoActions()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter()
            };

            var actionTargetCalculator = new Mock<IActionTargetCalculator>();
            actionTargetCalculator
                .Setup(
                    m => m.Calculate(
                        It.IsAny<Character>(),
                        It.IsAny<IEnumerable<Character>>()
                    )
                )
                .Returns((true, Enumerable.Empty<Character>()));

            var protect = TestHelpers.CreateProtect(
                actionTargetCalculator: actionTargetCalculator.Object);

            protect.SetTargets(user, otherCharacters);

            // Act
            var (success, results) = protect.Use<string>(user, otherCharacters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.True);
                Assert.That(results, Is.Empty);
            });
        }

        [Test]
        public void Use_NoTargetsSet_FailsAndAppliesNoActions()
        {
            // Arrange
            var user = TestHelpers.CreateBasicCharacter();
            var otherCharacters = new[]
            {
                TestHelpers.CreateBasicCharacter(),
            };

            var protect = TestHelpers.CreateProtect();

            // Act
            var (success, results) = protect.Use<string>(user, otherCharacters);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(success, Is.False);
                Assert.That(results, Is.Empty);
            });
        }
    }
}
