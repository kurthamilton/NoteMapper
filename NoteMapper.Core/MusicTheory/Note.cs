using System.Text.RegularExpressions;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.MusicTheory
{
    public class Note
    {
        private static readonly Regex _noteRegex = new Regex(@"^(?<name>[A-G]#?)(?<octave>\d+)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static IReadOnlyCollection<string> _notes = new[]
        { 
            // start at C to align with octave indexes incrementing at C
            "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B",
        };

        public Note(int index)
        {
            while (index < 0)
            {
                index += _notes.Count;
            }

            Index = index;
            NoteIndex = index % _notes.Count;
            OctaveIndex = (int)Math.Floor((double)index / _notes.Count);
            Name = _notes.ElementAt(NoteIndex);
        }

        public Note(int noteIndex, int octaveIndex)
            : this(octaveIndex * _notes.Count + noteIndex)
        {
        }

        public Note(string noteName, int octaveIndex)
            : this(_notes.IndexOf(noteName), octaveIndex)
        {
        }

        /// <summary>
        /// The index of the note including the octave
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// The index of the note within the chromatic sale
        /// </summary>
        public int NoteIndex { get; }

        public string Name { get; }

        public int OctaveIndex { get; }

        public static IReadOnlyCollection<string> GetNotes()
        {
            return _notes
                .ToArray();
        }

        public static INoteCollection GetNotes(NoteMapType type, string key)
        {
            switch (type)
            {
                case NoteMapType.Chord:
                    return Chord.Parse(key);
                case NoteMapType.Scale:
                    return Scale.Parse(key);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        public static int GetNoteIndex(int index)
        {
            return index % _notes.Count;
        }

        public static IReadOnlyCollection<int> GetOctaves()
        {
            return new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8
            };
        }

        public static Note Parse(string name)
        {
            Match match = _noteRegex.Match(name);
            if (!match.Success)
            {
                throw new ArgumentException("Incorrect format", nameof(name));
            }

            name = match.Groups["name"].Value;
            int octave = match.Groups["octave"].Success
                ? int.Parse(match.Groups["octave"].Value)
                : 0;

            int index = _notes.IndexOf(name);
            if (index < 0)
            {
                throw new ArgumentException($"Note '{name}' not found", nameof(name));
            }

            index += octave * _notes.Count;

            return new Note(index);
        }

        public Note Next(int offset)
        {
            return new Note(Index + offset);
        }

        public override string ToString()
        {
            return Name + OctaveIndex;
        }
    }
}