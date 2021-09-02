using FluentValidation;
using Hackney.Core.Validation;
using NotesApi.V2.Domain;

namespace NotesApi.V2.Boundary.Request.Validation
{
    public class AuthorDetailsValidator : AbstractValidator<AuthorDetails>
    {
        public AuthorDetailsValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithErrorCode(ErrorCodes.InvalidEmail)
                .When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.Email)
                .NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.FullName).NotXssString()
                .WithErrorCode(ErrorCodes.XssCheckFailure)
                .When(x => !string.IsNullOrEmpty(x.FullName));
        }
    }
}
