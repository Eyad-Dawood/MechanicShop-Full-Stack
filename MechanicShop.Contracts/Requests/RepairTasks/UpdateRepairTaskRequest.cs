namespace MechanicShop.Contracts.Requests.RepairTasks;

public class UpdateRepairTaskRequest
{
    public string Name { get; set; } = string.Empty;

    public decimal LaborCost { get; set; }

    public RepairDurationInMinutes EstimatedDurationInMins { get; set; }

    public List<UpdateRepairTaskPartRequest> Parts { get; set; } = [];
}