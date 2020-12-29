﻿using System;
using System.Collections.Generic;
using BattleSystem.Characters;
using BattleSystem.Stats;
using NUnit.Framework;

namespace BattleSystem.Tests.Characters
{
    /// <summary>
    /// Unit tests for <see cref="Character"/>.
    /// </summary>
    [TestFixture]
    public class CharacterTests
    {
        [Test]
        public void Stats_Get_NoItem_ReturnsStats()
        {
            // Arrange
            var character = TestHelpers.CreateBasicCharacter(attack: 1, defence: 2, speed: 3);

            // Act
            var stats = character.Stats;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(stats.Attack.BaseValue, Is.EqualTo(1));
                Assert.That(stats.Defence.BaseValue, Is.EqualTo(2));
                Assert.That(stats.Speed.BaseValue, Is.EqualTo(3));
            });
        }

        [Test]
        public void Stats_Get_WithItem_ReturnsTransformedStats()
        {
            // Arrange
            var character = TestHelpers.CreateBasicCharacter(attack: 10, defence: 20, speed: 30);

            var item = TestHelpers.CreateItem(statTransforms: ss =>
            {
                return new StatSet
                {
                    Attack = new Stat((int) (ss.Attack.BaseValue * 1.1)),
                    Defence = new Stat((int) (ss.Defence.BaseValue * 1.2)),
                    Speed = new Stat((int) (ss.Speed.BaseValue * 1.3)),
                };
            });
            _ = character.EquipItem(item);

            // Act
            var stats = character.Stats;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(stats.Attack.BaseValue, Is.EqualTo(11));
                Assert.That(stats.Defence.BaseValue, Is.EqualTo(24));
                Assert.That(stats.Speed.BaseValue, Is.EqualTo(39));
            });
        }

        [Test]
        public void EquipItem_FirstItem_SetsItem()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();
            var item = TestHelpers.CreateItem();

            // Act
            var result = target.EquipItem(item);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(target.ItemSlot.Current, Is.EqualTo(item));
                Assert.That(result.Success, Is.True);
                Assert.That(result.HadPreviousItem, Is.False);
                Assert.That(result.PreviousItem, Is.Null);
            });
        }

        [Test]
        public void EquipItem_SubsequentItem_SetsItem()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();

            var item1 = TestHelpers.CreateItem();
            _ = target.EquipItem(item1);

            var item2 = TestHelpers.CreateItem();

            // Act
            var result = target.EquipItem(item2);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(target.ItemSlot.Current, Is.EqualTo(item2));
                Assert.That(result.Success, Is.True);
                Assert.That(result.HadPreviousItem, Is.True);
                Assert.That(result.PreviousItem, Is.EqualTo(item1));
            });
        }

        [Test]
        public void RemoveItem_HasItem_RemovesItem()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();
            var item = TestHelpers.CreateItem();
            _ = target.EquipItem(item);

            // Act
            var result = target.RemoveItem();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(target.ItemSlot.HasItem, Is.False);
                Assert.That(result.Success, Is.True);
                Assert.That(result.Item, Is.EqualTo(item));
            });
        }

        [Test]
        public void RemoveItem_HasNoItem_Fails()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();

            // Act
            var result = target.RemoveItem();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.Item, Is.Null);
            });
        }

        [Test]
        public void ReceiveDamage_TakesDamage()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter(maxHealth: 5);

            // Act
            _ = target.ReceiveDamage(2, "omd");

            // Assert
            Assert.That(target.CurrentHealth, Is.EqualTo(3));
        }

        [Test]
        public void ReceiveDamage_WithProtectCounter_TakesNoDamage()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter(maxHealth: 5);
            target.AddProtect("userId");

            // Act
            _ = target.ReceiveDamage(2, "omd");

            // Assert
            Assert.That(target.CurrentHealth, Is.EqualTo(5));
        }

        [Test]
        public void ReceiveDamage_IsDeadIfNoHealthLeft()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter(maxHealth: 5);

            // Act
            _ = target.ReceiveDamage(6, "omd");

            // Assert
            Assert.That(target.IsDead, Is.True);
        }

        [Test]
        public void ReceiveBuff_ChangesStatMultipliers()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter(attack: 10, defence: 10, speed: 10);

            // Act
            target.ReceiveBuff(new Dictionary<StatCategory, double>
            {
                [StatCategory.Attack] = 0.2,
                [StatCategory.Defence] = -0.3,
                [StatCategory.Speed] = -0.1,
            }, "omd");

            // Assert
            Assert.That(target.Stats.Attack.CurrentValue, Is.EqualTo(12));
            Assert.That(target.Stats.Defence.CurrentValue, Is.EqualTo(7));
            Assert.That(target.CurrentSpeed, Is.EqualTo(9));
        }

        [Test]
        public void Heal_AddsHealth()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter(maxHealth: 5);
            _ = target.ReceiveDamage(2, "omd");

            // Act
            target.Heal(2, "omd");

            // Assert
            Assert.That(target.CurrentHealth, Is.EqualTo(5));
        }

        [Test]
        public void AddProtect_AddsProtectActionToQueue()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();

            // Act
            _ = target.AddProtect("DJ rozwell");

            // Assert
            Assert.That(target.ProtectCount, Is.EqualTo(1));
        }

        [Test]
        public void AddProtect_LimitReached_ProtectActionNotAddedToQueue()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();
            _ = target.AddProtect("DJ rozwell");

            // Act
            _ = target.AddProtect(target.Id);

            // Assert
            Assert.That(target.ProtectCount, Is.EqualTo(1));
        }

        [Test]
        public void ChangeProtectLimit_ChangesProtectLimit()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();
            _ = target.AddProtect("DJ rozwell");
            _ = target.ChangeProtectLimit(1, target.Id); // ensures this isn't protected against

            // Act
            _ = target.AddProtect(target.Id); // ensures this isn't protected against

            // Assert
            Assert.That(target.ProtectCount, Is.EqualTo(2));
        }

        [Test]
        public void ConsumeProtect_ReturnsIdOfProtectUser()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();
            _ = target.AddProtect("DJ rozwell");

            // Act
            var userId = target.ConsumeProtect();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(target.ProtectCount, Is.Zero);
                Assert.That(userId, Is.EqualTo("DJ rozwell"));
            });
        }

        [Test]
        public void ConsumeProtect_EmptyProtectQueue_Throws()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();

            // Act and Assert
            Assert.Throws<InvalidOperationException>(() => _ = target.ConsumeProtect());
        }

        [Test]
        public void ClearProtectQueue_EmptiesProtectQueue()
        {
            // Arrange
            var target = TestHelpers.CreateBasicCharacter();
            _ = target.AddProtect("DJ rozwell");
            _ = target.AddProtect("DJ rozwell");

            // Act
            target.ClearProtectQueue();

            // Act and Assert
            Assert.That(target.ProtectCount, Is.Zero);
        }
    }
}
