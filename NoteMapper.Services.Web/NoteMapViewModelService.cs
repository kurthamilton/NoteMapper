using NoteMapper.Core.Instruments;
using NoteMapper.Core.MusicTheory;
using NoteMapper.Data.Core.Instruments;
using NoteMapper.Services.Web.ViewModels.Instruments;
using NoteMapper.Services.Web.ViewModels.NoteMap;

namespace NoteMapper.Services.Web
{
    public class NoteMapViewModelService : INoteMapViewModelService
    {
        private readonly IInstrumentFactory _instrumentFactory;
        private readonly IMusicTheoryService _musicTheoryService;
        private readonly IUserInstrumentRepository _userInstrumentRepository;

        public NoteMapViewModelService(IInstrumentFactory instrumentFactory, IMusicTheoryService musicTheoryService,
            IUserInstrumentRepository userInstrumentRepository)
        {
            _instrumentFactory = instrumentFactory;
            _musicTheoryService = musicTheoryService;
            _userInstrumentRepository = userInstrumentRepository;
        }

        public async Task<NoteMapCriteriaOptionsViewModel> GetNoteMapCriteriaViewModelAsync(Guid? userId)
        {
            IReadOnlyCollection<UserInstrument> defaultInstruments = await _userInstrumentRepository.GetDefaultInstrumentsAsync();
            IReadOnlyCollection<UserInstrument> userInstruments = userId != null ?
                await _userInstrumentRepository.GetUserInstrumentsAsync(userId.Value)
                : Array.Empty<UserInstrument>();

            IReadOnlyCollection<string> keyNames = _musicTheoryService.GetKeyNames();
            IReadOnlyCollection<string> keyTypes = _musicTheoryService.GetKeyTypes();
            return new NoteMapCriteriaOptionsViewModel(
                defaultInstruments.Select(x => _instrumentFactory.FromUserInstrument(x)), 
                userInstruments.Select(x => _instrumentFactory.FromUserInstrument(x)), 
                keyNames, 
                keyTypes);
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
