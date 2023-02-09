using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Core.Tests
{
    public static class NoteTests
    {
        [TestCase(0, ExpectedResult = "C")]
        [TestCase(2, ExpectedResult = "D")]
        [TestCase(4, ExpectedResult = "E")]
        [TestCase(5, ExpectedResult = "F")]
        [TestCase(7, ExpectedResult = "G")]
        [TestCase(9, ExpectedResult = "A")]
        [TestCase(11, ExpectedResult = "B")]
        public static string GetName_NaturalNotes_ReturnsName(int noteIndex)
        {
            Note note = new Note(noteIndex);
            return note.GetName(AccidentalType.Sharp);
        }

        [TestCase(1, AccidentalType.Sharp, ExpectedResult = "C#")]
        [TestCase(1, AccidentalType.Flat, ExpectedResult = "D♭")]
        [TestCase(3, AccidentalType.Sharp, ExpectedResult = "D#")]
        [TestCase(3, AccidentalType.Flat, ExpectedResult = "E♭")]
        [TestCase(6, AccidentalType.Sharp, ExpectedResult = "F#")]
        [TestCase(6, AccidentalType.Flat, ExpectedResult = "G♭")]
        [TestCase(8, AccidentalType.Sharp, ExpectedResult = "G#")]
        [TestCase(8, AccidentalType.Flat, ExpectedResult = "A♭")]
        [TestCase(10, AccidentalType.Sharp, ExpectedResult = "A#")]
        [TestCase(10, AccidentalType.Flat, ExpectedResult = "B♭")]
        public static string GetName_AccidentalNotes_ReturnsName(int noteIndex, AccidentalType accidental)
        {
            Note note = new Note(noteIndex);
            return note.GetName(accidental);
        }

        [TestCase(0, 0, 0, ExpectedResult = "00")]
        [TestCase(0, 0, 12, ExpectedResult = "01")]
        [TestCase(0, 4, 5, ExpectedResult = "54")]
        public static string Next(int noteIndex, int octaveIndex, int offset)
        {
            Note note = new(noteIndex, octaveIndex);
            Note next = note.Next(offset);
            return $"{next.NoteIndex}{next.OctaveIndex}";
        }

        [TestCase("C", ExpectedResult = 0)]
        [TestCase("C#", ExpectedResult = 1)]
        [TestCase("D♭", ExpectedResult = 1)]
        [TestCase("D", ExpectedResult = 2)]
        [TestCase("D#", ExpectedResult = 3)]
        [TestCase("E♭", ExpectedResult = 3)]
        [TestCase("E", ExpectedResult = 4)]
        [TestCase("F", ExpectedResult = 5)]
        [TestCase("F#", ExpectedResult = 6)]
        [TestCase("G♭", ExpectedResult = 6)]
        [TestCase("G", ExpectedResult = 7)]
        [TestCase("G#", ExpectedResult = 8)]
        [TestCase("A♭", ExpectedResult = 8)]
        [TestCase("A", ExpectedResult = 9)]
        [TestCase("A#", ExpectedResult = 10)]
        [TestCase("B♭", ExpectedResult = 10)]
        [TestCase("B", ExpectedResult = 11)]
        public static int Parse(string note)
        {
            Note result = Note.Parse(note);
            return result.NoteIndex;
        }
    }
}
