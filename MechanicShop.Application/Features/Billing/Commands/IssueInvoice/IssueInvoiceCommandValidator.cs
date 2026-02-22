namespace MechanicShop.Application.Features.Billing.Commands.IssueInvoice;

public sealed class IssueInvoiceCommandValidator : AbstractValidator<IssueInvoiceCommand>
{
    public IssueInvoiceCommandValidator()
    {
        RuleFor(request => request.WorkOrderId)
            .NotEmpty()
            .WithMessage("WorkOrderId is required.");
    }
}