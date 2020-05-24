using CQRSPerson.Domain.Entities;
using CQRSPerson.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSPerson.Infrastructure.Tests.Repositories.PersonQueryRepository.Integration
{
    [TestFixture, Category("Integration")]
    public class GetAllAsyncTests : IntegrationTestBase
    {
        [Test]
        public async Task CanRetreiveAllPersons()
        {
            var allPersons = DBContext.Person.Select(x => x).ToList();

            var results = await PersonQueryRepository.GetAllAsync();

            results.Should().NotBeNullOrEmpty();
            results.Should().BeOfType<List<Person>>();
            results.Should().BeEquivalentTo(allPersons);
        }
    }
}
