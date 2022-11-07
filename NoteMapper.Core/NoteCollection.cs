using System.Collections;
using System.Collections.ObjectModel;

namespace NoteMapper.Core
{
    public abstract class NoteCollection : INoteCollection
    {
        private readonly Lazy<IReadOnlyDictionary<int, Note>> _noteDictionary;
        private readonly IReadOnlyCollection<Note> _notes;

        protected NoteCollection(IEnumerable<Note> notes)
        {
            _notes = notes.ToArray();
            _noteDictionary = new Lazy<IReadOnlyDictionary<int, Note>>(() =>
                new ReadOnlyDictionary<int, Note>(_notes.ToDictionary(x => x.NoteIndex)));
        }

        public int Count => _notes.Count;

        public bool Contains(Note note)
        {
            return Contains(note.NoteIndex);
        }

        public IEnumerator<Note> GetEnumerator()
        {
            return _notes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected bool Contains(int noteIndex)
        {
            return _noteDictionary.Value.ContainsKey(noteIndex);
        }
    }
}
