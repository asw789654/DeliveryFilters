using FluentValidation;

namespace Orders.Application.Handlers.Queries.GetOrders;

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(e => e.Region).GreaterThan(0);
        RuleFor(e => e.NumberOfOrders).GreaterThan(0);
    }
}