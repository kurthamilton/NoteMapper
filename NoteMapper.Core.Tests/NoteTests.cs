namespace NoteMapper.Core.Tests
{
    public static class NoteTests
    {
        [TestCase("C0", 0, ExpectedResult = "C0")]
        [TestCase("C0", 12, ExpectedResult = "C1")]
        [TestCase("C4", 5, ExpectedResult = "F4")]
        public static string Next(string name, int offset)
        {
            Note note = Note.FromName(name);
            Note next = note.Next(offset);
            return next.ToString();
        }
    }
}
