using NoteMapper.Core;
using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels
{
    public class NoteMapCriteriaViewModel
    {
        public NoteMapCriteriaViewModel(IEnumerable<InstrumentBase> instruments, IReadOnlyCollection<Key> keys)
        {
            Instruments = instruments
                .Select(x => x.Name)
                .ToArray();

            Keys = keys
                .Select(x => new KeyListItemViewModel(x.ShortName, x.Name))
                .ToArray();
        }

        public IReadOnlyCollection<string> Instruments { get; }

        public IReadOnlyCollection<KeyListItemViewModel> Keys { get; }
    }
}
