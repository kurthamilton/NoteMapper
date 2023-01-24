using NoteMapper.Data.Core.Instruments;
using NoteMapper.Services.Web.ViewModels.Instruments;

namespace NoteMapper.Services.Web
{
    public interface IUserInstrumentViewModelService
    {
        Task<InstrumentEditViewModel?> GetInstrumentEditViewModelAsync(Guid userId, string userInstrumentId);

        InstrumentEditViewModel MapUserInstrumentToEditViewModel(UserInstrument userInstrument);

        void MapEditViewModelToUserInstrument(InstrumentEditViewModel viewModel, UserInstrument userInstrument);
    }
}
