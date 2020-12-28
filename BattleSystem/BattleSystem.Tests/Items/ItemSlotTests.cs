﻿using BattleSystem.Items;
using NUnit.Framework;

namespace BattleSystem.Tests.Items
{
    /// <summary>
    /// Unit tests for <see cref="ItemSlot"/>.
    /// </summary>
    [TestFixture]
    public class ItemSlotTests
    {
        [Test]
        public void Set_SetsItem()
        {
            // Arrange
            var itemSlot = TestHelpers.CreateItemSlot();
            var item = TestHelpers.CreateItem();

            // Act
            itemSlot.Set(item);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(itemSlot.HasItem, Is.True);
                Assert.That(itemSlot.Current, Is.EqualTo(item));
            });
        }

        [Test]
        public void Remove_RemovesItem()
        {
            // Arrange
            var itemSlot = TestHelpers.CreateItemSlot();
            var item = TestHelpers.CreateItem();
            itemSlot.Set(item);

            // Act
            itemSlot.Remove();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(itemSlot.HasItem, Is.False);
            });
        }
    }
}
