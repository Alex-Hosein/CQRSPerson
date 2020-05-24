using AutoMapper;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Errors;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.Domain.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSPerson.API.Person.GetPersons
{
    public class GetPersonsHandler : BaseHandler<IEnumerable<PersonDto>, GetPersonsHandler>, IRequestHandler<GetPersonsQuery, StandardContentResponse<IEnumerable<PersonDto>>>
    {
        private readonly IPersonQueryRepository _personQueryRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationLogger<GetPersonsHandler> _logger;

        public GetPersonsHandler(IPersonQueryRepository personQueryRepository, IMapper mapper, IApplicationLogger<GetPersonsHandler> logger)
        {
            _personQueryRepository = personQueryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StandardContentResponse<IEnumerable<PersonDto>>> Handle(GetPersonsQuery query, CancellationToken cancellationToken)
        {
            var response = new StandardContentResponse<IEnumerable<PersonDto>>()
            {
                Errors = new List<ApiError>(),
                StatusCode = HttpStatusCode.OK
            };

            try
            {
                var persons = await _personQueryRepository.GetAllAsync();

                if (persons?.Count() > 0)
                {
                    response.Content = _mapper.Map<List<PersonDto>>(persons);
                    response.InformationalMessage = InformationalMessages.GetAllPersonsSuccessMessage;
                }
                else
                {
                    response.InformationalMessage = InformationalMessages.GetAllPersonsNoResults;
                }
            }
            catch (Exception ex)
            {
                response = HandleInternalServerError(
                    ex, _logger, InformationalMessages.GetAllPersonException, ErrorCodes.GetAllPersonsErrorCode, Contexts.GetAllPersons);
            }
            return response;
        }
    }
}
