namespace MechanicShop.Contracts.Requests.RepairTasks;

public class UpdateRepairTaskPartRequest
{
    public Guid? PartId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Cost { get; set; }

    public int Quantity { get; set; }
}