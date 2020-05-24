using CQRSPerson.Domain.Entities;
using CQRSPerson.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSPerson.Infrastructure.Tests.Repositories.PersonCommandRepository.Integration
{
    [TestFixture, Category("Integration")]
    public class InsertAsyncTests : IntegrationTestBase
    {
        private Person _person;

        [Test]
        public async Task CanInsertPerson()
        {
            _person = new Person
            {
                FirstName = "Alex",
                LastName = "Hosein",
                Age = 29,
                Interests = "Software Development",
                Image = "MyImageUrl"
            };

            var result = await PersonCommandRepository.InsertAsync(_person);
            var personResult = await DBContext.Person.Where(x => x.PersonId == result).FirstOrDefaultAsync();
            try
            {
                result.Should().BeGreaterThan(0);
                personResult.PersonId.Should().Be(result);
                personResult.FirstName.Should().Be(_person.FirstName);
                personResult.LastName.Should().Be(_person.LastName);
                personResult.Age.Should().Be(_person.Age);
                personResult.Interests.Should().Be(_person.Interests);
                personResult.Image.Should().Be(_person.Image);
            }
            finally{
                DBContext.Person.Remove(personResult);
                DBContext.SaveChanges();
            }
        }
    }
}
