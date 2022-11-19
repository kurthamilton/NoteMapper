using NoteMapper.Core;
using NoteMapper.Core.Instruments;
using NoteMapper.Services.Web.ViewModels.Instruments;
using NoteMapper.Services.Web.ViewModels.NoteMap;

namespace NoteMapper.Services.Web
{
    public interface INoteMapViewModelService
    {
        NoteMapCriteriaOptionsViewModel GetNoteMapCriteriaViewModel();

        InstrumentViewModel? GetNoteMapInstrumentViewModel(string instrument);

        NoteMapViewModel? GetNoteMapPermutationsViewModel(StringedInstrumentBase? instrument, string key, 
            NoteMapType type);
    }
}
