namespace MechanicShop.Application.Features.RepairTasks.Commands.UpdateRepairTask;

public class UpdateRepairTaskCommandHandler(
    ILogger<UpdateRepairTaskCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache
    )
    : IRequestHandler<UpdateRepairTaskCommand, Result<Updated>>
{
    private readonly ILogger<UpdateRepairTaskCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<Updated>> Handle(UpdateRepairTaskCommand command, CancellationToken ct)
    {
        var repairTask = await _context.RepairTasks
            .Include(rt => rt.Parts)
            .FirstOrDefaultAsync(rt => rt.Id == command.RepairTaskId, ct);

        if (repairTask is null)
        {
            _logger.LogWarning("RepairTask {RepairTaskId} not found for update.", command.RepairTaskId);

            return ApplicationErrors.RepairTaskNotFound;
        }

        var validatedParts = new List<Part>();

        foreach (var p in command.Parts)
        {
            var partId = p.PartId ?? Guid.NewGuid();

            var partResult = Part.Create(partId, p.Name, p.Cost, p.Quantity);

            if (partResult.IsError)
            {
                return partResult.Errors;
            }

            validatedParts.Add(partResult.Value);
        }

        var updateRepairTaskResult = repairTask.Update(command.Name, command.LaborCost, command.EstimatedDurationInMins);

        if (updateRepairTaskResult.IsError)
        {
            return updateRepairTaskResult.Errors;
        }

        var upsertPartsResult = repairTask.UpsertParts(validatedParts);

        if (upsertPartsResult.IsError)
        {
            return upsertPartsResult.Errors;
        }

        await _context.SaveChangesAsync(ct);

        await _cache.RemoveByTagAsync(RepairTaskCachingConstants.repairTaskCachTag, ct);

        return Result.Updated;
    }
}