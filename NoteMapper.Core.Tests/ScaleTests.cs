namespace NoteMapper.Core.Tests
{
    public static class ScaleTests
    {
        [TestCase("A", ExpectedResult = "A0,B0,C#1,D1,E1,F#1,G#1")]
        [TestCase("C", ExpectedResult = "C0,D0,E0,F0,G0,A0,B0")]
        [TestCase("E", ExpectedResult = "E0,F#0,G#0,A0,B0,C#1,D#1")]
        [TestCase("F", ExpectedResult = "F0,G0,A0,A#0,C1,D1,E1")]
        public static string Major(string key)
        {
            Scale scale = Scale.Parse(key);
            return string.Join(",", scale.Select(x => x.ToString()));
        }

        [TestCase("Cm", ExpectedResult = "C0,D0,D#0,F0,G0,G#0,A#0")]        
        public static string Minor(string key)
        {
            Scale scale = Scale.Parse(key);
            return string.Join(",", scale.Select(x => x.ToString()));
        }

        [TestCase("C", ExpectedResult = "C0,D0,E0,F0,G0,A0,B0,C1,D1,E1,F1,G1,A1,B1,C2")]
        public static string NotesBetween(string key)
        {
            Scale scale = Scale.Parse(key);
            IEnumerable<Note>? notes = scale?.NotesBetween(0, 24) ?? Enumerable.Empty<Note>();
            return string.Join(",", notes.Select(x => x.ToString()));
        }
    }
}