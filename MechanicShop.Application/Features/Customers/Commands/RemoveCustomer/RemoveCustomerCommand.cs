namespace MechanicShop.Application.Features.Customers.Commands.RemoveCustomer;

public sealed record RemoveCustomerCommand(Guid CustomerId)
    : IRequest<Result<Deleted>>;