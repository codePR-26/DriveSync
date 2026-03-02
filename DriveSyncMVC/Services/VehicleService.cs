using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace DriveSyncMVC.Services
{
    public class VehicleService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _http;

        public VehicleService(IHttpClientFactory factory, IHttpContextAccessor http)
        {
            _factory = factory;
            _http = http;
        }

        // ===============================
        // GET ALL VEHICLES (PUBLIC)
        // ===============================
        public async Task<List<VehicleDto>> GetAllVehiclesAsync()
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            var res = await client.GetFromJsonAsync<List<VehicleDto>>("Vehicle");
            return res ?? new List<VehicleDto>();
        }

        // ===============================
        // ADD VEHICLE (ADMIN ONLY)
        // ===============================
        public async Task<bool> AddVehicleAsync(object vehicle)
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            AttachCookies(client);

            var res = await client.PostAsJsonAsync("Vehicle", vehicle);
            return res.IsSuccessStatusCode;
        }

        // ===============================
        // COOKIE FORWARD HELPER
        // ===============================
        private void AttachCookies(HttpClient client)
        {
            var cookies = _http.HttpContext?.Request.Headers["Cookie"].ToString();

            if (!string.IsNullOrEmpty(cookies))
            {
                if (!client.DefaultRequestHeaders.Contains("Cookie"))
                    client.DefaultRequestHeaders.Add("Cookie", cookies);
            }
        }
    }
}

