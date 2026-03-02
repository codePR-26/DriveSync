using System.Net.Http.Json;
using System.Text.Json;

namespace DriveSyncMVC.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _factory;

        public AuthService(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        // LOGIN WITH ROLE + NAME
        public async Task<(bool Success, string Role, string Name)> LoginAsync(string email, string password)
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            var res = await client.PostAsJsonAsync("Auth/login", new
            {
                email,
                password
            });

            if (!res.IsSuccessStatusCode)
                return (false, "", "");

            var json = await res.Content.ReadFromJsonAsync<JsonElement>();

            string role = json.GetProperty("role").GetString() ?? "";
            string name = json.GetProperty("name").GetString() ?? "";

            return (true, role, name);
        }

        // ROLE BASED REGISTER
        public async Task<bool> RegisterAsync(string name, string email, string password, string role)
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            string endpoint = role switch
            {
                "Admin" => "Auth/register-admin",
                "Owner" => "Auth/register-owner",
                _ => "Auth/register-customer"
            };

            var res = await client.PostAsJsonAsync(endpoint, new
            {
                name,
                email,
                password
            });

            return res.IsSuccessStatusCode;
        }
    }
}