using MechanicShop.Application.Common.Constants.Caching;

namespace MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler(
    ILogger<UpdateCustomerCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache
    )
    : IRequestHandler<UpdateCustomerCommand, Result<Updated>>
{
    private readonly ILogger<UpdateCustomerCommandHandler> _logger = logger;
    private readonly IAppDbContext _context = context;
    private readonly HybridCache _cache = cache;

    public async Task<Result<Updated>> Handle(UpdateCustomerCommand command, CancellationToken ct)
    {
        var customer = await _context.Customers
             .Include(rt => rt.Vehicles)
             .FirstOrDefaultAsync(rt => rt.Id == command.CustomerId, ct);

        if (customer is null)
        {
            _logger.LogWarning("Customer {CustomerId} not found for update.", command.CustomerId);

            return ApplicationErrors.CustomerNotFound;
        }

        bool IsEmailChanged = ! (string.Equals(command.Email, customer.Email, StringComparison.OrdinalIgnoreCase));

        if(IsEmailChanged)
        {
            bool IsEmailExists = await _context.Customers.AnyAsync(
                c => string.Equals(command.Email, c.Email));

            if(IsEmailExists)
            {
                _logger.LogWarning("Customer update aborted. Email already exists.");

                return CustomerErrors.CustomerExists;
            }

        }



        var validatedVehicles = new List<Vehicle>();

        foreach (var v in command.Vehicles)
        {
            var vehicleId = v.VehicleId ?? Guid.NewGuid();

            var vehicleResult = Vehicle.Create(vehicleId, v.Make, v.Model, v.Year, v.LicensePlate);

            if (vehicleResult.IsError)
            {
                return vehicleResult.Errors;
            }

            validatedVehicles.Add(vehicleResult.Value);
        }

        var updateCustomerResult = customer.Update(command.Name, command.Email, command.PhoneNumber);

        if (updateCustomerResult.IsError)
        {
            return updateCustomerResult.Errors;
        }

        var upsertVehiclesResult = customer.UpsertVehicles(validatedVehicles);

        if (upsertVehiclesResult.IsError)
        {
            return upsertVehiclesResult.Errors;
        }

        await _context.SaveChangesAsync(ct);

        await _cache.RemoveByTagAsync(CustomersCachingConstants.CustomerCachTag, ct);

        return Result.Updated;
    }
}