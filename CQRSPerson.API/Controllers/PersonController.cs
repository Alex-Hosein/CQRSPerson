using CQRSPerson.API.Person.Command;
using CQRSPerson.API.Person.GetPersons;
using CQRSPerson.Domain.Constants;
using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSPerson.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : BaseController<PersonController>
    {
        private readonly IMediator _mediator;
        private readonly IApplicationLogger<PersonController> _logger;

        public PersonController(IMediator mediator, IApplicationLogger<PersonController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            ObjectResult response;
            try
            {
                var query = new GetPersonsQuery();
                var result = await _mediator.Send(query);
                response = new ObjectResult(result) { StatusCode = (int)result.StatusCode };
            }
            catch (Exception ex)
            {
                response = HandleInternalServerError<IEnumerable<PersonDto>>(
                    InformationalMessages.GetAllPersonException, ErrorCodes.GetAllPersonsErrorCode, Contexts.GetAllPersons, ex, _logger);
            }
            return response;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CreatePersonCommand createPersonCommand)
        {
            ObjectResult response;
            try
            {
                var result = await _mediator.Send(createPersonCommand);
                response = new ObjectResult(result) { StatusCode = (int)result.StatusCode };
            }
            catch (Exception ex)
            {
                response = HandleInternalServerError<CreatePersonDto>(
                    InformationalMessages.AddPersonFailure, ErrorCodes.AddPersonErrorCode, Contexts.AddPerson, ex, _logger);
            }
            return response;
        }
    }
}
