using FluentValidation;
using TFA.Domain.Exceptions;

namespace TFA.Domain.UseCases.CreateForum
{
    internal class CreateForumCommandValidator : AbstractValidator<CreateForumCommand>
    {
        public CreateForumCommandValidator()
        {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
                .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong);
        }
    }
}