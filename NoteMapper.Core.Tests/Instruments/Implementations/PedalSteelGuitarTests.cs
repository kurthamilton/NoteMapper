using FluentAssertions;
using NoteMapper.Core.Guitars;
using NoteMapper.Core.Guitars.Implementations;
using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Core.Tests.Instruments.Implementations
{
    public static class PedalSteelGuitarTests
    {
        [Test]
        public static void GetPermutations_NoModifers_ReturnsStandardNotes()
        {
            // Arrange
            PedalSteelGuitar psg = PedalSteelGuitar.Custom("id", "custom", new PedalSteelGuitarConfig
            { 
                Strings =
                [
                    "n=0|o=3|f=0-12",
                    "n=4|o=3|f=0-12",
                    "n=7|o=3|f=0-12"
                ]                
            });

            INoteCollection notes = Note.GetNoteCollection(new NoteCollectionOptions
            {
                ScaleType = ScaleType.Major, 
                Type = NoteCollectionType.Chord
            });
            StringPermutationOptions options = new(notes, 0, null, 0);
            
            // Act
            IReadOnlyCollection<IReadOnlyCollection<GuitarStringNote?>> permutations = 
                psg.GetPermutations(options).ToArray();

            // Asser
            var actual = permutations
                .Select(x => string.Join(",", x.Select(p => p != null ? $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}" : "")))
                .ToArray();

            actual.Should().BeEquivalentTo(
            [
                "C3,E3,G3"
            ]);
        }

        [Test]
        public static void GetPermutations_InvalidModifiedNotesAreIgnored()
        {
            // Arrange
            PedalSteelGuitar psg = PedalSteelGuitar.Custom("id", "custom", new PedalSteelGuitarConfig
            {
                Modifiers =
                [
                    "Pedal|A|0+2",
                    "Pedal|B|1+2",
                    "Pedal|C|2+2",
                    "Pedal|D|3+1"
                ],
                Strings =
                [
                    "n=0|o=3|f=0-12",
                    "n=4|o=3|f=0-12",
                    "n=7|o=3|f=0-12",
                    "n=11|o=3|f=0-12"
                ]
            });

            INoteCollection notes = Note.GetNoteCollection(new NoteCollectionOptions
            {
                ScaleType = ScaleType.Major,
                Type = NoteCollectionType.Chord
            });
            StringPermutationOptions options = new(notes, 0, null, 0);
            
            // Act
            IReadOnlyCollection<IReadOnlyCollection<GuitarStringNote?>> permutations =
                psg.GetPermutations(options).ToArray();

            // Assert
            var actual = permutations
                .Select(x => string.Join(",", x.Select(p => p != null ? $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}" : "")))
                .ToArray();

            actual.Should().BeEquivalentTo(
            [
                "C3,E3,G3,",
                "C3,E3,G3,C4(D)"
            ]);
        }

        [Test]
        public static void GetPermutations_StringHasNoValidNote_ReturnsEmpty()
        {
            // Arrange
            PedalSteelGuitar psg = PedalSteelGuitar.Custom("id", "custom", new PedalSteelGuitarConfig
            {
                Modifiers =
                [
                    "Pedal|A|0+2",
                    "Pedal|B|1+2",
                    "Pedal|C|2+2"
                ],
                Strings =
                [
                    "n=0|o=3|f=0-12",
                    "n=4|o=3|f=0-12",
                    "n=7|o=3|f=0-12"
                ]
            });

            INoteCollection notes = Note.GetNoteCollection(new NoteCollectionOptions
            {
                ScaleType = ScaleType.Major,
                Type = NoteCollectionType.Chord
            });
            StringPermutationOptions options = new(notes, 1, null, 0);
            
            // Act
            var permutations =
                psg.GetPermutations(options).ToArray();

            // Assert
            var actual = permutations
                .Select(x => string.Join(",", x.Select(p => p != null ? $"{p.Note}{(p.Modifier != null ? "(" + p.Modifier.Name + ")" : "")}" : "")))
                .ToArray();

            actual.Should().BeEmpty();
        }
    }
}
