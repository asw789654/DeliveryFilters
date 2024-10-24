using FluentValidation;

namespace Orders.Application.Handlers.Queries.GetOrders;

internal class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(e => e.Region).GreaterThan(0);
    }
}