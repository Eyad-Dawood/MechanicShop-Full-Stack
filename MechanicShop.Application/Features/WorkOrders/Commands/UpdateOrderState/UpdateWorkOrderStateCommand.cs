namespace MechanicShop.Application.Features.WorkOrders.Commands.UpdateOrderState;

public sealed record UpdateWorkOrderStateCommand(
    Guid WorkOrderId,
    WorkOrderState State) : IRequest<Result<Updated>>;