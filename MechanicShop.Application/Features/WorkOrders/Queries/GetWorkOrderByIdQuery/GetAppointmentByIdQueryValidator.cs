namespace MechanicShop.Application.Features.WorkOrders.Queries.GetWorkOrderByIdQuery;

public sealed class GetAppointmentByIdQueryValidator : AbstractValidator<GetWorkOrderByIdQuery>
{
    public GetAppointmentByIdQueryValidator()
    {
        RuleFor(request => request.WorkOrderId)
            .NotEmpty()
            .WithMessage("WorkOrderId is required.");
    }
}