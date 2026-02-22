namespace MechanicShop.Application.Features.Identity.GenerateTokens;

public record GenerateTokenQuery(
    string Email,
    string Password) : IRequest<Result<TokenResponse>>;