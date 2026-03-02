using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace DriveSyncMVC.Services
{
    public class ReservationService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _http;

        public ReservationService(IHttpClientFactory factory, IHttpContextAccessor http)
        {
            _factory = factory;
            _http = http;
        }

        // ===============================
        // CREATE RESERVATION
        // ===============================
        public async Task<bool> CreateReservation(int vehicleId, DateTime start, DateTime end)
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            AttachCookies(client);

            var response = await client.PostAsJsonAsync("Reservation", new
            {
                vehicleId,
                startDate = start,
                endDate = end
            });

            return response.IsSuccessStatusCode;
        }

        // ===============================
        // GET MY RESERVATIONS
        // ===============================
        public async Task<List<MyReservationDto>> GetMyReservations()
        {
            var client = _factory.CreateClient("DriveSyncAPI");

            AttachCookies(client);

            var response = await client.GetAsync("Reservation/my");

            if (!response.IsSuccessStatusCode)
                return new List<MyReservationDto>();

            var data = await response.Content.ReadFromJsonAsync<List<MyReservationDto>>();
            return data ?? new List<MyReservationDto>();
        }

        // ===============================
        // HELPER: ATTACH COOKIES
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