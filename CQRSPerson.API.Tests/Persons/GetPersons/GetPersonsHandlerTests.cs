using AutoMapper;
using CQRSPerson.API.Person.GetPersons;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Errors;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.TestData;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CQRSPerson.API.Tests.Persons.GetPersons
{
    public class GetPersonsHandlerTests : UnitTestBase
    {
        private GetPersonsHandler _getPersonsHandler;
        private Mock<IPersonQueryRepository> _resultsPersonQueryRepository;
        private Mock<IPersonQueryRepository> _noResultsPersonQueryRepository;
        private Mock<IPersonQueryRepository> _exceptionPersonQueryRepository;
        private Mock<IMapper> _mapper;
        private Mock<IApplicationLogger<GetPersonsHandler>> _logger;

        private List<Domain.Entities.Person> _validPersonList;
        private List<Domain.Entities.Person> _emptyPersonList;
        private List<PersonDto> _validPersonDtoList;

        [OneTimeSetUp]
        public void Setup()
        {
            SetupVariables();
        }

        [Test]
        public async Task CanGetAllPersons()
        {
            var query = new GetPersonsQuery();
            SetupResultsPersonsHandler();

            var results = await _getPersonsHandler.Handle(query, default);

            results.Should().NotBeNull();
            results.Content.Should().NotBeNull();
            results.Content.Should().BeOfType<List<PersonDto>>();
            results.Content.Should().BeEquivalentTo(_validPersonDtoList);
            results.Errors.Should().BeNullOrEmpty();
            results.InformationalMessage.Should().Be(InformationalMessages.GetAllPersonsSuccessMessage);
            results.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task NoResultsReturnsPropertyHydratedResponse()
        {
            var query = new GetPersonsQuery();
            SetupNoResultsPersonsHandler();

            var results = await _getPersonsHandler.Handle(query, default);

            results.Should().NotBeNull();
            results.Content.Should().BeNullOrEmpty();
            results.Errors.Should().BeNullOrEmpty();
            results.InformationalMessage.Should().Be(InformationalMessages.GetAllPersonsNoResults);
            results.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Test]
        public async Task ExceptionCaughtRetusnPropertyHydratedResponse()
        {
            var query = new GetPersonsQuery();
            SetupExceptionPersonsHandler();
            var expectedError = new ApiError(ErrorCodes.GetAllPersonsErrorCode, Contexts.GetAllPersons, Exception.Message);

            var results = await _getPersonsHandler.Handle(query, default);

            results.Should().NotBeNull();
            results.InformationalMessage.Should().Be(InformationalMessages.GetAllPersonException);
            results.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            results.Content.Should().BeNullOrEmpty();
            results.Errors.Should().NotBeNullOrEmpty();
            results.Errors.Count.Should().Be(1);
            results.Errors.Should().ContainEquivalentOf(expectedError);
            _logger.Verify(x => x.LogError(Exception, InformationalMessages.GetAllPersonException));

        }

        private void SetupVariables()
        {
            _validPersonList = new List<Domain.Entities.Person> { new Domain.Entities.Person()
                {
                PersonId = 1,
                Age =22,
                FirstName = CommonMockData.GetRandomAlphabeticString(50),
                LastName = CommonMockData.GetRandomAlphabeticString(50),
                Interests = CommonMockData.GetRandomAlphabeticString(50),
                Image = CommonMockData.GetRandomAlphabeticString(50),
                }
            };
            _validPersonDtoList = new List<PersonDto> { new PersonDto() {
                PersonId = 1,
                Age = 22,
                FirstName = CommonMockData.GetRandomAlphabeticString(50),
                LastName = CommonMockData.GetRandomAlphabeticString(50),
                Interests = CommonMockData.GetRandomAlphabeticString(50),
                Image = CommonMockData.GetRandomAlphabeticString(50),
                }
            };
            _emptyPersonList = new List<Domain.Entities.Person> { };
            _logger = SetupLoggerMock<GetPersonsHandler>();
            SetupValidPersonQueryRepository();
            SetupEmptyPersonQueryRepository();
            SetupExceptionPersonQueryRepository();
            SetupAutoMapper();
        }

        private void SetupValidPersonQueryRepository()
        {
            var mock = new Mock<IPersonQueryRepository>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(_validPersonList);
            _resultsPersonQueryRepository = mock;
        }

        private void SetupEmptyPersonQueryRepository()
        {
            var mock = new Mock<IPersonQueryRepository>();
            mock.Setup(x => x.GetAllAsync()).ReturnsAsync(_emptyPersonList);
            _noResultsPersonQueryRepository = mock;
        }

        private void SetupExceptionPersonQueryRepository()
        {
            var mock = new Mock<IPersonQueryRepository>();
            mock.Setup(x => x.GetAllAsync()).Throws(Exception);
            _exceptionPersonQueryRepository = mock;
        }
        private void SetupAutoMapper()
        {
            var mock = new Mock<IMapper>();
            mock.Setup(x => x.Map<List<PersonDto>>(_validPersonList)).Returns(_validPersonDtoList);
            _mapper = mock;
        }
        private void SetupResultsPersonsHandler()
        {
            _getPersonsHandler = new GetPersonsHandler(_resultsPersonQueryRepository.Object, _mapper.Object, _logger.Object);
        }

        private void SetupNoResultsPersonsHandler()
        {
            _getPersonsHandler = new GetPersonsHandler(_noResultsPersonQueryRepository.Object, _mapper.Object, _logger.Object);
        }
        private void SetupExceptionPersonsHandler()
        {
            _getPersonsHandler = new GetPersonsHandler(_exceptionPersonQueryRepository.Object, _mapper.Object, _logger.Object);
        }

    }
}
