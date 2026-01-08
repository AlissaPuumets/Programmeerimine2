using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Employees
{

    public class SaveEmployeesCommandValidator : AbstractValidator<SaveEmployeesCommand>
    {
        public SaveEmployeesCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(50).WithMessage("Email cannot exceed 50 characters")
                .EmailAddress().WithMessage("Email format is invalid");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required")
                .MaximumLength(15).WithMessage("Phone cannot exceed 15 characters");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required");
        }
    }
}
