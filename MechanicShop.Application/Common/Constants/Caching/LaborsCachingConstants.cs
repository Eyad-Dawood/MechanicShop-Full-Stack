namespace MechanicShop.Application.Common.Constants.Caching
{
    static public class LaborsCachingConstants
    {
        public const string laborCachTag = "labor";
        public const string laborsCachKey = "labor";
        public static string laborCachKey(Guid Id) => $"{laborCachTag}_{Id}";

        public const int laborCachingDurationInMinutes = 10;
        public static TimeSpan laborCachingExpiration => TimeSpan.FromMinutes(laborCachingDurationInMinutes);
    }
}
