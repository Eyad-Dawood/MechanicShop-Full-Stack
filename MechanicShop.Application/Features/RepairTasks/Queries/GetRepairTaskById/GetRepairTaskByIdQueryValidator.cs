namespace MechanicShop.Application.Features.RepairTasks.Queries.GetRepairTaskById;

public sealed class GetRepairTaskByIdQueryValidator : AbstractValidator<GetRepairTaskByIdQuery>
{
    public GetRepairTaskByIdQueryValidator()
    {
        RuleFor(request => request.RepairTaskId)
            .NotEmpty()
            .WithMessage("CustomerId is required.");
    }
}