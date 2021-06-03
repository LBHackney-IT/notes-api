using FluentValidation;
using Hackney.Core.Validation;
using System;

namespace NotesApi.V1.Boundary.Request.Validation
{
    public class CreateNoteRequestValidator : AbstractValidator<CreateNoteRequest>
    {
        public CreateNoteRequestValidator()
        {
            RuleFor(x => x.Description).NotNull()
                                       .NotEmpty()
                                       .Length(1, 500)
                                       .NotXssString();
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
