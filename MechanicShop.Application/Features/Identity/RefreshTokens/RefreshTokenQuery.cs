namespace MechanicShop.Application.Features.Identity.RefreshTokens;

public record RefreshTokenQuery(string RefreshToken, string ExpiredAccessToken) : IRequest<Result<TokenResponse>>;