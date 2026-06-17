using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Tasks
{

    public class SaveTasksCommandValidator : AbstractValidator<SaveTasksCommand>
    {
        public SaveTasksCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID is required");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

            RuleFor(x => x.AssignedTo)
                .GreaterThan(0).WithMessage("Assigned To is required");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

            RuleFor(x => x.Priority)
                .NotEmpty().WithMessage("Priority is required");
        }
    }
}
