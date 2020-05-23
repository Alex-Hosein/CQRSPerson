using CQRSPerson.Domain.Entities;
using CQRSPerson.TestData;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSPerson.Infrastructure.Tests.Repositories.PersonQueryRepository.Integration
{
    [TestFixture, Category("Integration")]
    public class GetAllAsyncTests : IntegrationTestBase
    {
        [Test]
        public async Task CanRetreiveAllPersons()
        {
            var results = await PersonQueryRepository.GetAllAsync();

            results.Should().NotBeNullOrEmpty();
            results.Should().BeOfType<List<Person>>();
        }
    }
}
