using CQRSPerson.API.Person.Command;
using CQRSPerson.TestData;
using FluentAssertions;
using NUnit.Framework;


namespace CQRSPerson.API.Tests.Map
{
    public class CreatePersonCommandToPersonTests : UnitTestBase
    {
        [Test]
        public void CanMapCreatePersonCommandToPerson()
        {
            var person = new CreatePersonCommand
            {
                FirstName = CommonMockData.GetRandomAlphabeticString(50),
                LastName = CommonMockData.GetRandomAlphabeticString(50),
                Age = 29,
                Interests = CommonMockData.GetRandomAlphabeticString(50),
                Image = CommonMockData.GetRandomAlphabeticString(50)
            };

            var mappedResult = Mapper.Map<Domain.Entities.Person>(person);

            mappedResult.FirstName.Should().Be(person.FirstName);
            mappedResult.LastName.Should().Be(person.LastName);
            mappedResult.Age.Should().Be(person.Age);
            mappedResult.Interests.Should().Be(person.Interests);
            mappedResult.Image.Should().Be(person.Image);
        }
    }
}
