using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Responses;
using MediatR;
using System.Collections.Generic;

namespace CQRSPerson.API.Person.GetPersons
{
    public class GetPersonsQuery : IRequest<StandardContentResponse<IEnumerable<PersonDto>>>
    {
    }
}
