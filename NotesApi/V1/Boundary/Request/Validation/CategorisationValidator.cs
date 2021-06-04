using FluentValidation;
using Hackney.Core.Validation;
using NotesApi.V1.Domain;

namespace NotesApi.V1.Boundary.Request.Validation
{
    public class CategorisationValidator : AbstractValidator<Categorisation>
    {
        public CategorisationValidator()
        {
            RuleFor(x => x.Category).NotXssString();
            RuleFor(x => x.SubCategory).NotXssString();
            RuleFor(x => x.Description).NotXssString();
        }
    }
}
