using CQRSPerson.API.Person.Command;
using CQRSPerson.Domain.Constants;
using CQRSPerson.TestData;
using CQRSPerson.TestData.Constants;
using FluentAssertions;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace CQRSPerson.API.Tests.Persons.CreatePerson
{
    public class CreatePersonValidatorTests
    {
        private CreatePersonValidator _createPersonValidator;
        private CreatePersonCommand _createPersonCommand;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _createPersonValidator = new CreatePersonValidator();
        }

        [SetUp]
        public void Setup()
        {
            _createPersonCommand = new CreatePersonCommand()
            {
                FirstName = "Alex",
                LastName = "Hosein",
                Age = 29,
                Interests = "Software Development",
                Image = "ImageUrl"
            };
        }

        [Test]
        public void ValidCreatePersonCommandReturnsNoErrors()
        {
            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeTrue();
            results.Errors.Should().HaveCount(0);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" \r\n ")]
        public void NullOrEmptyFirstNameReturnsHydratedValidationResult(string firstName)
        {
            _createPersonCommand.FirstName = firstName;
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(ValidationTestConstants.FirstNameProperty, firstName, $"{ValidationTestConstants.FirstNameProperty}{ValidationTestConstants.CannotBeNullEmptyOrWhiteSpace}");
            var expectedErrorCode = ErrorCodes.FirstNameInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.FirstName)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" \r\n ")]
        public void NullOrEmptyLastNameReturnsHydratedValidationResult(string lastName)
        {
            _createPersonCommand.LastName = lastName;
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(ValidationTestConstants.LastNameProperty, lastName, $"{ValidationTestConstants.LastNameProperty}{ValidationTestConstants.CannotBeNullEmptyOrWhiteSpace}");
            var expectedErrorCode = ErrorCodes.LastNameInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.LastName)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" \r\n ")]
        public void NullOrEmptyInterestsReturnsHydratedValidationResult(string interests)
        {
            _createPersonCommand.Interests = interests;
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(ValidationTestConstants.InterestsProperty, interests, $"{ValidationTestConstants.InterestsProperty}{ValidationTestConstants.CannotBeNullEmptyOrWhiteSpace}");
            var expectedErrorCode = ErrorCodes.InterestsInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.Interests)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(" \r\n ")]
        public void NullOrEmptyImageReturnsHydratedValidationResult(string image)
        {
            _createPersonCommand.Image = image;
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(
                ValidationTestConstants.ImageProperty,
                image,
                $"{ValidationTestConstants.ImageProperty}{ValidationTestConstants.CannotBeNullEmptyOrWhiteSpace}");
            var expectedErrorCode = ErrorCodes.ImageInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.Image)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }

        [Test]
        public void ExceedsMaximumFirstNameReturnsHydratedValidationResult()
        {
            _createPersonCommand.FirstName = CommonMockData.GetRandomAlphabeticString(ValidationProperties.FirstName + 1);
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(
                ValidationTestConstants.FirstNameProperty,
                _createPersonCommand.FirstName,
                $"{ValidationTestConstants.FirstNameProperty}{ValidationTestConstants.MaximumCharacterLimit}{ValidationProperties.FirstName}");
            var expectedErrorCode = ErrorCodes.FirstNameInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.FirstName)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }

        [Test]
        public void ExceedsMaximumLastNameReturnsHydratedValidationResult()
        {
            _createPersonCommand.LastName = CommonMockData.GetRandomAlphabeticString(ValidationProperties.LastName + 1);
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(
                ValidationTestConstants.LastNameProperty,
                _createPersonCommand.LastName,
                $"{ValidationTestConstants.LastNameProperty}{ValidationTestConstants.MaximumCharacterLimit}{ValidationProperties.LastName}");
            var expectedErrorCode = ErrorCodes.LastNameInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.LastName)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }

        [Test]
        public void ExceedsMaximumInterestsReturnsHydratedValidationResult()
        {
            _createPersonCommand.Interests = CommonMockData.GetRandomAlphabeticString(ValidationProperties.Interests + 1);
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(
                ValidationTestConstants.InterestsProperty,
                _createPersonCommand.Interests,
                $"{ValidationTestConstants.InterestsProperty}{ValidationTestConstants.MaximumCharacterLimit}{ValidationProperties.Interests}");
            var expectedErrorCode = ErrorCodes.InterestsInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.Interests)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }

        [Test]
        public void ExceedsMaximumImageReturnsHydratedValidationResult()
        {
            _createPersonCommand.Image = CommonMockData.GetRandomAlphabeticString(ValidationProperties.Image + 1);
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(
                ValidationTestConstants.ImageProperty,
                _createPersonCommand.Image,
                $"{ValidationTestConstants.ImageProperty}{ValidationTestConstants.MaximumCharacterLimit}{ValidationProperties.Image}");
            var expectedErrorCode = ErrorCodes.ImageInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.Image)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(-100)]
        [TestCase(-1000)]
        [TestCase(-99999)]
        public void LessThanZeroAgeReturnsHydratedValidationResult(int age)
        {
            _createPersonCommand.Age = age;
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(
                ValidationTestConstants.AgeProperty,
                _createPersonCommand.Age.ToString(),
                $"{ValidationTestConstants.AgeProperty}{ValidationTestConstants.InvalidInteger}");
            var expectedErrorCode = ErrorCodes.AgeInvalid;

            var results = _createPersonValidator.TestValidate(_createPersonCommand);

            results.Should().NotBeNull();
            results.Should().BeOfType<TestValidationResult<CreatePersonCommand, CreatePersonCommand>>();
            results.IsValid.Should().BeFalse();
            results.Errors.Should().HaveCount(1);
            results.ShouldHaveValidationErrorFor(createPersonCommand => createPersonCommand.Age)
                .WithErrorCode(expectedErrorCode)
                .WithErrorMessage(expectedErrorMessage);
        }
    }
}
