using FluentValidation;

namespace Core3.Application.Commands.Notes
{
    public class CreateNoteCommandValidator : AbstractValidator<CreateNoteCommand>
    {
        public CreateNoteCommandValidator()
        {
            RuleFor(x => x.Text).Must(text => !string.IsNullOrWhiteSpace(text)).WithMessage("Text is empty");
        }
    }
}
