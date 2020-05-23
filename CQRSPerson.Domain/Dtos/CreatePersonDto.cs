using Newtonsoft.Json;

namespace CQRSPerson.Domain.Dtos
{
    public class CreatePersonDto
    {
        [JsonProperty("personId")]
        public int PersonId { get; set; }
    }
}
