using FluentAssertions;
using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Tests.Permutations
{
    public static class PermutationTests
    {
        [TestCase("10101", "10001", ExpectedResult = true)]
        [TestCase("10101", "11001", ExpectedResult = false)]
        public static bool Contains(string bits, string otherBits)
        {
            Permutation permutation = Permutation.Parse(bits);
            Permutation other = Permutation.Parse(otherBits);

            return permutation.Contains(other);
        }

        [Test]
        public static void GetPermutations()
        {
            // Act
            IReadOnlyCollection<Permutation> permutations = Permutation.GetPermutations(3);

            // Assert
            IReadOnlyCollection<IReadOnlyCollection<bool>> expected =
            [
                new[] { false, false, false },
                [true, false, false],
                [false, true, false],
                [true, true, false],
                [false, false, true],
                [true, false, true],
                [false, true, true],
                [true, true, true]
            ];

            permutations.Should().BeEquivalentTo(expected);
        }

        [Test]
        public static void Parse()
        {
            // Arrange
            string bits = "10101";

            // Act
            Permutation permutation = Permutation.Parse(bits);
            
            // Assert
            bool[] actual = permutation.ToArray();

            actual.Should().BeEquivalentTo(
            [
                true, false, true, false, true
            ]);
        }
    }
}
