﻿using NoteMapper.Core.Instruments;
using NoteMapper.Core.Instruments.Implementations;
using NoteMapper.Data.Core.Instruments;

namespace NoteMapper.Services
{
    public class InstrumentFactory : IInstrumentFactory
    {
        public InstrumentBase FromUserInstrument(UserInstrument userInstrument)
        {
            InstrumentType type = Enum.Parse<InstrumentType>(userInstrument.Type, true);
            
            switch (type)
            {
                case InstrumentType.PedalSteelGuitar:
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
                        strings.Add(PedalSteelGuitarConfig.GetStringConfig(@string.Note, userInstrument.Frets));
                    }

                    PedalSteelGuitarConfig config = new PedalSteelGuitarConfig
                    {
                        Modifiers = modifiers,
                        MutuallyExclusiveModifiers = mutuallyExclusiveModifiers,
                        Strings = strings
                    };
                    return PedalSteelGuitar.Custom(userInstrument.UserInstrumentId, userInstrument.Name, config);
                default:
                    throw new NotImplementedException();
            }
        }

        public UserInstrument ToUserInstrument(StringedInstrumentBase instrument)
        {
            return new UserInstrument
            {
                Frets = 12,
                Modifiers = instrument.Modifiers.Select(x =>
                {
                    IEnumerable<ModifierOffset> offsets = instrument.Modifiers
                        .SelectMany((modifier) =>
                        {
                            return instrument.Strings
                                .Where((s, i) => modifier.IsFor(i))
                                .Select((s, i) => new ModifierOffset
                                {
                                    Offset = modifier.GetOffset(i),
                                    String = i
                                });

                        });
                    return new UserInstrumentModifier
                    {
                        Name = x.Name,
                        Offsets = offsets.ToArray()                        
                    };
                }).ToArray(),
                Name = instrument.Name, 
                Type = instrument.Type.ToString(),
                UserInstrumentId = Guid.NewGuid().ToString()
            };
        }
    }
}
