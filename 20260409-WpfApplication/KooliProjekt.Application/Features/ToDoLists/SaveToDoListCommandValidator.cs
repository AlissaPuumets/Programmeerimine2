using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.ToDoLists
{
    // 15.11.2025
    // Valideerimise klass SaveToDoListCommand käsu jaoks
    // Võetakse programmi poolt külge automaatselt
    public class SaveToDoListCommandValidator : AbstractValidator<SaveToDoListCommand>
    {
        public SaveToDoListCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters")
                // Oma loogikaga valideerimise reegel
                // Siin võib kasutada DbContexti klassi
                .Custom((s, context) =>
                {
                    // Command või query, mida valideerime
                    var command = context.InstanceToValidate;

                    // Oma valideerimise loogika
                    // koos vea lisamisega
                    //var failure = new ValidationFailure();
                    //failure.AttemptedValue = command.ProjectId;
                    //failure.ErrorMessage = "Cannot find project with id " + command.ProjectId;
                    //failure.PropertyName = nameof(command.ProjectId);

                    //context.AddFailure(failure);
                });
        }
    }
}
