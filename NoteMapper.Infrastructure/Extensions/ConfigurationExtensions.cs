using Microsoft.Extensions.Configuration;

namespace NoteMapper.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
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
