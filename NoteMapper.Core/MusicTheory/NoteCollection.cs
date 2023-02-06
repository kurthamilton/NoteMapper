﻿using System.Collections;
using System.Collections.ObjectModel;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.MusicTheory
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

        public abstract Scale Key { get; }

        public bool Contains(Note note)
        {
            return Contains(note.NoteIndex);
        }

        public int IndexOf(string name)
        {
            for (int i = 0; i < _notes.Count; i++)
            {
                Note note = _notes.ElementAt(i);
                if (string.Equals(note.Name, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
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
