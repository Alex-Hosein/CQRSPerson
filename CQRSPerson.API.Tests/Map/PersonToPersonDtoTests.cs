using CQRSPerson.Domain.Dtos;
using CQRSPerson.TestData;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRSPerson.API.Tests.Map
{
    public class PersonToPersonDtoTests : UnitTestBase
    {
        [Test]
        public void CanMapPersonToPersonDto()
        {
            var person = new Domain.Entities.Person
            {
                PersonId = 1,
                FirstName = CommonMockData.GetRandomAlphabeticString(50),
                LastName = CommonMockData.GetRandomAlphabeticString(50),
                Age = 29,
                Interests = CommonMockData.GetRandomAlphabeticString(50),
                Image = CommonMockData.GetRandomAlphabeticString(50)
            };

            var mappedResult = Mapper.Map<PersonDto>(person);

            mappedResult.PersonId.Should().Be(person.PersonId);
            mappedResult.FirstName.Should().Be(person.FirstName);
            mappedResult.LastName.Should().Be(person.LastName);
            mappedResult.Age.Should().Be(person.Age);
            mappedResult.Interests.Should().Be(person.Interests);
            mappedResult.Image.Should().Be(person.Image);
        }
    }
}
