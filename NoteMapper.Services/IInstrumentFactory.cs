using NoteMapper.Core.Instruments;
using NoteMapper.Data.Core.Instruments;

namespace NoteMapper.Services
{
    public interface IInstrumentFactory
    {
        InstrumentBase FromUserInstrument(UserInstrument userInstrument);

        UserInstrument ToUserInstrument(StringedInstrumentBase instrument);
    }
}
