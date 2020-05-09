using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAuth.Model;

namespace OpenAuth.Service
{
    public class TokenService : ITokenService
    {
        private readonly OpenAuthOptions _options;

        public TokenService(OpenAuthOptions options)
        {
            _options = options;
        }


        public async Task<string> GetAccessToken(string code)
        {
            var data = new
            {
                client_id = _options.ClientId,
                client_secret = _options.ClientSecret,
                grant_type = "authorization_code",
                code = code
            };
            return await PostAsync("oauth2/access-token", data);
        }

        public async Task<string> RefreshToken(string token)
        {
            var data = new
            {
                client_id = _options.ClientId,
                client_secret = _options.ClientSecret,
                grant_type = "refresh_token",
                refresh_token = token
            };
            return await PostAsync("oauth2/refresh-token", data);
        }

        public async Task<string> CheckToken(string token)
        {
            var url = $"{_options.Authority}/oauth2/check-token?token={token}";
            var result = await GetAsync(url);
            var jsonStr = JsonConvert.SerializeObject(result);
            return jsonStr;
        }

        public async Task Logout()
        {
            await PostAsync("oauth2/logout", null);
        }

        private async Task<string> PostAsync(string url, object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_options.Authority);
                client.Timeout = TimeSpan.FromSeconds(10);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        private static async Task<Dictionary<string, object>> GetAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
            }
        }
    }
}
