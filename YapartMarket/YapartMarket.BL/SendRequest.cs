using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YapartMarket.BL
{
    public abstract class SendRequest<T>
    {
        protected virtual async Task<string> Send(HttpClient httpClient, string url, string body)
        {
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(url, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            return resultContent;
        }

        protected virtual async Task<string> Request(T model, string url, HttpClient httpClient)
        {
            var body = JsonConvert.SerializeObject(model);
            return await Send(httpClient, url, body);
        }
    }
}
