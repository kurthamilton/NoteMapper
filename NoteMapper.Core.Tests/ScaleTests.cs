namespace NoteMapper.Core.Tests
{
    public static class ScaleTests
    {
        [TestCase("A", ExpectedResult = "A0,B0,C#0,D0,E0,F#0,G#0")]
        [TestCase("C", ExpectedResult = "C0,D0,E0,F0,G0,A1,B1")]
        [TestCase("E", ExpectedResult = "E0,F#0,G#0,A1,B1,C#1,D#1")]
        public static string Major(string key)
        {
            Scale scale = Scale.Major(key);
            return string.Join(",", scale.Select(x => x.ToString()));
        }

        [TestCase("C", ExpectedResult = "A0,B0,C0,D0,E0,F0,G0,A1,B1,C1,D1,E1,F1,G1,A2")]
        public static string NotesBetween(string key)
        {
            Scale scale = Scale.Major(key);
            IEnumerable<Note> notes = scale.NotesBetween(0, 24);
            return string.Join(",", notes.Select(x => x.ToString()));
        }
    }
}