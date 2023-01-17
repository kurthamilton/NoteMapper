using NoteMapper.Core.Guitars;
using NoteMapper.Core.MusicTheory;
using NoteMapper.Services.Web.ViewModels.Instruments;
using NoteMapper.Services.Web.ViewModels.NoteMap;

namespace NoteMapper.Services.Web
{
    public interface INoteMapViewModelService
    {
        Task<NoteMapCriteriaOptionsViewModel> GetNoteMapCriteriaViewModelAsync(Guid? userId);

        NoteMapViewModel? GetNoteMapPermutationsViewModel(GuitarBase? instrument, string key, 
            NoteMapType type);
    }
}
