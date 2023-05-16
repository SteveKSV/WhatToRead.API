using EFTopics.BBL.Dtos;
using FluentValidation;

namespace EFWhatToRead_BBL.Helpers
{
    public class PostValidator : AbstractValidator<PostDto>
    {
        public PostValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("The Title field is required.");
            RuleFor(p => p.Body).Length(5, 500).WithMessage("The Body field must be between 5 and 500 characters.");
            RuleFor(p => p.Slug).Length(5, 50).WithMessage("The Slug field must be between 5 and 50 characters.");
        }
    }
}
