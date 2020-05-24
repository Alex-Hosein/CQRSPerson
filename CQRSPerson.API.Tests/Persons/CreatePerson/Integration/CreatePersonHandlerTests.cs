using CQRSPerson.API.Person.Command;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Responses;
using CQRSPerson.TestData;
using CQRSPerson.TestData.Constants;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSPerson.API.Tests.Persons.CreatePerson.Integration
{
    public class CreatePersonHandlerTests : IntegrationTestBase
    {
        private CreatePersonCommand _createPersonCommand;

        [SetUp]
        public void Setup()
        {
            _createPersonCommand = new CreatePersonCommand()
            {
                FirstName = CommonMockData.GetRandomAlphabeticString(50),
                LastName = CommonMockData.GetRandomAlphabeticString(50),
                Age = 29,
                Interests = CommonMockData.GetRandomAlphabeticString(50),
                Image = CommonMockData.GetRandomAlphabeticString(50)
            };
        }

        [Test]
        public async Task ValidCreatePersonCommandReturnsSuccessfulResponse()
        {
            var response = await CreatePersonHandler.Handle(_createPersonCommand, new CancellationToken());
            var person = await DBContext.Person.Where(x => x.PersonId == response.Content.PersonId).FirstOrDefaultAsync();
           
            try
            {
                response.Should().NotBeNull();
                response.Should().BeOfType<StandardContentResponse<CreatePersonDto>>();
                response.StatusCode.Should().Be(HttpStatusCode.Created);
                response.InformationalMessage.Should().Be(InformationalMessages.AddPersonSuccessMessage);
                response.Content.Should().NotBeNull();
                response.Content.Should().BeOfType<CreatePersonDto>();
                person.FirstName.Should().BeEquivalentTo(_createPersonCommand.FirstName);
                person.LastName.Should().BeEquivalentTo(_createPersonCommand.LastName);
                person.Age.Should().Be(_createPersonCommand.Age);
                person.Interests.Should().BeEquivalentTo(_createPersonCommand.Interests);
                person.Image.Should().BeEquivalentTo(_createPersonCommand.Image);
            }
            finally
            {
                DBContext.Person.Remove(person);
                DBContext.SaveChanges();
            }
        }

        [Test]
        public async Task ExceedsMaximumFirstNameReturnsHydratedResponse()
        {
            _createPersonCommand.FirstName = CommonMockData.GetRandomAlphabeticString(ValidationProperties.FirstName + 1);
            var expectedErrorCode = ErrorCodes.FirstNameInvalid;
            var expectedErrorMessage = ValidationErrorMessages.PropertyErrorMessage(
                ValidationTestConstants.FirstNameProperty,
                _createPersonCommand.FirstName,
                $"{ValidationTestConstants.FirstNameProperty}{ValidationTestConstants.MaximumCharacterLimit}{ValidationProperties.FirstName}");

            var response = await CreatePersonHandler.Handle(_createPersonCommand, new CancellationToken());

            response.Should().NotBeNull();
            response.Should().BeOfType<StandardContentResponse<CreatePersonDto>>();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Should().BeNull();
            response.Errors.Should().NotBeNullOrEmpty();
            response.Errors.Should().OnlyContain(x => x.Code == expectedErrorCode && x.Message == expectedErrorMessage);
            response.InformationalMessage.Should().Be(InformationalMessages.AddPersonFailure);
        }
    }
}
