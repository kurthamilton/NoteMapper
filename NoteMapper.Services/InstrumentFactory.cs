using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;
using NoteMapper.Data.Core.Instruments;

namespace NoteMapper.Services
{
    public class InstrumentFactory : IInstrumentFactory
    {
        public InstrumentBase FromUserInstrument(UserInstrument userInstrument)
        {
            switch (userInstrument.Type.ToLowerInvariant())
            {
                case "pedalsteelguitar":
                    List<string> modifiers = new();
                    List<KeyValuePair<string, string>> mutuallyExclusiveModifiers = new();
                    List<string> strings = new();

                    foreach (UserInstrumentModifier modifier in userInstrument.Modifiers)
                    {
                        List<int> offsets = new();
                        foreach (ModifierOffset offset in modifier.Offsets)
                        {
                            offsets.Add(offset.String);
                            offsets.Add(offset.Offset);
                        }

                        string modifierConfig = PedalSteelGuitarConfig.GetModifierConfig(modifier.Name, offsets.ToArray());
                        modifiers.Add(modifierConfig);

                        if (modifier.MutuallyExclusive != null)
                        {
                            foreach (string otherModifier in modifier.MutuallyExclusive)
                            {
                                mutuallyExclusiveModifiers.Add(new KeyValuePair<string, string>(modifier.Name, otherModifier));
                            }
                        }
                    }

                    foreach (UserInstrumentString @string in userInstrument.Strings)
                    {
                        strings.Add(PedalSteelGuitarConfig.GetStringConfig(@string.Note, userInstrument.Frets ?? 12));
                    }

                    PedalSteelGuitarConfig config = new PedalSteelGuitarConfig
                    {
                        Modifiers = modifiers,
                        MutuallyExclusiveModifiers = mutuallyExclusiveModifiers,
                        Strings = strings
                    };
                    return PedalSteelGuitar.Custom(userInstrument.Name, config);
                default:
                    throw new NotImplementedException();
            }
        }

        public InstrumentBase? GetInstrument(string? name)
        {
            return GetInstruments()
                .FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyCollection<InstrumentBase> GetInstruments()
        {
            return new InstrumentBase[]
            {
                PedalSteelGuitar.C6(),
                PedalSteelGuitar.E9()
            };
        }
    }
}
