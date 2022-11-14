using NoteMapper.Core;
using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapCriteriaViewModel
    {
        public NoteMapCriteriaViewModel(IEnumerable<InstrumentBase> instruments, IReadOnlyCollection<string> keyNames,
            IReadOnlyCollection<string> keyTypes)
        {
            Instruments = instruments
                .Select(x => x.Name)
                .ToArray();

            KeyNames = keyNames;
            KeyTypes = keyTypes;

            Types = Enum.GetValues<NoteMapType>()
                .Where(x => x != NoteMapType.None)
                .ToArray();
        }

        public IReadOnlyCollection<string> Instruments { get; }        

        public IReadOnlyCollection<string> KeyNames { get; }

        public IReadOnlyCollection<string> KeyTypes { get; }

        public IReadOnlyCollection<NoteMapType> Types { get; }
    }
}
