using System;
using FluentValidation;
using Hackney.Core.Validation;

namespace NotesApi.V2.Boundary.Request.Validation
{
    public class CreateNoteRequestValidator : AbstractValidator<CreateNoteRequest>
    {
        public CreateNoteRequestValidator()
        {
            RuleFor(x => x.Description).NotNull()
                                       .NotEmpty()
                                       .WithErrorCode(ErrorCodes.DescriptionMandatory);
            RuleFor(x => x.Description).Length(1, 500)
                                       .WithErrorCode(ErrorCodes.DescriptionTooLong)
                                       .When(x => !string.IsNullOrEmpty(x.Description));
            RuleFor(x => x.Description).NotXssString()
                                       .WithErrorCode(ErrorCodes.XssCheckFailure)
                                       .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.TargetType).NotNull()
                                      .IsInEnum();
            RuleFor(x => x.TargetId).NotNull()
                                    .NotEqual(Guid.Empty);

            RuleFor(x => x.CreatedAt).NotNull()
                                     .NotEqual(default(DateTime));

            RuleFor(x => x.Author).SetValidator(new AuthorDetailsValidator());
            RuleFor(x => x.Author).NotNull();

            RuleFor(x => x.Categorisation).SetValidator(new CategorisationValidator());
        }
    }
}
