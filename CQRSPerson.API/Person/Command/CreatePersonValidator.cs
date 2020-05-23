using FluentValidation;
using CQRSPerson.Domain.Constants;

namespace CQRSPerson.API.Person.Command
{
    public class CreatePersonValidator : AbstractValidator<CreatePersonCommand>
    {
        public CreatePersonValidator()
        {
            RuleFor(createPersonCommand => createPersonCommand.FirstName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.FirstNameInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.FirstName, ValidationErrorMessages.CannotBeNullEmptyOrWhiteSpace))
                .MaximumLength(ValidationProperties.FirstName)
                .WithErrorCode(ErrorCodes.FirstNameInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.FirstName, $"{ValidationErrorMessages.MaximumCharacterLimit}{ValidationProperties.FirstName}"));

            RuleFor(createPersonCommand => createPersonCommand.LastName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.LastNameInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.LastName, ValidationErrorMessages.CannotBeNullEmptyOrWhiteSpace))
                .MaximumLength(ValidationProperties.LastName)
                .WithErrorCode(ErrorCodes.LastNameInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.LastName, $"{ValidationErrorMessages.MaximumCharacterLimit}{ValidationProperties.LastName}"));

            RuleFor(createPersonCommand => createPersonCommand.Age)
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(ErrorCodes.AgeInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.Age.ToString(), ValidationErrorMessages.InvalidInteger));

            RuleFor(createPersonCommand => createPersonCommand.Interests)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.InterestsInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.Interests, ValidationErrorMessages.CannotBeNullEmptyOrWhiteSpace))
                .MaximumLength(ValidationProperties.Interests)
                .WithErrorCode(ErrorCodes.InterestsInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.Interests, $"{ValidationErrorMessages.MaximumCharacterLimit}{ValidationProperties.Interests}"));

            RuleFor(createPersonCommand => createPersonCommand.Image)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(ErrorCodes.ImageInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.Image, ValidationErrorMessages.CannotBeNullEmptyOrWhiteSpace))
                .MaximumLength(ValidationProperties.Image)
                .WithErrorCode(ErrorCodes.ImageInvalid)
                .WithMessage(createPersonCommand => ValidationErrorMessages.PropertyErrorMessage("{PropertyName}", createPersonCommand.Image, $"{ValidationErrorMessages.MaximumCharacterLimit}{ValidationProperties.Image}"));

        }
    }
}
