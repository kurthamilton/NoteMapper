using NoteMapper.Core.Extensions;
using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;
using NoteMapper.Core.NoteMap;
using NoteMapper.Data.Core.Instruments;
using NoteMapper.Services.Users;
using NoteMapper.Services.Web.ViewModels.NoteMap;

namespace NoteMapper.Services.Web
{
    public class NoteMapViewModelService : INoteMapViewModelService
    {
        private readonly IInstrumentFactory _instrumentFactory;
        private readonly IMusicTheoryService _musicTheoryService;
        private readonly IUserInstrumentRepository _userInstrumentRepository;
        private readonly IUserService _userService;

        public NoteMapViewModelService(IInstrumentFactory instrumentFactory, IMusicTheoryService musicTheoryService,
            IUserInstrumentRepository userInstrumentRepository, IUserService userService)
        {
            _instrumentFactory = instrumentFactory;
            _musicTheoryService = musicTheoryService;
            _userInstrumentRepository = userInstrumentRepository;
            _userService = userService;
        }

        public NoteMapCriteriaViewModel GetNoteMapCriteriaViewModel(UserPreferences preferences, 
            NoteMapCriteriaOptionsViewModel options,
            string instrument, string note, string key, string mode, string intervals, string flats)
        {
            if (string.IsNullOrEmpty(instrument))
            {
                instrument = options?.DefaultInstruments.FirstOrDefault()?.Id ?? "";
            }

            int.TryParse(note, out int noteIndex);

            Scale? keyScale = Scale.TryParse(noteIndex, key);
            
            if (!Enum.TryParse(mode, true, out NoteMapMode parsedMode))
            {
                parsedMode = NoteMapCriteriaViewModel.DefaultMode;
            }

            if (!bool.TryParse(flats, out bool parsedFlats))
            {
                parsedFlats = preferences.Accidental == AccidentalType.Flat;
            }

            if (!bool.TryParse(intervals, out bool parsedIntervals))
            {
                parsedIntervals = preferences.Intervals;
            }

            return new NoteMapCriteriaViewModel
            {
                Accidental = parsedFlats ? AccidentalType.Flat : AccidentalType.Sharp,
                InstrumentId = instrument,
                Mode = parsedMode,
                NoteIndex = noteIndex,
                ScaleType = keyScale?.Type.ShortName(),
                ShowIntervals = parsedIntervals,
                Type = NoteCollectionType.Chord
            };
        }

        public async Task<NoteMapCriteriaOptionsViewModel> GetNoteMapCriteriaOptionsViewModelAsync(Guid? userId)
        {
            IReadOnlyCollection<UserInstrument> defaultInstruments = await _userInstrumentRepository.GetDefaultInstrumentsAsync();
            IReadOnlyCollection<UserInstrument> userInstruments = userId != null 
                ? await _userInstrumentRepository.GetUserInstrumentsAsync(userId.Value)
                : Array.Empty<UserInstrument>();

            IReadOnlyCollection<int> noteIndexes = _musicTheoryService.GetNoteIndexes();
            IReadOnlyCollection<string> keyTypes = _musicTheoryService.GetScaleTypes();
            return new NoteMapCriteriaOptionsViewModel(
                defaultInstruments.Select(_instrumentFactory.FromUserInstrument), 
                userInstruments.Select(_instrumentFactory.FromUserInstrument), 
                noteIndexes, 
                keyTypes);
        }

        public NoteMapViewModel? GetNoteMapPermutationsViewModel(GuitarBase? instrument, NoteMapOptionsViewModel options)
        {
            if (instrument == null)
            {
                return default;
            }

            INoteCollection notes = Note.GetNotes(options.NoteIndex, options.Key, options.Type);

            int frets = instrument.Strings.Max(x => x.Frets);

            NoteMapViewModel viewModel = new();

            for (int fret = 0; fret <= frets; fret++)
            {
                NoteMapFretViewModel fretViewModel = new(fret);

                int threshold = options.Type == NoteCollectionType.Chord
                    ? notes.Count
                    : 1;
                StringPermutationOptions permutationOptions = new(notes, fret, threshold);

                List<IReadOnlyCollection<GuitarStringNote?>> permutations = new();                

                if (options.Mode == NoteMapMode.Manual)
                {
                    IReadOnlyCollection<GuitarStringNote?> singlePermutation = instrument
                        .GetNotes(options.Modifiers.ToArray(), permutationOptions)
                        .ToArray();
                    if (singlePermutation.Count > 0)
                    {
                        permutations.Add(singlePermutation);
                    }                    
                }
                else if (options.Mode == NoteMapMode.Combinations)
                {
                    permutations.AddRange(instrument.GetPermutations(permutationOptions));
                }

                foreach (IReadOnlyCollection<GuitarStringNote?> permutation in permutations)
                {
                    NoteMapNotesViewModel permutationViewModel = new(permutation, notes.Key, options.Accidental);
                    fretViewModel.AddPermutation(permutationViewModel);
                }
                
                viewModel.AddFret(fretViewModel);
            }

            return viewModel;
        }
    }
}
