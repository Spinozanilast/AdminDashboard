using AdminDashboard.Contracts.Clients;
using FluentValidation;

namespace AdminDashboard.Application.Clients.Validators;


public class CreateClientCommandValidator : AbstractValidator<CreateClientRequest>
{
    public CreateClientCommandValidator()
    {
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