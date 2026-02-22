namespace MechanicShop.Application.Features.WorkOrders.Commands.AssignLabor;

public sealed class AssignLaborCommandValidator : AbstractValidator<AssignLaborCommand>
{
    public AssignLaborCommandValidator()
    {
        RuleFor(x => x.WorkOrderId)
         .NotEmpty()
         .WithMessage("WorkOrderId is required.");

        RuleFor(x => x.LaborId)
           .NotEmpty()
           .WithMessage("LaborId is required.");
    }
}