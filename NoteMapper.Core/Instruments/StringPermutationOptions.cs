namespace NoteMapper.Core.Instruments
{
    public class StringPermutationOptions
    {
        public StringPermutationOptions(INoteCollection notes, int position)
        {
            Notes = notes;
            Position = position;
        }

        public INoteCollection Notes { get; }

        public int Position { get; }
    }
}
