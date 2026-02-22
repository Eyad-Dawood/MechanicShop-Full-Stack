namespace MechanicShop.Application.Features.RepairTasks.Commands.RemoveRepairTask;

public sealed record RemoveRepairTaskCommand(Guid RepairTaskId)
    : IRequest<Result<Deleted>>;
