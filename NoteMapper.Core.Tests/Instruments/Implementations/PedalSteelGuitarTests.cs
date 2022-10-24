using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;

namespace NoteMapper.Core.Tests.Instruments.Implementations
{
    public static class PedalSteelGuitarTests
    {
        [Test]
        public static void GetPermutations_NoModifers_ReturnsStandardNotes()
        {
            PedalSteelGuitar psg = PedalSteelGuitar.Custom(new PedalSteelGuitarConfig
            { 
                Strings = new[]
                {
                    "C3|f=0-12",
                    "E3|f=0-12",
                    "G3|f=0-12"
                }                
            });

            Scale scale = Scale.Major("F");

            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> permutations = 
                psg.GetPermutations(scale, 5).ToArray();

            string[] expected = new[]
            {
                "F3",
                "A3",
                "C4"
            };

            string[] actual = permutations
                .Select(x => string.Join(",", x.Select(p => $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}")))
                .ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public static void GetPermutations_InvalidModifiedNotesAreIgnored()
        {
            PedalSteelGuitar psg = PedalSteelGuitar.Custom(new PedalSteelGuitarConfig
            {
                Modifiers = new[]
                {
                    "A|0+2",
                    "B|1+2",
                    "C|2+2"
                },
                Strings = new[]
                {
                    "C3|f=0-12",
                    "E3|f=0-12",
                    "G3|f=0-12"
                }
            });

            Scale scale = Scale.Major("F");

            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> permutations =
                psg.GetPermutations(scale, 5).ToArray();

            string[] expected = new[]
            {
                "F3,G3(A)",
                "A3",
                "C4,D4(C)"
            };

            string[] actual = permutations
                .Select(x => string.Join(",", x.Select(p => $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}")))
                .ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public static void GetPermutations_StringHasNoValidNote_ReturnsEmpty()
        {
            PedalSteelGuitar psg = PedalSteelGuitar.Custom(new PedalSteelGuitarConfig
            {
                Modifiers = new[]
                {
                    "A|0+2",
                    "B|1+2",
                    "C|2+2"
                },
                Strings = new[]
                {
                    "C3|f=0-12",
                    "E3|f=0-12",
                    "G3|f=0-12"
                }
            });

            Scale scale = Scale.Major("F");

            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> permutations =
                psg.GetPermutations(scale, 6).ToArray();

            string?[] expected = new[]
            {
                "",
                "A#3,C4(B)",
                ""
            };

            string[] actual = permutations
                .Select(x => string.Join(",", x.Select(p => $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}")))
                .ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
