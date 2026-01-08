using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.ProjectMembers
{

    public class SaveProjectMembersCommandValidator : AbstractValidator<SaveProjectMembersCommand>
    {
        public SaveProjectMembersCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.ProjectId)
                .GreaterThan(0).WithMessage("Project ID is required");

            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID is required");

            RuleFor(x => x.RoleInProject)
                .NotEmpty().WithMessage("Role in project is required");
        }
    }
}
