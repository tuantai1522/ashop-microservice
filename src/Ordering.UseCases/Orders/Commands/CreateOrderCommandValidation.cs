using FluentValidation;

namespace Ordering.UseCases.Orders.Commands;

public class CreateOrderCommandValidation : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidation()
    {
        RuleFor(v => v.CustomerId)
            .NotEmpty()
            .WithMessage("'{CustomerId}' can't be empty.");
        
        RuleFor(v => v.Street)
            .NotEmpty()
            .WithMessage("'{Street}' can't be empty.");
        
        RuleFor(v => v.City)
            .NotEmpty()
            .WithMessage("'{City}' can't be empty.");
        
        RuleFor(v => v.Country)
            .NotEmpty()
            .WithMessage("'{Country}' can't be empty.");
        
        RuleFor(v => v.CardName)
            .NotEmpty()
            .WithMessage("'{CardName}' cant't be empty.");
        
        RuleFor(v => v.CardNumber)
            .NotEmpty()
            .WithMessage("'{CardNumber}' cant't be empty.");
        
        RuleFor(v => v.CVV)
            .NotEmpty()
            .WithMessage("'{CVV}' cant't be empty.");
        
    }
}