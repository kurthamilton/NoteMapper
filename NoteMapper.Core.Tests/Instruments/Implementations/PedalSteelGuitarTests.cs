using NoteMapper.Core.Instruments.Implementations;

namespace NoteMapper.Core.Tests.Instruments.Implementations
{
    public static class PedalSteelGuitarTests
    {
        [Test]
        public static void Test()
        {
            PedalSteelGuitar psg = PedalSteelGuitar.Custom(new[]
            {
                "C3|f=0-12",
                "E3|f=0-12",
                "G3|f=0-12"
            }, new[]
            {
                "A:0+2",
                "B:1+2",
                "C:2+2"
            });

            Scale scale = Scale.Major("F");

            var permutations = psg.GetPermutations(scale, 5).ToArray();
        }
    }
}
