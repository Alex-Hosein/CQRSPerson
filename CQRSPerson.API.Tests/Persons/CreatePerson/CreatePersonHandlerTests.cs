using AutoMapper;
using CQRSPerson.API.Person.Command;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Errors;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.TestData;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSPerson.API.Tests.Persons.CreatePerson
{
    public class CreatePersonHandlerTests : UnitTestBase
    {
        private Mock<IApplicationLogger<CreatePersonHandler>> _logger;
        private const string propertyName = "CreatePersonCommand";
        private const string errorCode = "TEST";
        private const string errorMessage = "THIS IS A TEST";
        private CreatePersonHandler _createPersonHandler;
        private CreatePersonCommand _validCreatePersonCommand;
        private CreatePersonCommand _invalidCreatePersonCommand;
        private CreatePersonCommand _exceptionCreatePersonCommand;
        private Domain.Entities.Person _validPerson;
        private Domain.Entities.Person _invalidPerson;

        [SetUp]
        public void Setup()
        {
            SetupVariables();
            _logger = SetupLoggerMock<CreatePersonHandler>();
            _createPersonHandler = new CreatePersonHandler(SetupCreatePersonCommandValidator().Object, _logger.Object, SetupPersonCommandRepository().Object, SetupAutoMapper().Object);
        }

        [Test]
        public async Task ValidCreatePersonCommandCanCreatePerson()
        {
            var result = await _createPersonHandler.Handle(_validCreatePersonCommand, new CancellationToken());

            result.Should().NotBeNull();
            result.Content.Should().NotBeNull();
            result.Content.Should().BeEquivalentTo(new CreatePersonDto { PersonId = 1 });
            result.Errors.Should().BeNullOrEmpty();
            result.InformationalMessage.Should().Be(InformationalMessages.AddPersonSuccessMessage);
            result.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Test]
        public async Task ValidationFailureReturnsHydratedResponse()
        {
            var expectedError = new ApiError(errorCode, Contexts.AddPerson, errorMessage);

            var result = await _createPersonHandler.Handle(_invalidCreatePersonCommand, new CancellationToken());

            result.Should().NotBeNull();
            result.Content.Should().BeNull();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Count.Should().Be(1);
            result.Errors.Should().ContainEquivalentOf(expectedError);
            result.InformationalMessage.Should().Be(InformationalMessages.AddPersonFailure);
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task ExceptionCaughtReturnsHydratedResponse()
        {
            var expectedError = new ApiError(ErrorCodes.AddPersonErrorCode, Contexts.AddPerson, Exception.Message);

            var result = await _createPersonHandler.Handle(_exceptionCreatePersonCommand, new CancellationToken());

            result.Should().NotBeNull();
            result.Content.Should().BeNull();
            result.Errors.Should().NotBeNullOrEmpty();
            result.Errors.Count.Should().Be(1);
            result.Errors.Should().ContainEquivalentOf(expectedError);
            result.InformationalMessage.Should().Be(InformationalMessages.AddPersonFailure);
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            _logger.Verify(x => x.LogError(Exception, InformationalMessages.AddPersonFailure));
        }

        private void SetupVariables()
        {
            _validCreatePersonCommand = new CreatePersonCommand
            {
                FirstName = CommonMockData.GetRandomAlphabeticString(10),
                LastName = CommonMockData.GetRandomAlphabeticString(10),
                Age = 10,
                Image = CommonMockData.GetRandomAlphabeticString(10),
                Interests = CommonMockData.GetRandomAlphabeticString(10)
            };
            _invalidCreatePersonCommand = new CreatePersonCommand();

            _validPerson = new Domain.Entities.Person
            {
                PersonId = 1,
                FirstName = CommonMockData.GetRandomAlphabeticString(10),
                LastName = CommonMockData.GetRandomAlphabeticString(10),
                Age = 10,
                Image = CommonMockData.GetRandomAlphabeticString(10),
                Interests = CommonMockData.GetRandomAlphabeticString(10)
            };
            _invalidPerson = new Domain.Entities.Person();
            _exceptionCreatePersonCommand = new CreatePersonCommand
            {
                FirstName = CommonMockData.GetRandomAlphabeticString(10),
                LastName = CommonMockData.GetRandomAlphabeticString(10),
                Age = 10,
                Image = CommonMockData.GetRandomAlphabeticString(10),
                Interests = CommonMockData.GetRandomAlphabeticString(10)
            };
        }

        private Mock<IValidator<CreatePersonCommand>> SetupCreatePersonCommandValidator()
        {
            var validationResult = new ValidationResult(
                new List<ValidationFailure>()
                {
                    new ValidationFailure(propertyName, errorMessage, propertyName)
                    {
                        ErrorCode = errorCode
                    }
                }
            );
            var mock = new Mock<IValidator<CreatePersonCommand>>();
            mock.Setup(x => x.Validate(_validCreatePersonCommand)).Returns(new ValidationResult());
            mock.Setup(x => x.Validate(_invalidCreatePersonCommand)).Returns(validationResult);
            mock.Setup(x => x.Validate(_exceptionCreatePersonCommand)).Throws(Exception);
            return mock;
        }

        private Mock<IPersonCommandRepository> SetupPersonCommandRepository()
        {
            var mock = new Mock<IPersonCommandRepository>();
            mock.Setup(x => x.InsertAsync(_validPerson)).ReturnsAsync(1);
            mock.Setup(x => x.InsertAsync(_invalidPerson)).ReturnsAsync(0);
            return mock;
        }

        private Mock<IMapper> SetupAutoMapper()
        {
            var mock = new Mock<IMapper>();
            mock.Setup(x => x.Map<Domain.Entities.Person>(_validCreatePersonCommand)).Returns(_validPerson);
            mock.Setup(x => x.Map<Domain.Entities.Person>(_invalidCreatePersonCommand)).Returns(_invalidPerson);
            return mock;
        }

    }
}
