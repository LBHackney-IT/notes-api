using FluentValidation;
using Hackney.Core.Validation;
using NotesApi.V2.Domain;

namespace NotesApi.V2.Boundary.Request.Validation
{
    public class CategorisationValidator : AbstractValidator<Categorisation>
    {
        public CategorisationValidator()
        {
            RuleFor(x => x.Category)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(x => !string.IsNullOrEmpty(x.Category));

            RuleFor(x => x.SubCategory)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(x => !string.IsNullOrEmpty(x.SubCategory));

            RuleFor(x => x.Description)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
