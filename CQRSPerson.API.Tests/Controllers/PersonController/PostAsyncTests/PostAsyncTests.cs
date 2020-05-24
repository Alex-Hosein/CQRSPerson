using CQRSPerson.API.Person.Command;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Errors;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Responses;
using CQRSPerson.TestData;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSPerson.API.Tests.Controllers.PersonController.PostAsyncTests
{
    public class PostAsyncTests : UnitTestBase
    {
        private Mock<IApplicationLogger<API.Controllers.PersonController>> _logger;
        private Mock<IMediator> _validMediator;
        private Mock<IMediator> _exceptionMediator;
        private StandardContentResponse<CreatePersonDto> _response;
        private StandardContentResponse<CreatePersonDto> _errorResponse;

        [OneTimeSetUp]
        public void Setup()
        {
            SetupVariables();
        }

        [Test]
        public async Task CanCreatePerson()
        {
            var controller = new API.Controllers.PersonController(_validMediator.Object, _logger.Object);

            var response = await controller.PostAsync(new CreatePersonCommand());

            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            response.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.Created);
            response.As<ObjectResult>().Value.Should().NotBeNull();
            response.As<ObjectResult>().Value.Should().BeOfType<StandardContentResponse<CreatePersonDto>>();
            response.As<ObjectResult>().Value.As<StandardContentResponse<CreatePersonDto>>().Should().BeEquivalentTo(_response);
        }

        [Test]
        public async Task ExceptionReturnsHydratedResponse()
        {
            var controller = new API.Controllers.PersonController(_exceptionMediator.Object, _logger.Object);

            var response = await controller.PostAsync(new CreatePersonCommand());

            response.Should().NotBeNull();
            response.Should().BeOfType<ObjectResult>();
            response.As<ObjectResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            response.As<ObjectResult>().Value.Should().NotBeNull();
            response.As<ObjectResult>().Value.Should().BeOfType<StandardContentResponse<CreatePersonDto>>();
            response.As<ObjectResult>().Value.As<StandardContentResponse<CreatePersonDto>>().Should().BeEquivalentTo(_errorResponse);
            _logger.Verify(x => x.LogError(Exception, InformationalMessages.AddPersonFailure));
        }

        private void SetupVariables()
        {
            _logger = SetupLoggerMock<API.Controllers.PersonController>();
            _response = new StandardContentResponse<CreatePersonDto>()
            {
                Errors = new List<ApiError>(),
                Content = new CreatePersonDto()
                {
                    PersonId = 1
                },
                StatusCode = HttpStatusCode.Created,
                InformationalMessage = InformationalMessages.AddPersonFailure
            };
            var expectedErrors = new List<ApiError> { new ApiError(
                ErrorCodes.AddPersonErrorCode,
                Contexts.AddPerson,
                Exception.Message)};
            _errorResponse = new StandardContentResponse<CreatePersonDto>()
            {
                Errors = expectedErrors,
                Content = null,
                StatusCode = HttpStatusCode.InternalServerError,
                InformationalMessage = InformationalMessages.AddPersonFailure
            };
            SetupSuccessMediator();
            SetupExceptionMediator();
        }

        private void SetupSuccessMediator()
        {
            var mock = new Mock<IMediator>();
            mock.Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(_response);

            _validMediator = mock;
        }

        private void SetupExceptionMediator()
        {
            var mock = new Mock<IMediator>();
            mock.Setup(m => m.Send(It.IsAny<CreatePersonCommand>(), It.IsAny<CancellationToken>())).Throws(Exception);

            _exceptionMediator = mock;
        }
    }
}
