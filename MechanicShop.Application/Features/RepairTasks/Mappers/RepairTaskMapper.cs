namespace MechanicShop.Application.Features.RepairTasks.Mappers;

public static class RepairTaskMapper
{
    public static RepairTaskDto ToDto(this RepairTask entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new RepairTaskDto
        {
            RepairTaskId = entity.Id,
            Name = entity.Name!,
            LaborCost = entity.LaborCost,
            TotalCost = entity.TotalCost,
            EstimatedDurationInMins = entity.EstimatedDurationInMins,
            Parts = entity.Parts
            .Select(p => p.ToDto())
                .ToList()
        };
    }

    public static List<RepairTaskDto> ToDtos(this IEnumerable<RepairTask> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
