using NoteMapper.Core.Guitars;
using NoteMapper.Services.Users;
using NoteMapper.Services.Web.ViewModels.NoteMap;

namespace NoteMapper.Services.Web
{
    public interface INoteMapViewModelService
    {
        NoteMapCriteriaViewModel GetNoteMapCriteriaViewModel(UserPreferences preferences, 
            NoteMapCriteriaOptionsViewModel options, 
            string instrument, string note, string key, string mode, string intervals, string flats);

        Task<NoteMapCriteriaOptionsViewModel> GetNoteMapCriteriaOptionsViewModelAsync(Guid? userId);

        NoteMapViewModel? GetNoteMapPermutationsViewModel(GuitarBase? instrument, NoteMapOptionsViewModel options);
    }
}
