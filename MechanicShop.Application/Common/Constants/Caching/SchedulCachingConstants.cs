namespace MechanicShop.Application.Common.Constants.Caching
{
    static public class SchedulCachingConstants
    {
        public const string workOrderCachTag = "work-order";
        public const string workOrdersCachKey = "work-order";
        public static string workOrderCachKey(DateOnly ScheduleDate,Guid laborId) => $"work-order:{ScheduleDate:yyyy-MM-dd}:laborId={laborId.ToString() ?? "-"}";

        public const int workOrderCachingDurationInMinutes = 10;
        public static TimeSpan workOrderCachingExpiration => TimeSpan.FromMinutes(workOrderCachingDurationInMinutes);
    }
}
