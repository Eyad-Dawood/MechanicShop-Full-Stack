namespace MechanicShop.Application.Features.Billing.Queries.GetInvoicePdf;

public sealed record GetInvoicePdfQuery(Guid InvoiceId) : IRequest<Result<InvoicePdfDto>>
{
}