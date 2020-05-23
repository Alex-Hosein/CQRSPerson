using CQRSPerson.Domain.Dtos;
using CQRSPerson.Domain.Responses;
using MediatR;
using Newtonsoft.Json;

namespace CQRSPerson.API.Person.Command
{
    public class CreatePersonCommand : IRequest<StandardContentResponse<CreatePersonDto>>
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }

        [JsonProperty("interests")]
        public string Interests { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }
    }
}
