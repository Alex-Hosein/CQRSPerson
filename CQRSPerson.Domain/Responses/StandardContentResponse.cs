using CQRSPerson.Domain.Errors;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace CQRSPerson.Domain.Responses
{
    public class StandardContentResponse<T>
    {
        [JsonProperty("content")]
        public T Content { get; set; }

        [JsonProperty("errors")]
        public List<ApiError> Errors { get; set; }

        [JsonProperty("informationalMessage")]
        public string InformationalMessage { get; set; }

        [JsonProperty("statusCode")]
        public HttpStatusCode StatusCode { get; set; }
    }
}
