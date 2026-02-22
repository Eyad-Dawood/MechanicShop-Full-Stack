namespace MechanicShop.Application.Features.Identity.GenerateTokens;

public sealed class GenerateTokenQueryValidator : AbstractValidator<GenerateTokenQuery>
{
    public GenerateTokenQueryValidator()
    {
        RuleFor(request => request.Email)
            .NotNull().NotEmpty()
            .WithMessage("Email cannot be null or empty")
            .EmailAddress()
            .WithMessage("Invalid Email Address");
            
        RuleFor(request => request.Password)
            .NotNull().NotEmpty()
            .WithMessage("Password cannot be null or empty.");
    }
}