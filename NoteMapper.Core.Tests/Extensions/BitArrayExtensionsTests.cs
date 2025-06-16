using System.Collections;
using FluentAssertions;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.Tests.Extensions
{
    public static class BitArrayExtensionsTests
    {
        [Test]
        public static void ToArray_LengthShorterThanBitArray_CopiesSubset()
        {
            // Arrange
            BitArray bitArray = new(4);
            bitArray.Set(0, true);
            bitArray.Set(1, false);
            bitArray.Set(2, true);
            bitArray.Set(3, false);

            // Act
            var result = bitArray.ToArray(3);

            // Assert
            result.Should().BeEquivalentTo(
            [
                true, false, true
            ]);
        }

        [Test]
        public static void ToArray_LengthEqualToBitArrayLength_CopiesSet()
        {
            // Arrange
            BitArray bitArray = new(4);
            bitArray.Set(0, true);
            bitArray.Set(1, false);
            bitArray.Set(2, true);
            bitArray.Set(3, true);

            // Act
            var result = bitArray.ToArray(4);

            result.Should().BeEquivalentTo(
            [
                true, false, true, true
            ]);
        }

        [Test]
        public static void ToArray_LengthGreaterThanBitArrayLength_CopiesSet()
        {
            // Arrange
            BitArray bitArray = new(4);
            bitArray.Set(0, true);
            bitArray.Set(1, false);
            bitArray.Set(2, true);
            bitArray.Set(3, true);

            // Act
            var result = bitArray.ToArray(5);

            // Assert
            result.Should().BeEquivalentTo(
            [
                true, false, true, true, false
            ]);
        }

        [TestCase("10000", ExpectedResult = 1)]
        [TestCase("01000", ExpectedResult = 2)]
        [TestCase("11000", ExpectedResult = 3)]
        [TestCase("11110", ExpectedResult = 15)]
        public static int ToInt(string bits)
        {
            BitArray bitArray = new(bits.Length);
            for (int i = 0; i < bits.Length; i++)
            {
                bitArray.Set(i, bits[i] == '1');
            }

            return bitArray.ToInt();
        }
    }
}
