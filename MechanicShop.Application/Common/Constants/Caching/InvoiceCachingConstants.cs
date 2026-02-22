namespace MechanicShop.Application.Common.Constants.Caching
{
    static public class InvoiceCachingConstants
    {
        public const string invoiceCachTag = "invoice";
        public const string invoicesCachKey = "invoice";
        public static string invoiceCachKey(Guid Id) => $"{invoiceCachTag}_{Id}";

        public const int invoiceCachingDurationInMinutes = 10;
        public static TimeSpan invoiceCachingExpiration => TimeSpan.FromMinutes(invoiceCachingDurationInMinutes);
    }
}
