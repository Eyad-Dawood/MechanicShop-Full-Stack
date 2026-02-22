namespace MechanicShop.Application.Features.WorkOrders.Commands.RelocateWorkOrder;

public sealed record RelocateWorkOrderCommand(
    Guid WorkOrderId,
    DateTimeOffset NewStartAt,
    Spot NewSpot) : IRequest<Result<Updated>>;