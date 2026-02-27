namespace MechanicShop.Contracts.Requests.RepairTasks;

public class CreateRepairTaskRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal LaborCost { get; set; }

    public RepairDurationInMinutes? EstimatedDurationInMins { get; set; }

    public List<CreateRepairTaskPartRequest> Parts { get; set; } = [];
}