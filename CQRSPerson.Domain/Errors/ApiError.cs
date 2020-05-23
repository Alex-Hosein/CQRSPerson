using Newtonsoft.Json;

namespace CQRSPerson.Domain.Errors
{
    public class ApiError
    {
        [JsonProperty("code")]
        public string Code { get; }

        [JsonProperty("context")]
        public string Context { get; }

        [JsonProperty("message")]
        public string Message { get; }

        public ApiError(string code, string context, string message)
        {
            Code = code;
            Context = context;
            Message = message;
        }
    }
}
