namespace MechanicShop.Application.Features.Dashboard.Queries.GetWorkOrderStats
{
    internal class GetWorkOrderStatsQueryValidator : AbstractValidator<GetWorkOrderStatsQuery>
    {
        public GetWorkOrderStatsQueryValidator()
        {
            RuleFor(request => request.Date)
                .NotEmpty()
                .WithMessage("Date is required.");
        }
    }
}