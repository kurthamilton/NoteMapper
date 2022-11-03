using NoteMapper.Core;

namespace NoteMapper.Web.Api.Models.Instruments.Responses
{
    public class ResponseNote
    {
        public ResponseNote(Note note)
        {
            Name = note.Name;
            Octave = note.OctaveIndex;
        }

        public int Octave { get; }

        public string Name { get; }
    }
}
