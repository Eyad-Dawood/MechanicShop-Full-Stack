
namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public sealed record CreateRepairTaskCommand(
    string? Name,
    decimal LaborCost,
    RepairDurationInMinutes? EstimatedDurationInMins,
    List<CreateRepairTaskPartCommand> Parts
) : IRequest<Result<RepairTaskDto>>;