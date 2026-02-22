namespace MechanicShop.Application.Features.Customers.Queries.GetCustomers;

public class GetCustomersQueryHandler(IAppDbContext context
    )
    : IRequestHandler<GetCustomersQuery, Result<List<CustomerDto>>>
{
    private readonly IAppDbContext _context = context;

    public async Task<Result<List<CustomerDto>>> Handle(GetCustomersQuery query, CancellationToken ct)
    {
        var customers = await _context.Customers.Include(c => c.Vehicles).AsNoTracking().ToListAsync(ct);

        return customers.ToDtos();
    }
}