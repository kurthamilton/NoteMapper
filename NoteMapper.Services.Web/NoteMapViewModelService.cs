using NoteMapper.Core;
using NoteMapper.Core.Instruments;
using NoteMapper.Services.Web.ViewModels;

namespace NoteMapper.Services.Web
{
    public class NoteMapViewModelService : INoteMapViewModelService
    {
        private readonly IInstrumentFactory _instrumentFactory;
        private readonly IMusicTheoryService _musicTheoryService;

        public NoteMapViewModelService(IInstrumentFactory instrumentFactory, IMusicTheoryService musicTheoryService)
        {
            _instrumentFactory = instrumentFactory;
            _musicTheoryService = musicTheoryService;
        }

        public NoteMapCriteriaViewModel GetNoteMapCriteriaViewModel()
        {
            IReadOnlyCollection<InstrumentBase> instruments = _instrumentFactory.GetInstruments();
            IReadOnlyCollection<Key> keys = _musicTheoryService.GetKeys();
            return new NoteMapCriteriaViewModel(instruments, keys);
        }

        public NoteMapInstrumentViewModel? GetNoteMapInstrumentViewModel(string instrumentName)
        {
            InstrumentBase? instrument = _instrumentFactory.GetInstrument(instrumentName);
            StringedInstrumentBase? stringedInstrument = instrument as StringedInstrumentBase;
            return stringedInstrument != null
                ? new NoteMapInstrumentViewModel(stringedInstrument)
                : null;
        }

        public NoteMapPermutationsViewModel? GetNoteMapPermutationsViewModel(StringedInstrumentBase? instrument, string keyName)
        {
            if (instrument == null)
            {
                return null;
            }

            Key? key = _musicTheoryService.GetKey(keyName);
            if (key == null)
            {
                return null;
            }

            IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> permutations = 
                instrument.GetPermutations(key.ShortName, 1);

            return new NoteMapPermutationsViewModel(permutations);
        }
    }
}
