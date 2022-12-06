using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Core.Tests
{
    public static class ChordTests
    {
        [TestCase("C", ExpectedResult = "C,E,G")]
        [TestCase("E", ExpectedResult = "E,G#,B")]
        [TestCase("Cm", ExpectedResult = "C,D#,G")]
        [TestCase("Em", ExpectedResult = "E,G,B")]
        [TestCase("Cmaj7", ExpectedResult = "C,E,G,B")]
        [TestCase("Emaj7", ExpectedResult = "E,G#,B,D#")]
        [TestCase("Cm7", ExpectedResult = "C,D#,G,A#")]
        [TestCase("Em7", ExpectedResult = "E,G,B,D")]
        [TestCase("C7", ExpectedResult = "C,E,G,A#")]
        [TestCase("E7", ExpectedResult = "E,G#,B,D")]
        public static string Parse(string key)
        {            
            Chord chord = Chord.Parse(key);
            return string.Join(",", chord.Select(x => x.Name));
        }        
    }
}
