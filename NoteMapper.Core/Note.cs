﻿using System;
using System.Text.RegularExpressions;
using NoteMapper.Core.Extensions;

namespace NoteMapper.Core
{
    public class Note
    {
        private static readonly Regex _noteRegex = new Regex(@"^(?<name>[A-G]#?)(?<octave>\d+)?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static IReadOnlyCollection<string> _notes = new[] 
        { 
            "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" 
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
            :this(octaveIndex * _notes.Count + noteIndex)
        {            
        }

        public int Index { get; }

        public int NoteIndex { get; } 

        public string Name { get; }

        public int OctaveIndex { get; }

        public static Note FromName(string name)
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

        public static int GetNoteIndex(int index)
        {
            return index % _notes.Count;
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