using System.Collections;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.Permutations
{
    public class Permutation : IEnumerable<bool>
    {
        private readonly IReadOnlyCollection<bool> _values;

        private Permutation(IReadOnlyCollection<bool> values)
        {
            _values = values;
            Count = values.Count;
        }        

        public int Count { get; }

        public static IReadOnlyCollection<Permutation> GetPermutations(int numberOfOptions)
        {
            int numberOfPermutations = (int)Math.Pow(2, numberOfOptions);

            List<Permutation> permutations = new List<Permutation>();

            for (int i = 0; i < numberOfPermutations; i++)
            {
                BitArray bitArray = new BitArray(new[] { i });                
                bool[] array = bitArray.ToArray(numberOfOptions);
                
                Permutation permutation = new Permutation(array);
                permutations.Add(permutation);
            }

            return permutations;
        }

        public bool Get(int i)
        {
            return i >=0 && i < _values.Count 
                ? _values.ElementAt(i)
                : false;
        }

        public IEnumerator<bool> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
