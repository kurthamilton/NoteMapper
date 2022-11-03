using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels
{
    public class NoteMapPermutationsViewModel
    {
        public NoteMapPermutationsViewModel(IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> permutations)
        {
            Permutations = permutations;
        }

        public IReadOnlyCollection<IReadOnlyCollection<InstrumentStringNote>> Permutations { get; }
    }
}
