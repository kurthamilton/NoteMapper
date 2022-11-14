using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapPositionViewModel
    {
        private readonly HashSet<string> _availableModifiers = new();
        private readonly List<NoteMapNotesViewModel> _permutations = new();

        public NoteMapPositionViewModel(int position)
        {
            Position = position;
        }

        public IReadOnlyCollection<string> AvailableModifiers => _availableModifiers;

        public IReadOnlyCollection<NoteMapNotesViewModel> Permutations => _permutations;

        public int Position { get; }        

        public NoteMapNotesViewModel? SelectedPermutation { get; private set; }

        public void AddPermutation(NoteMapNotesViewModel permutation)
        {
            _permutations.Add(permutation);

            foreach (InstrumentStringNote? note in permutation.Notes)
            {
                if (note?.Modifier == null || _availableModifiers.Contains(note.Modifier.Name))
                {
                    continue;
                }

                _availableModifiers.Add(note.Modifier.Name);
            }

            if (SelectedPermutation == null)
            {
                SelectedPermutation = permutation;
            }
        }

        public void SetSelectedPermutation(NoteMapNotesViewModel permutation)
        {
            if (Permutations.Contains(permutation))
            {
                SelectedPermutation = permutation;
            }
        }
    }
}
