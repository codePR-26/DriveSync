using DriveSyncMVC.Services;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// MVC
// ===============================
builder.Services.AddControllersWithViews();

// ===============================
// SESSION
// ===============================
builder.Services.AddSession();

// ===============================
// HTTP CLIENT (API BRIDGE)
// ===============================
builder.Services.AddHttpClient("DriveSyncAPI", client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ApiSettings:BaseUrl"]);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        UseCookies = true,
        CookieContainer = new System.Net.CookieContainer(),
        ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
});

// HttpContext access
builder.Services.AddHttpContextAccessor();

// ===============================
// SERVICES
// ===============================
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<ReservationService>();

var app = builder.Build();

// ===============================
// PIPELINE
// ===============================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ✅ REQUIRED FOR CSS / JS
app.UseStaticFiles();

app.UseRouting();

// ✅ SESSION AFTER ROUTING
app.UseSession();

app.UseAuthorization();

// Default MVC route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();