using System.Text.RegularExpressions;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.MusicTheory
{
    public class Note
    {
        public const string Flat = "♭";

        private static readonly Regex _noteRegex = new(@"^(?<natural>[A-G])(?<accidental>#|♭)?(?<octave>\d+)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static IReadOnlyCollection<string> _notes = new[]
        { 
            // start at C to align with octave indexes incrementing at C
            // skip the accidental notes - their formatting depends on which
            // accidental is preferred
            "C", "", "D", "", "E", "F", "", "G", "", "A", "", "B",
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
        }

        public Note(int noteIndex, int octaveIndex)
            : this(octaveIndex * _notes.Count + noteIndex)
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

        public int OctaveIndex { get; }

        public static IReadOnlyCollection<string> GetNotes(AccidentalType accidental)
        {
            string[] notes = new string[_notes.Count];
            for (int i = 0; i < _notes.Count; i++)
            {
                string name = new Note(i).GetName(accidental);
                notes[i] = name;
            }

            return notes;
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

            string natural = match.Groups["natural"].Value;
            string accidental = match.Groups["accidental"].Success
                ? match.Groups["accidental"].Value
                : "";
            int octave = match.Groups["octave"].Success
                ? int.Parse(match.Groups["octave"].Value)
                : 0;

            int index = _notes.IndexOf(natural);

            if (!string.IsNullOrEmpty(accidental))
            {
                if (accidental == "#")
                {
                    index++;
                }
                else if (accidental == Flat)
                {
                    index--;
                }
            }

            if (index < 0 || index >= _notes.Count)
            {
                throw new ArgumentException($"Note '{name}' not found", nameof(name));
            }

            index += octave * _notes.Count;

            return new Note(index);
        }

        public string GetName(AccidentalType accidental)
        {
            string name = _notes.ElementAt(NoteIndex);
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
                        
            switch (accidental)
            {
                case AccidentalType.Sharp:
                    int previousIndex = (NoteIndex - 1) % _notes.Count;
                    return $"{_notes.ElementAt(previousIndex)}#";
                case AccidentalType.Flat:
                    int nextIndex = (NoteIndex + 1) % _notes.Count;
                    return $"{_notes.ElementAt(nextIndex)}{Flat}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Note Next(int offset)
        {
            return new Note(Index + offset);
        }

        public override string ToString()
        {
            // this method should not be called directly, so it is OK to hard code the accidental type
            return GetName(AccidentalType.Sharp) + OctaveIndex;
        }
    }
}