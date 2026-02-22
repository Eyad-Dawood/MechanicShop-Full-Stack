using FluentValidation;

namespace MechanicShop.Application.Features.Customers.Queries.GetCustomerById;

public sealed class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    public GetCustomerByIdQueryValidator()
    {
        RuleFor(request => request.CustomerId)
            .NotEmpty()
            .WithMessage("CustomerId is required.");
    }
}