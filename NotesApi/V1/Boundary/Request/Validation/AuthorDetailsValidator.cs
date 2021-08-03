using FluentValidation;
using Hackney.Core.Validation;
using NotesApi.V1.Domain;

namespace NotesApi.V1.Boundary.Request.Validation
{
    public class AuthorDetailsValidator : AbstractValidator<AuthorDetails>
    {
        public AuthorDetailsValidator()
        {
            RuleFor(x => x.Email).NotXssString()
                                 .EmailAddress()
                                 .When(x => !string.IsNullOrEmpty(x.Email));
            RuleFor(x => x.FullName).NotXssString()
                                    .When(x => !string.IsNullOrEmpty(x.FullName));
        }
    }
}
