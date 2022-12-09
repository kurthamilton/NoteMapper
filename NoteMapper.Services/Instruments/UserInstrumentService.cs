using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;
using NoteMapper.Data.Core.Instruments;

namespace NoteMapper.Services.Instruments
{
    public class UserInstrumentService : IUserInstrumentService
    {
        private readonly IInstrumentFactory _instrumentFactory;
        private readonly IUserInstrumentRepository _userInstrumentRepository;

        public UserInstrumentService(IUserInstrumentRepository userInstrumentRepository, 
            IInstrumentFactory instrumentFactory)
        {
            _instrumentFactory = instrumentFactory;
            _userInstrumentRepository = userInstrumentRepository;
        }

        public async Task<IReadOnlyCollection<InstrumentBase>> GetDefaultInstrumentsAsync()
        {
            IReadOnlyCollection<UserInstrument> instruments = await _userInstrumentRepository.GetDefaultInstrumentsAsync();
            return instruments
                .Select(MapUserInstrument)
                .ToArray();
        }

        public Task<IReadOnlyCollection<InstrumentBase>> GetUserInstrumentsAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        private InstrumentBase MapUserInstrument(UserInstrument userInstrument)
        {
            return _instrumentFactory.FromUserInstrument(userInstrument);
        }
    }
}
