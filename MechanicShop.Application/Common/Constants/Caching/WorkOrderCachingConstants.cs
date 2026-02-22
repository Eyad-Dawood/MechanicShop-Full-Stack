namespace MechanicShop.Application.Common.Constants.Caching
{
    static public class WorkOrderCachingConstants
    {

        public const string workOrderCachTag = "work-order";
        public const string workOrdersCachKey = "work-order";
        public static string workOrderCachKey(Guid Id) => $"work-order:{Id}";

        public const int workOrderCachingDurationInMinutes = 10; 
        public static TimeSpan workOrderCachingExpiration => TimeSpan.FromMinutes(workOrderCachingDurationInMinutes);

    }
}
