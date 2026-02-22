namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(
    string Name,
    string PhoneNumber,
    string Email,
    List<CreateVehicleCommand> Vehicles

) : IRequest<Result<CustomerDto>>;