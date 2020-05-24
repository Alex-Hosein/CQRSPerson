using CQRSPerson.API.Person.GetPersons;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Responses;
using CQRSPerson.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSPerson.API.Tests.Persons.GetPersons.Integration
{
    [TestFixture, Category("Integration")]
    public class GetPersonsHandlerTests : IntegrationTestBase
    {
        [Test]
        public async Task CanGetAllPersons()
        {
            var query = new GetPersonsQuery();
            var persons = await DBContext.Person.ToListAsync();

            var response = await GetPersonsHandler.Handle(query, new CancellationToken());

            response.Should().NotBeNull();
            response.Should().BeOfType<StandardContentResponse<IEnumerable<PersonDto>>>();
            response.Content.Should().NotBeNullOrEmpty();
            response.Errors.Should().BeNullOrEmpty();
            response.InformationalMessage.Should().Be(InformationalMessages.GetAllPersonsSuccessMessage);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().BeEquivalentTo(persons);
        }

    }
}
