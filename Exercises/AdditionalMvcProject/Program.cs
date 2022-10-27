using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, config =>
    {
        config.Authority = "https://localhost:10001";
        config.ClientId = "add_mvc";
        config.ClientSecret = "add_mvc_secret";
        config.SaveTokens = true;
        config.TokenValidationParameters = new TokenValidationParameters
        {
        ValidateAudience = false
        };

        config.ResponseType = "code";

        config.Scope.Add("OrdersAPI");
        config.Scope.Add("offline_access");

        config.GetClaimsFromUserInfoEndpoint = true;

        config.ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, ClaimTypes.DateOfBirth);
        });

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("HasDateOfBirth", builder =>
    {
        builder.RequireClaim(ClaimTypes.DateOfBirth);
    });
});

builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();
        

    // Add services to the container.
    builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
