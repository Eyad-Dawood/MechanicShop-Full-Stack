namespace MechanicShop.Application.Features.RepairTasks.Commands.UpdateRepairTask;

public class UpdateRepairTaskPartCommandValidator : AbstractValidator<UpdateRepairTaskPartCommand>
{
    public UpdateRepairTaskPartCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Part name is required.")
            .MaximumLength(100);

        RuleFor(x => x.Cost)
            .InclusiveBetween(1, 10_000)
            .WithMessage("Cost must be between 1 and 10,000.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be between more than 0");
    }
}