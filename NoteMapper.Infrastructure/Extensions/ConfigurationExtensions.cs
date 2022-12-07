using Microsoft.Extensions.Configuration;

namespace NoteMapper.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetEnum<T>(this IConfiguration config, string key) where T : struct
        {
            return Enum.TryParse(config[key], true, out T value)
                ? value 
                : default;
        }

        public static int GetInt(this IConfiguration config, string key)
        {
            int.TryParse(config[key], out int intValue);
            return intValue;
        }

        public static string GetValue(this IConfiguration config, string key)
        {
            return config[key] ?? "";
        }
    }
}
