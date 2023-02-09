using NoteMapper.Core.MusicTheory;
using NoteMapper.Data.Core.Users;

namespace NoteMapper.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserPreferenceRepository _userPreferenceRepository;

        public UserService(IUserPreferenceRepository userPreferenceRepository)
        {
            _userPreferenceRepository = userPreferenceRepository;
        }

        public async Task<UserPreferences> GetPreferences(Guid? userId)
        {
            IReadOnlyCollection<UserPreference> userPreferences = userId != null 
                ? await _userPreferenceRepository.GetAsync(userId.Value)
                : Array.Empty<UserPreference>();
            IDictionary<UserPreferenceType, string> userPreferenceDictionary = userPreferences
                .ToDictionary(x => x.Type, x => x.Value);

            return new UserPreferences
            {
                Accidental = GetPreferenceValue(userPreferenceDictionary, UserPreferenceType.Accidental, 
                    Enum.Parse<AccidentalType>, AccidentalType.Sharp)
            };
        }

        private T? GetPreferenceValue<T>(IDictionary<UserPreferenceType, string> userPreferences, UserPreferenceType type, 
            Func<string, T> map, T @default)
        {
            if (!userPreferences.ContainsKey(type))
            {
                return @default;
            }

            string value = userPreferences[type];
            return map(value);
        }
    }
}
