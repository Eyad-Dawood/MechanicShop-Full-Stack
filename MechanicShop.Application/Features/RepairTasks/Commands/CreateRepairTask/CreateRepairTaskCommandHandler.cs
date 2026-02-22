
namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public class CreateRepairTaskCommandHandler(
    ILogger<CreateRepairTaskCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache
    )
    : IRequestHandler<CreateRepairTaskCommand, Result<RepairTaskDto>>
{
    private readonly ILogger<CreateRepairTaskCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<RepairTaskDto>> Handle(CreateRepairTaskCommand command, CancellationToken ct)
    {
        var nameExists = await _context.RepairTasks
           .AnyAsync(p => string.Equals(p.Name, command.Name), ct);

        if (nameExists)
        {
            _logger.LogWarning("Duplicate part name '{PartName}'.", command.Name);

            return RepairTaskErrors.DuplicateName;
        }

        List<Part> parts = [];

        foreach (var p in command.Parts)
        {
            var partResult = Part.Create(Guid.NewGuid(), p.Name, p.Cost, p.Quantity);

            if (partResult.IsError)
            {
                return partResult.Errors;
            }

            parts.Add(partResult.Value);
        }

        var createRepairTaskResult = RepairTask.Create(
                    id: Guid.NewGuid(),
                    name: command.Name!,
                    laborCost: command.LaborCost,
                    estimatedDurationInMins: command.EstimatedDurationInMins!.Value,
                    parts: parts);

        if (createRepairTaskResult.IsError)
        {
            return createRepairTaskResult.Errors;
        }

        var repairTask = createRepairTaskResult.Value;

        _context.RepairTasks.Add(repairTask);

        await _context.SaveChangesAsync(ct);

        await _cache.RemoveByTagAsync(RepairTaskCachingConstants.repairTaskCachTag, ct);

        return repairTask.ToDto();
    }
}