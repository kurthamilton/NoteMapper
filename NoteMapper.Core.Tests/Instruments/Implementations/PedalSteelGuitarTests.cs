using NoteMapper.Core.Guitars.Implementations;
using NoteMapper.Core.Guitars;
using NoteMapper.Core.Guitars.Implementations;
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
            StringPermutationOptions options = new(notes, 0);
            IReadOnlyCollection<IReadOnlyCollection<GuitarStringNote?>> permutations = 
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
                    "Pedal|A|0+2",
                    "Pedal|B|1+2",
                    "Pedal|C|2+2",
                    "Pedal|D|3+1"
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
            StringPermutationOptions options = new(notes, 0);
            IReadOnlyCollection<IReadOnlyCollection<GuitarStringNote?>> permutations =
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
                    "Pedal|A|0+2",
                    "Pedal|B|1+2",
                    "Pedal|C|2+2"
                },
                Strings = new[]
                {
                    "C3|f=0-12",
                    "E3|f=0-12",
                    "G3|f=0-12"
                }
            });

            INoteCollection notes = Note.GetNotes(NoteMapType.Chord, "C");
            StringPermutationOptions options = new(notes, 1);
            IReadOnlyCollection<IReadOnlyCollection<GuitarStringNote?>> permutations =
                psg.GetPermutations(options).ToArray();

            string[] actual = permutations
                .Select(x => string.Join(",", x.Select(p => p != null ? $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}" : "")))
                .ToArray();

            CollectionAssert.IsEmpty(actual);
        }
    }
}
