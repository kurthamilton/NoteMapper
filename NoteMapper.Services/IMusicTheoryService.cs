using NoteMapper.Core.MusicTheory;

namespace NoteMapper.Services
{
    public interface IMusicTheoryService
    {
        IReadOnlyCollection<string> GetKeyNames(AccidentalType accidental);        

        IReadOnlyCollection<string> GetScaleTypes();
    }
}
