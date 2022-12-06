using NoteMapper.Core.Instruments;
using NoteMapper.Core.MusicTheory;
using NoteMapper.Services.Web.ViewModels.Instruments;
using NoteMapper.Services.Web.ViewModels.NoteMap;

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

        public NoteMapCriteriaOptionsViewModel GetNoteMapCriteriaViewModel()
        {
            IReadOnlyCollection<InstrumentBase> instruments = _instrumentFactory.GetInstruments();
            IReadOnlyCollection<string> keyNames = _musicTheoryService.GetKeyNames();
            IReadOnlyCollection<string> keyTypes = _musicTheoryService.GetKeyTypes();
            return new NoteMapCriteriaOptionsViewModel(instruments, keyNames, keyTypes);
        }

        public InstrumentViewModel? GetNoteMapInstrumentViewModel(string instrumentName)
        {
            InstrumentBase? instrument = _instrumentFactory.GetInstrument(instrumentName);
            StringedInstrumentBase? stringedInstrument = instrument as StringedInstrumentBase;
            return stringedInstrument != null
                ? new InstrumentViewModel(stringedInstrument)
                : null;
        }

        public NoteMapViewModel? GetNoteMapPermutationsViewModel(StringedInstrumentBase? instrument, string key,
            NoteMapType type)
        {
            if (instrument == null)
            {
                return default;
            }

            INoteCollection notes = Note.GetNotes(type, key);

            int positions = instrument.Strings.Max(x => x.Positions);

            NoteMapViewModel viewModel = new();

            for (int position = 0; position <= positions; position++)
            {
                NoteMapPositionViewModel positionViewModel = new(position);

                StringPermutationOptions options = new(notes, position);

                foreach (IReadOnlyCollection<InstrumentStringNote?> permutation in instrument.GetPermutations(options))
                {
                    NoteMapNotesViewModel permutationViewModel = new(permutation);
                    positionViewModel.AddPermutation(permutationViewModel);
                }
                
                viewModel.AddPosition(positionViewModel);
            }

            return viewModel;
        }
    }
}
