namespace MechanicShop.Application.Common.Constants.Caching
{
    static public class CustomersCachingConstants
    {

        public const string CustomerCachTag = "customer";
        public const string CustomersCachKey = "customer";
        public static string CustomerCachKey(Guid Id) => $"{CustomerCachTag}_{Id}";

        public const int CustomerCachingDurationInMinutes = 10; 
        public static TimeSpan CustomerCachingExpiration => TimeSpan.FromMinutes(CustomerCachingDurationInMinutes);

    }
}
