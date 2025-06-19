using FluentValidation;
using AdminDashboard.Contracts.Clients;

namespace AdminDashboard.Application.Clients.Validators;


public class UpdateClientCommandValidator : AbstractValidator<UpdateClientRequest>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Client ID is required");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
        
        RuleFor(x => x.BalanceT)
            .GreaterThanOrEqualTo(0).WithMessage("Balance must be positive or zero");
        
        RuleForEach(x => x.Tags)
            .MaximumLength(50).WithMessage("Tag must not exceed 50 characters");
    }
}