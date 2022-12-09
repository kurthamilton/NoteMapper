using NoteMapper.Core.Instruments;
using NoteMapper.Core.MusicTheory;
using NoteMapper.Services.Web.ViewModels.Instruments;
using NoteMapper.Services.Web.ViewModels.NoteMap;

namespace NoteMapper.Services.Web
{
    public interface INoteMapViewModelService
    {
        Task<NoteMapCriteriaOptionsViewModel> GetNoteMapCriteriaViewModelAsync(Guid? userId);

        InstrumentViewModel? GetNoteMapInstrumentViewModel(string instrument);

        NoteMapViewModel? GetNoteMapPermutationsViewModel(StringedInstrumentBase? instrument, string key, 
            NoteMapType type);
    }
}
