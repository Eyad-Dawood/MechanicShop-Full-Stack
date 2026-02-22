namespace MechanicShop.Application.Features.WorkOrders.Commands.AssignLabor;

public sealed record AssignLaborCommand(Guid WorkOrderId, Guid LaborId) : IRequest<Result<Updated>>;