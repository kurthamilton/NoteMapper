namespace NoteMapper.Core.MusicTheory
{
    public interface INoteCollection : IReadOnlyCollection<Note>
    {
        bool Contains(Note note);
    }
}
