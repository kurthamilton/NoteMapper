namespace NoteMapper.Core.Tests
{
    public static class ChordTests
    {
        [TestCase("C", ExpectedResult = "C,E,G")]
        [TestCase("E", ExpectedResult = "E,G#,B")]
        public static string Major(string key)
        {            
            Chord chord = Chord.Major(key);
            return string.Join(",", chord.Select(x => x.Name));
        }
    }
}
