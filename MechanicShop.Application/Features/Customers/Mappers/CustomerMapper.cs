namespace MechanicShop.Application.Features.Customers.Mappers;

public static partial class CustomerMapper
{
    public static CustomerDto ToDto(this Customer entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        return new CustomerDto
        {
            CustomerId = entity.Id,
            Name = entity.Name!,
            Email = entity.Email!,
            PhoneNumber = entity.PhoneNumber!,
            Vehicles = entity.Vehicles?.Select(v => v.ToDto()).ToList() ?? []
        };
    }

    public static List<CustomerDto> ToDtos(this IEnumerable<Customer> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}