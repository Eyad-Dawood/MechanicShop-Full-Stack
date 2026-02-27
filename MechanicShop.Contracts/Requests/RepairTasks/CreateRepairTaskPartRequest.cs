namespace MechanicShop.Contracts.Requests.RepairTasks;

public class CreateRepairTaskPartRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal Cost { get; set; }

    public int Quantity { get; set; }
}