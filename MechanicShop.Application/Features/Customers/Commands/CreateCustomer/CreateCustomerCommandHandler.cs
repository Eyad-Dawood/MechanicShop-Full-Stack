using MechanicShop.Application.Common.Constants.Caching;

namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(
    ILogger<CreateCustomerCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache
    )
    : IRequestHandler<CreateCustomerCommand, Result<CustomerDto>>
{
    private readonly ILogger<CreateCustomerCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand command, CancellationToken ct)
    {
        var email = command.Email.Trim().ToLower();

        var exists = await _context.Customers.AnyAsync(
            c => string.Equals(email,c.Email),
            ct);
        
        if (exists)
        {
            _logger.LogWarning("Customer creation aborted. Email already exists.");

            return CustomerErrors.CustomerExists;
        }

        List<Vehicle> vehicles = [];

        foreach (var v in command.Vehicles)
        {
            var vehicleResult = Vehicle.Create(Guid.NewGuid(),v.Make, v.Model, v.Year, v.LicensePlate);

            if (vehicleResult.IsError)
            {
                return vehicleResult.Errors;
            }

            vehicles.Add(vehicleResult.Value);
        }

        var createCustomerResult = Customer.Create(
            Guid.NewGuid(),
            command.Name.Trim(),
            command.PhoneNumber.Trim(),
            command.Email.Trim(),
            vehicles);

        if (createCustomerResult.IsError)
        {
            return createCustomerResult.Errors;
        }

        _context.Customers.Add(createCustomerResult.Value);

        await _context.SaveChangesAsync(ct);

        await _cache.RemoveByTagAsync(CustomersCachingConstants.CustomerCachTag, ct);

        var customer = createCustomerResult.Value;

        _logger.LogInformation("Customer created successfully. Id: {CustomerId}", createCustomerResult.Value.Id);

        return customer.ToDto();
    }
}