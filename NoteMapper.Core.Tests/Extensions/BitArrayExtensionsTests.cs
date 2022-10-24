using System.Collections;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.Tests.Extensions
{
    public static class BitArrayExtensionsTests
    {
        [Test]
        public static void ToArray_LengthShorterThanBitArray_CopiesSubset()
        {
            BitArray bitArray = new BitArray(4);
            bitArray.Set(0, true);
            bitArray.Set(1, false);
            bitArray.Set(2, true);
            bitArray.Set(3, false);

            bool[] array = bitArray.ToArray(3);

            CollectionAssert.AreEqual(new[]
            {
                true, false, true
            }, array);
        }

        [Test]
        public static void ToArray_LengthEqualToBitArrayLength_CopiesSet()
        {
            BitArray bitArray = new BitArray(4);
            bitArray.Set(0, true);
            bitArray.Set(1, false);
            bitArray.Set(2, true);
            bitArray.Set(3, true);

            bool[] array = bitArray.ToArray(4);

            CollectionAssert.AreEqual(new[]
            {
                true, false, true, true
            }, array);
        }

        [Test]
        public static void ToArray_LengthGreaterThanBitArrayLength_CopiesSet()
        {
            BitArray bitArray = new BitArray(4);
            bitArray.Set(0, true);
            bitArray.Set(1, false);
            bitArray.Set(2, true);
            bitArray.Set(3, true);

            bool[] array = bitArray.ToArray(5);

            CollectionAssert.AreEqual(new[]
            {
                true, false, true, true, false
            }, array);
        }
    }
}
