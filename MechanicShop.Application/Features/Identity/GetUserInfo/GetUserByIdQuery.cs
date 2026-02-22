namespace MechanicShop.Application.Features.Identity.GetUserInfo;

public sealed record GetUserByIdQuery(string? UserId) : IRequest<Result<AppUserDto>>;