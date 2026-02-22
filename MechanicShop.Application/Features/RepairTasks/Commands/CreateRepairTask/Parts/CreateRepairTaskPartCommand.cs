namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask.Parts;

public sealed record CreateRepairTaskPartCommand(
    string Name,
    decimal Cost,
    int Quantity
) : IRequest<Result<Success>>;