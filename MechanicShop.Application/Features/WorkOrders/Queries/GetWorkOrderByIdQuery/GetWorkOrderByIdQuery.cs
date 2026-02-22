namespace MechanicShop.Application.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

public sealed record GetWorkOrderByIdQuery(Guid WorkOrderId) : ICachedQuery<Result<WorkOrderDto>>
{
    public string CacheKey => WorkOrderCachingConstants.workOrderCachKey(WorkOrderId);
    public string[] Tags => [WorkOrderCachingConstants.workOrderCachTag];
    public TimeSpan Expiration => WorkOrderCachingConstants.workOrderCachingExpiration;
}