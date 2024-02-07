using Newtonsoft.Json;

namespace OceansInFlame.Library.Helpers.HttpModels
{
    public static class HttpRequests
    {
        public static async Task<T> DeserializeBodyAsync<T>(this HttpRequestMessage req)
        {
            var requestStream = req.Content.ReadAsStream();
            var requestReader = new StreamReader(requestStream);
            string requestBody = await requestReader.ReadToEndAsync();
            var bodyObject = JsonConvert.DeserializeObject<T>(requestBody);
            return bodyObject;
        }
    }
}