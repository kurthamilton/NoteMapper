using NoteMapper.Core.Permutations;

namespace NoteMapper.Core.Tests.Permutations
{
    public static class PermutationTests
    {
        [Test]
        public static void GetPermutations()
        {
            IReadOnlyCollection<Permutation> permutations = Permutation.GetPermutations(3);

            IReadOnlyCollection<IReadOnlyCollection<bool>> expected = new[]
            {
                new[] { false, false, false },
                new[] { true, false, false },
                new[] { false, true, false },
                new[] { true, true, false },
                new[] { false, false, true },
                new[] { true, false, true },
                new[] { false, true, true },
                new[] { true, true, true }
            };

            CollectionAssert.AreEqual(expected, permutations);

        }
    }
}
