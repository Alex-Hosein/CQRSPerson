using CQRSPerson.Domain.Entities;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.TestData;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSPerson.Infrastructure.Tests.Repositories.PersonQueryRepository
{
    public class GetAllAsyncTests : UnitTestBase
    {
        private IPersonQueryRepository _validPersonQueryRepository;
        private IPersonQueryRepository _invalidPersonQueryRepository;
        private List<Person> _personList;
        private List<Person> _emptyList;

        [OneTimeSetUp]
        public void Setup()
        {
            SetupVariables();
        }

        [Test]
        public async Task CanGetAllAsync()
        {
            var result = await _validPersonQueryRepository.GetAllAsync();

            result.Should().NotBeNullOrEmpty();
            result.Should().BeOfType<List<Person>>();
            result.Should().BeEquivalentTo(_personList);
        }

        [Test]
        public async Task NoResultsReturnsEmptyList()
        {
            var result = await _invalidPersonQueryRepository.GetAllAsync();

            result.Should().BeEmpty();
            result.Should().BeOfType<List<Person>>();
            result.Should().BeEquivalentTo(_emptyList);
        }

        private void SetupPersonQueryRepository()
        {
            var validMock = new Mock<IPersonQueryRepository>();
            var invalidMock = new Mock<IPersonQueryRepository>();
            validMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_personList);
            invalidMock.Setup(x => x.GetAllAsync()).ReturnsAsync(_emptyList);

            _validPersonQueryRepository = validMock.Object;
            _invalidPersonQueryRepository = invalidMock.Object;
        }

        private void SetupVariables()
        {
            _personList = new List<Person> { new Person() };
            _emptyList = new List<Person>();
            SetupPersonQueryRepository();
        }
    }
}
