using FluentAssertions;
using CQRSPerson.API.Person.GetPersons;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Errors;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Responses;
using CQRSPerson.TestData;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSPerson.API.Tests.Controllers.PersonController.GetAsyncTests
{
    public class GetAsyncTests : UnitTestBase
    {
        private Mock<IApplicationLogger<API.Controllers.PersonController>> _logger;
        private Mock<IMediator> _validMediator;
        private Mock<IMediator> _exceptionMediator;
        private StandardContentResponse<IEnumerable<PersonDto>> _response;
        private StandardContentResponse<IEnumerable<PersonDto>> _errorResponse;

        [OneTimeSetUp]
        public void Setup()
        {
            SetupVariables();
        }

        [Test]
        public async Task CanGetPersons()
        {
            var controller = new API.Controllers.PersonController(_validMediator.Object, _logger.Object);

            var response = await controller.GetAsync();

            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            response.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.OK);
            response.As<ObjectResult>().Value.Should().NotBeNull();
            response.As<ObjectResult>().Value.Should().BeOfType<StandardContentResponse<IEnumerable<PersonDto>>>();
            response.As<ObjectResult>().Value.As<StandardContentResponse<IEnumerable<PersonDto>>>().Should().BeEquivalentTo(_response);
        }

        [Test]
        public async Task ExceptionReturnsHydratedResponse()
        {
            var controller = new API.Controllers.PersonController(_exceptionMediator.Object, _logger.Object);

            var response = await controller.GetAsync();

            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            response.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            response.As<ObjectResult>().Value.Should().NotBeNull();
            response.As<ObjectResult>().Value.Should().BeOfType<StandardContentResponse<IEnumerable<PersonDto>>>();
            response.As<ObjectResult>().Value.As<StandardContentResponse<IEnumerable<PersonDto>>>().Should().BeEquivalentTo(_errorResponse);
            _logger.Verify(x => x.LogError(Exception, InformationalMessages.GetAllPersonException));
        }

        private void SetupVariables()
        {
            _logger = SetupLoggerMock<API.Controllers.PersonController>();
            _response = new StandardContentResponse<IEnumerable<PersonDto>>()
            {
                Errors = new List<ApiError>(),
                Content = new List<PersonDto>()
                {
                    new PersonDto()
                    {
                        PersonId = 1,
                        Age = 29,
                        FirstName = "FirstName",
                        LastName = "LastName",
                        Image = "URL",
                        Interests = "Interests"
                    }
                },
                StatusCode = HttpStatusCode.OK,
                InformationalMessage = InformationalMessages.GetAllPersonsSuccessMessage
            };
            var expectedErrors = new List<ApiError> { new ApiError(
                ErrorCodes.GetAllPersonsErrorCode,
                Contexts.GetAllPersons,
                Exception.Message)};
            _errorResponse = new StandardContentResponse<IEnumerable<PersonDto>>()
            {
                Errors = expectedErrors,
                Content = null,
                StatusCode = HttpStatusCode.InternalServerError,
                InformationalMessage = InformationalMessages.GetAllPersonException
            };
            SetupSuccessMediator();
            SetupExceptionMediator();
        }



        private void SetupSuccessMediator()
        {
            var mock = new Mock<IMediator>();
            mock.Setup(m => m.Send(It.IsAny<GetPersonsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(_response);

            _validMediator = mock;
        }

        private void SetupExceptionMediator()
        {
            var mock = new Mock<IMediator>();
            mock.Setup(m => m.Send(It.IsAny<GetPersonsQuery>(), It.IsAny<CancellationToken>())).Throws(Exception);

            _exceptionMediator = mock;
        }
    }
}
