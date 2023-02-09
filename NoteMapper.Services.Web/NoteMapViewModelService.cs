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

        public NoteMapCriteriaViewModel GetNoteMapCriteriaViewModel(NoteMapCriteriaOptionsViewModel? options,
            string instrument, string key, string mode, string intervals, string accidental)
        {
            if (string.IsNullOrEmpty(instrument))
            {
                instrument = options?.DefaultInstruments.FirstOrDefault()?.Id ?? "";
            }            

            Scale.TryParse(key, out Scale? keyScale);

            if (keyScale == null)
            {
                key = options?.KeyNames.FirstOrDefault() ?? "";
                Scale.TryParse(key, out keyScale);
            }

            if (!Enum.TryParse(mode, true, out NoteMapMode parsedMode))
            {
                parsedMode = NoteMapCriteriaViewModel.DefaultMode;
            }

            if (!Enum.TryParse(accidental, true, out AccidentalType parsedAccidental))
            {
                parsedAccidental = options?.Accidentals.First().Key ?? NoteMapCriteriaViewModel.DefaultAccidental;
            }

            bool.TryParse(intervals, out bool parsedIntervals);

            return new NoteMapCriteriaViewModel
            {
                Accidental = parsedAccidental,
                InstrumentId = instrument,
                KeyName = keyScale?.ElementAt(0).GetName(parsedAccidental),
                Mode = parsedMode,
                ScaleType = keyScale?.Type.ShortName(),
                ShowIntervals = parsedIntervals,
                Type = NoteMapType.Chord
            };
        }

        public async Task<NoteMapCriteriaOptionsViewModel> GetNoteMapCriteriaOptionsViewModelAsync(Guid? userId)
        {
            IReadOnlyCollection<UserInstrument> defaultInstruments = await _userInstrumentRepository.GetDefaultInstrumentsAsync();
            IReadOnlyCollection<UserInstrument> userInstruments = userId != null 
                ? await _userInstrumentRepository.GetUserInstrumentsAsync(userId.Value)
                : Array.Empty<UserInstrument>();

            UserPreferences userPreferences = await _userService.GetPreferences(userId);

            IReadOnlyCollection<string> keyNames = _musicTheoryService.GetKeyNames(userPreferences.Accidental);
            IReadOnlyCollection<string> keyTypes = _musicTheoryService.GetScaleTypes();
            return new NoteMapCriteriaOptionsViewModel(
                defaultInstruments.Select(_instrumentFactory.FromUserInstrument), 
                userInstruments.Select(_instrumentFactory.FromUserInstrument), 
                keyNames, 
                keyTypes);
        }

        public NoteMapViewModel? GetNoteMapPermutationsViewModel(GuitarBase? instrument, NoteMapOptionsViewModel options)
        {
            if (instrument == null)
            {
                return default;
            }

            INoteCollection notes = Note.GetNotes(options.Type, options.Key);

            int frets = instrument.Strings.Max(x => x.Frets);

            NoteMapViewModel viewModel = new();

            for (int fret = 0; fret <= frets; fret++)
            {
                NoteMapFretViewModel fretViewModel = new(fret);

                StringPermutationOptions permutationOptions = new(notes, fret);

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
                else if (options.Mode == NoteMapMode.Permutations)
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
