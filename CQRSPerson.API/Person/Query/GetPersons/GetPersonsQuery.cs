using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSPerson.API.Person.GetPersons
{
    public class GetPersonsQuery : IRequest<StandardContentResponse<IEnumerable<PersonDto>>>
    {
    }
}
