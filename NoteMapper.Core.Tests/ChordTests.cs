namespace NoteMapper.Core.Tests
{
    public static class ChordTests
    {
        [TestCase("C", ExpectedResult = "C,E,G")]
        [TestCase("E", ExpectedResult = "E,G#,B")]
        public static string Major(string key)
        {            
            Chord chord = Chord.Parse(key);
            return string.Join(",", chord.Select(x => x.Name));
        }

        [TestCase("Cm", ExpectedResult = "C,D#,G")]
        [TestCase("Em", ExpectedResult = "E,G,B")]
        public static string Minor(string key)
        {
            Chord chord = Chord.Parse(key);
            return string.Join(",", chord.Select(x => x.Name));
        }
    }
}
