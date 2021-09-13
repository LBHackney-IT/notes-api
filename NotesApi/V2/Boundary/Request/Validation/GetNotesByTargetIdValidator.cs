using System;
using FluentValidation;

namespace NotesApi.V2.Boundary.Request.Validation
{
    public class GetNotesByTargetIdValidator : AbstractValidator<GetNotesByTargetIdQuery>
    {
        public GetNotesByTargetIdValidator()
        {
            RuleFor(x => x.TargetId).NotNull()
                                    .NotEqual(Guid.Empty);
            RuleFor(x => x.PageSize).GreaterThan(0)
                                    .When(y => y.PageSize.HasValue);
        }
    }
}
