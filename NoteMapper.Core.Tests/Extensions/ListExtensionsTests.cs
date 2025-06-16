using FluentAssertions;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.Tests.Extensions
{
    public static class ListExtensionsTests
    {
        [Test]
        public static void SetCount_SameCount_DoesNotAlter()
        {
            // Arrange
            List<int> list = new()
            {
                1, 2, 3
            };

            // Act
            list.SetCount(3, 0);

            // Assert
            list.Should().BeEquivalentTo(
            [
                1, 2, 3
            ]);
        }

        [Test]
        public static void SetCount_AddItems()
        {
            // Arrange
            List<int> list = new()
            {
                1, 2
            };

            // Act
            list.SetCount(3, 0);

            // Assert
            list.Should().BeEquivalentTo(
            [
                1, 2, 0
            ]);
        }

        [Test]
        public static void SetCount_RemoveItems()
        {
            // Arrange
            List<int> list = new()
            {
                1, 2, 3, 4
            };

            // Act
            list.SetCount(3, 0);

            // Assert
            list.Should().BeEquivalentTo(
            [
                1, 2, 3
            ]);
        }
    }
}
