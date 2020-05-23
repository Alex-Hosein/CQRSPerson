using AutoMapper;
using FluentAssertions;
using CQRSPerson.API.Person.GetPersons;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.Domain.Responses;
using CQRSPerson.TestData;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSPerson.API.Tests.Persons.GetPersons.Integration
{
    [TestFixture,Category("Integration")]
    public class GetPersonsHandlerTests : IntegrationTestBase
    {
        private GetPersonsHandler _getPersonsHandler;

        [Test]
        public async Task CanGetAllPersons()
        {
            var query = new GetPersonsQuery();
            _getPersonsHandler = new GetPersonsHandler(PersonQueryRepository, Mapper, GetPersonsHandlerLogger);

            var response = await _getPersonsHandler.Handle(query, new CancellationToken());

            response.Should().NotBeNull();
            response.Should().BeOfType<StandardContentResponse<IEnumerable<PersonDto>>>();
            response.Content.Should().NotBeNullOrEmpty();
            response.Errors.Should().BeNullOrEmpty();
            response.InformationalMessage.Should().Be(InformationalMessages.GetAllPersonsSuccessMessage);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}
