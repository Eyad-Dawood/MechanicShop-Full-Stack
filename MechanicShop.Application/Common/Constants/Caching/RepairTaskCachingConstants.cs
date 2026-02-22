namespace MechanicShop.Application.Common.Constants.Caching
{
    static public class RepairTaskCachingConstants
    {
        public const string repairTaskCachTag = "repair-task";
        public const string repairTasksCachKey = "repair-task";
        public static string repairTaskCachKey(Guid Id) => $"{repairTaskCachTag}_{Id}";

        public const int repairTaskCachingDurationInMinutes = 10;
        public static TimeSpan repairTaskCachingExpiration => TimeSpan.FromMinutes(repairTaskCachingDurationInMinutes);

    }
}
