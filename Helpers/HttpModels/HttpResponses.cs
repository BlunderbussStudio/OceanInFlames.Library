using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace OceansInFlame.Library.Helpers.HttpModels
{
    public static class HttpResponses
    {
        // Response Helpers
        public static HttpResponseMessage Ok() => new HttpResponseMessage(HttpStatusCode.OK);

        public static HttpResponseMessage Ok(byte[] message)
        {
            var content = new ByteArrayContent(message);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = content;
            return response;
        }

        public static HttpResponseMessage Ok(string message)
        {
            var content = new StringContent(message);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = content;
            return response;
        }

        public static HttpResponseMessage Ok(object body)
        {
            var content = JsonConvert.SerializeObject(body);
            return Ok(content);
        }

        public static HttpResponseMessage Ok(JsonContent content)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = content;
            return response;
        }

        public static HttpResponseMessage NotFound() => new HttpResponseMessage(HttpStatusCode.NotFound);

        public static HttpResponseMessage InternalServerError() =>
            new HttpResponseMessage(HttpStatusCode.InternalServerError);

        public static HttpResponseMessage BadRequest() => new HttpResponseMessage(HttpStatusCode.BadRequest);

        public static HttpResponseMessage NetworkAuthenticationRequired() => new HttpResponseMessage(HttpStatusCode.NetworkAuthenticationRequired);
        public static HttpResponseMessage Unauthorized() => new HttpResponseMessage(HttpStatusCode.Unauthorized);
    }
}