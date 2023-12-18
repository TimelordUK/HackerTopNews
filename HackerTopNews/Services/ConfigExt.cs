namespace HackerTopNews.Services
{
    public static class ConfigExt
    {
        public static int GetAsInt32(this IConfiguration configuration, string key, int def)
        {
            var v = configuration[key] ?? $"{def}";
            var s = int.TryParse(v, out var expireSecs) ? expireSecs : def;
            return s;
        }

        public static string GetAsString(this IConfiguration configuration, string key, string def)
        {
            var v = configuration[key] ?? def;
            return v;
        }
    }
}
