using NoteMapper.Core.Guitars;
using NoteMapper.Services.Web.ViewModels.NoteMap;

namespace NoteMapper.Services.Web
{
    public interface INoteMapViewModelService
    {
        NoteMapCriteriaViewModel GetNoteMapCriteriaViewModel(NoteMapCriteriaOptionsViewModel? options, 
            string instrument, string key, string mode);

        Task<NoteMapCriteriaOptionsViewModel> GetNoteMapCriteriaViewModelAsync(Guid? userId);

        NoteMapViewModel? GetNoteMapPermutationsViewModel(GuitarBase? instrument, NoteMapOptionsViewModel options);
    }
}
