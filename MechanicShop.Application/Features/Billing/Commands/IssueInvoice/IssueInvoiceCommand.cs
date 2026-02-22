namespace MechanicShop.Application.Features.Billing.Commands.IssueInvoice;

public sealed record IssueInvoiceCommand(Guid WorkOrderId) : IRequest<Result<InvoiceDto>>;