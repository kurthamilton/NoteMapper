using NoteMapper.Core;
using NoteMapper.Core.Instruments;
using NoteMapper.Services.Web.ViewModels;

namespace NoteMapper.Services.Web
{
    public interface INoteMapViewModelService
    {
        NoteMapCriteriaViewModel GetNoteMapCriteriaViewModel();

        NoteMapInstrumentViewModel? GetNoteMapInstrumentViewModel(string instrument);

        NoteMapPermutationsViewModel? GetNoteMapPermutationsViewModel(StringedInstrumentBase? instrument, string key, 
            NoteMapType type);
    }
}
