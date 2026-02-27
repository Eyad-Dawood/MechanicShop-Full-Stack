namespace MechanicShop.Contracts.Requests.WorkOrders;

public class CreateWorkOrderRequest
{
    public Spot Spot { get; set; }

    public Guid VehicleId { get; set; }

    public List<Guid> RepairTaskIds { get; set; } = [];

    public Guid LaborId { get; set; }

    public DateTimeOffset StartAtUtc { get; set; }
}