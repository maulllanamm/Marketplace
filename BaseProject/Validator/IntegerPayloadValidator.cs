using FluentValidation;

namespace WebAPI.Validator
{
    public class IntegerPayloadValidator : AbstractValidator<int>
    {
        public IntegerPayloadValidator()
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("The payload must not be empty.")
                .GreaterThan(0).WithMessage("The payload must be greater than 0.");
        }
    }
}
