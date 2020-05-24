using CQRSPerson.Domain.Entities;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.TestData;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace CQRSPerson.Infrastructure.Tests.Repositories.PersonCommandRepository
{
    public class InsertAsyncTests : UnitTestBase
    {
        private IPersonCommandRepository _validPersonCommandRepository;
        private IPersonCommandRepository _invalidPersonCommandRepository;
        private Person _person;

        [OneTimeSetUp]
        public void Setup()
        {
            SetupVariables();
        }

        [Test]
        public async Task CanCreatePerson()
        {
            var result = await _validPersonCommandRepository.InsertAsync(_person);

            result.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task NoResultsReturnsZero()
        {
            var result = await _invalidPersonCommandRepository.InsertAsync(_person);

            result.Should().Be(0);
        }

        private void SetupPersonQueryRepository()
        {
            var validMock = new Mock<IPersonCommandRepository>();
            var invalidMock = new Mock<IPersonCommandRepository>();
            validMock.Setup(x => x.InsertAsync(_person)).ReturnsAsync(1);
            invalidMock.Setup(x => x.InsertAsync(It.IsAny<Person>())).ReturnsAsync(0);

            _validPersonCommandRepository = validMock.Object;
            _invalidPersonCommandRepository = invalidMock.Object;
        }

        private void SetupVariables()
        {
            _person = new Person
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Age = 29,
                Interests = "Hello World",
                Image = "Image"
            };
            SetupPersonQueryRepository();
        }
    }
}
