namespace MechanicShop.Application.Features.Billing.Queries.GetInvoiceById;

public sealed record GetInvoiceByIdQuery(Guid InvoiceId) : ICachedQuery<Result<InvoiceDto>>
{
    public string CacheKey => InvoiceCachingConstants.invoiceCachKey(InvoiceId);

    public TimeSpan Expiration => InvoiceCachingConstants.invoiceCachingExpiration;

    public string[] Tags => [InvoiceCachingConstants.invoiceCachTag];
}