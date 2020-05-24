using AutoMapper;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Logging;
using CQRSPerson.Domain.Repositories;
using CQRSPerson.Domain.Responses;
using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSPerson.API.Person.Command
{
    public class CreatePersonHandler : BaseHandler<CreatePersonDto, CreatePersonHandler>, IRequestHandler<CreatePersonCommand, StandardContentResponse<CreatePersonDto>>
    {
        private readonly IValidator<CreatePersonCommand> _validator;
        private readonly IApplicationLogger<CreatePersonHandler> _logger;
        private readonly IPersonCommandRepository _personCommandRepository;
        private readonly IMapper _mapper;

        public CreatePersonHandler(IValidator<CreatePersonCommand> validator, IApplicationLogger<CreatePersonHandler> logger, IPersonCommandRepository personCommandRepository, IMapper mapper)
        {
            _validator = validator;
            _logger = logger;
            _personCommandRepository = personCommandRepository;
            _mapper = mapper;
        }
        public async Task<StandardContentResponse<CreatePersonDto>> Handle(CreatePersonCommand createPersonCommand, CancellationToken cancellationToken)
        {
            var response = new StandardContentResponse<CreatePersonDto>() { StatusCode = HttpStatusCode.OK };
            try
            {
                var validationResult = _validator.Validate(createPersonCommand);
                if (validationResult?.Errors?.Any() == true)
                {
                    response = HandleValidationError(
                        validationErrors: validationResult.Errors.ToList(),
                        informationalMessage: InformationalMessages.AddPersonFailure,
                        context: Contexts.AddPerson);
                }
                else
                {
                    var person = _mapper.Map<Domain.Entities.Person>(createPersonCommand);
                    var personId = await _personCommandRepository.InsertAsync(person);
                    if (personId > 0)
                    {
                        response.Content = new CreatePersonDto { PersonId = personId };
                        response.InformationalMessage = InformationalMessages.AddPersonSuccessMessage;
                        response.StatusCode = HttpStatusCode.Created;
                    }
                    else
                    {
                        response = HandleGenericFailure(InformationalMessages.AddPersonFailure, ErrorCodes.AddPersonErrorCode, Contexts.AddPerson, InformationalMessages.AddPersonFailure);
                    }
                }
            }
            catch (Exception ex)
            {
                response = HandleInternalServerError(ex, _logger, InformationalMessages.AddPersonFailure, ErrorCodes.AddPersonErrorCode, Contexts.AddPerson);
            }

            return response;
        }
    }

}
