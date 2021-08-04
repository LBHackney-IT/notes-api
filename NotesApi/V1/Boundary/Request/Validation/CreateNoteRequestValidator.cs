using FluentValidation;
using Hackney.Core.Validation;
using System;

namespace NotesApi.V1.Boundary.Request.Validation
{
    public class CreateNoteRequestValidator : AbstractValidator<CreateNoteRequest>
    {
        //private const string RegExSpecialCharacters = @"(?i)^[a-z’'$£()/.,\s-]+$";

        public CreateNoteRequestValidator()
        {
            RuleFor(x => x.Description).NotNull()
                                       .NotEmpty()
                                       .WithErrorCode("W2");
            RuleFor(x => x.Description).Length(1, 500)
                                       .WithErrorCode("W3")
                                       .When(x => !string.IsNullOrEmpty(x.Description));
            // TODO - Re-enable when the list of special charaters is finalised
            //RuleFor(x => x.Description).Matches(x => RegExSpecialCharacters)
            //                           .WithErrorCode("W8")
            //                           .When(x => !string.IsNullOrEmpty(x.Description));
            RuleFor(x => x.Description).NotXssString()
                                       .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.TargetType).NotNull()
                                      .IsInEnum();
            RuleFor(x => x.TargetId).NotNull()
                                    .NotEqual(Guid.Empty);
            RuleFor(x => x.CreatedAt).NotNull()
                                     .NotEqual(default(DateTime));

            RuleFor(x => x.Author).SetValidator(new AuthorDetailsValidator());
            RuleFor(x => x.Categorisation).SetValidator(new CategorisationValidator());
        }
    }
}
