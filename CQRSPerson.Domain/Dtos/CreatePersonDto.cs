using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRSPerson.Domain.Dtos
{
    public class CreatePersonDto
    {
        [JsonProperty("personId")]
        public int PersonId { get; set; }
    }
}
