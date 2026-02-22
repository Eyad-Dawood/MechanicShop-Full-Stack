namespace MechanicShop.Application.Features.RepairTasks.Mappers;

public static class RepairTaskPartsMapper
{
    public static PartDto ToDto(this Part entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new PartDto
        {
            PartId = entity.Id,
            Name = entity.Name!,
            Cost = entity.Cost,
            Quantity = entity.Quantity
        };
    }

    public static List<PartDto> ToDtos(this IEnumerable<Part> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}