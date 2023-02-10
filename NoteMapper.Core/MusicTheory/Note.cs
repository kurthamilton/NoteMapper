﻿using System.Text.RegularExpressions;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core.MusicTheory
{
    public class Note
    {
        private static IReadOnlyCollection<string> Notes = new[]
        { 
            // start at C to align with octave indexes incrementing at C
            // skip the accidental notes - their formatting depends on which accidental is preferred
            "C", "", "D", "", "E", "F", "", "G", "", "A", "", "B",
        };

        public Note(int index)
        {
            while (index < 0)
            {
                index += Notes.Count;
            }

            Index = index;
            NoteIndex = index % Notes.Count;
            OctaveIndex = (int)Math.Floor((double)index / Notes.Count);
        }

        public Note(int noteIndex, int octaveIndex)
            : this(octaveIndex * Notes.Count + noteIndex)
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

        public static string GetName(int noteIndex, AccidentalType accidental)
        {
            noteIndex %= Notes.Count;

            string name = Notes.ElementAt(noteIndex);
            if (!string.IsNullOrEmpty(name))
            {
                // return natural note
                return name;
            }

            string suffix = Accidental.ToString(accidental);
            
            // get natural note index in opposite direction to the effect of the accidental
            int naturalNoteIndex = noteIndex - (int)accidental;
            string naturalName = GetName(naturalNoteIndex, accidental);
            return naturalName + suffix;
        }

        public static IReadOnlyCollection<string> GetNotes(AccidentalType accidental)
        {
            string[] notes = new string[Notes.Count];
            for (int i = 0; i < Notes.Count; i++)
            {
                string name = GetName(i, accidental);
                notes[i] = name;
            }

            return notes;
        }

        public static IReadOnlyCollection<int> GetNoteIndexes()
        {
            return Notes
                .Select((x, i) => i)
                .ToArray();
        }

        public static INoteCollection GetNotes(int noteIndex, string key, NoteCollectionType type)
        {
            switch (type)
            {
                case NoteCollectionType.Chord:
                    return Chord.Parse(noteIndex, key);
                case NoteCollectionType.Scale:
                    return Scale.Parse(noteIndex, key);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }

        public static int GetNoteIndex(int index)
        {
            return index % Notes.Count;
        }

        public static IReadOnlyCollection<int> GetOctaves()
        {
            return new[]
            {
                0, 1, 2, 3, 4, 5, 6, 7, 8
            };
        }

        public string GetName(AccidentalType accidental)
        {
            return GetName(NoteIndex, accidental);
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