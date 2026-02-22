namespace MechanicShop.Application.Features.Billing.Queries.GetInvoicePdf;

public sealed class GetInvoicePdfQueryValidator : AbstractValidator<GetInvoicePdfQuery>
{
    public GetInvoicePdfQueryValidator()
    {
        RuleFor(request => request.InvoiceId)
            .NotEmpty()
            .WithMessage("InvoiceId is required.");
    }
}