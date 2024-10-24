using FluentValidation;

namespace Orders.Application.Handlers.Commands.CreateOrder;

internal class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(e => e.RegionId).NotEmpty().GreaterThan(0);
        RuleFor(e => e.Weight).NotEmpty().GreaterThan(0);
        RuleFor(e => e.DeliveringDateTime).NotEmpty().GreaterThan(DateTime.UtcNow);       
    }
}