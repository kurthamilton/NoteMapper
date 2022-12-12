using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;
using NoteMapper.Core.MusicTheory;
using NUnit.Framework;

namespace NoteMapper.Core.Tests.Instruments.Implementations
{
    public static class PedalSteelGuitarTests
    {
        [Test]
        public static void GetPermutations_NoModifers_ReturnsStandardNotes()
        {
            PedalSteelGuitar psg = PedalSteelGuitar.Custom("id", "custom", new PedalSteelGuitarConfig
            { 
                Strings = new[]
                {
                    "C3|f=0-12",
                    "E3|f=0-12",
                    "G3|f=0-12"
                }                
            });

            INoteCollection notes = Note.GetNotes(NoteMapType.Chord, "C");
            StringPermutationOptions options = new StringPermutationOptions(notes, 0);
            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote?>> permutations = 
                psg.GetPermutations(options).ToArray();

            string[] expected = new[]
            {
                "C3,E3,G3"
            };

            string[] actual = permutations
                .Select(x => string.Join(",", x.Select(p => p != null ? $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}" : "")))
                .ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public static void GetPermutations_InvalidModifiedNotesAreIgnored()
        {
            PedalSteelGuitar psg = PedalSteelGuitar.Custom("id", "custom", new PedalSteelGuitarConfig
            {
                Modifiers = new[]
                {
                    "A|0+2",
                    "B|1+2",
                    "C|2+2",
                    "D|3+1"
                },
                Strings = new[]
                {
                    "C3|f=0-12",
                    "E3|f=0-12",
                    "G3|f=0-12",
                    "B3|f=0-12"
                }
            });

            INoteCollection notes = Note.GetNotes(NoteMapType.Chord, "C");
            StringPermutationOptions options = new StringPermutationOptions(notes, 0);
            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote?>> permutations =
                psg.GetPermutations(options).ToArray();

            string[] expected = new[]
            {
                "C3,E3,G3,",
                "C3,E3,G3,C4(D)"
            };

            string[] actual = permutations
                .Select(x => string.Join(",", x.Select(p => p != null ? $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}" : "")))
                .ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public static void GetPermutations_StringHasNoValidNote_ReturnsEmpty()
        {
            PedalSteelGuitar psg = PedalSteelGuitar.Custom("id", "custom", new PedalSteelGuitarConfig
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

            INoteCollection notes = Note.GetNotes(NoteMapType.Chord, "C");
            StringPermutationOptions options = new StringPermutationOptions(notes, 1);
            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote?>> permutations =
                psg.GetPermutations(options).ToArray();

            string[] actual = permutations
                .Select(x => string.Join(",", x.Select(p => p != null ? $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}" : "")))
                .ToArray();

            CollectionAssert.IsEmpty(actual);
        }
    }
}
