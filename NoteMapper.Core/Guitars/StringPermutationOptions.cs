using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Core.Guitars
{
    public class StringPermutationOptions
    {
        public StringPermutationOptions(INoteCollection notes, int fret)
        {
            Fret = fret;
            Notes = notes;
        }

        public int Fret { get; }

        public INoteCollection Notes { get; }        
    }
}
