using NoteMapper.Core.Instruments;

namespace NoteMapper.Services.Web.ViewModels.NoteMap
{
    public class NoteMapViewModel
    {
        private readonly List<NoteMapPositionViewModel> _positions = new();

        public IReadOnlyCollection<NoteMapPositionViewModel> Positions => _positions;

        public void AddPosition(NoteMapPositionViewModel position)
        {
            _positions.Add(position);
        }
    }
}
