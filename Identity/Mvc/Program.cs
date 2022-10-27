using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = "oidc";
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("oidc", options =>
    {
        options.SignInScheme = "Cookies";
        options.Authority = "https://localhost:5001";
        options.RequireHttpsMetadata = true;

        options.ClientId = "web_client";
        options.ClientSecret = "client_secret_mvc";
        options.ResponseType = "code";

        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.Scope.Add("test.web");
        options.Scope.Add("offline_access");
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("test.web", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("test.web", "test.web");
    });
});

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "Default",
        pattern: "{controller=Site}/{action=Index}/{id?}");
});

app.Run();
