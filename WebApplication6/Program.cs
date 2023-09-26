using Microsoft.AspNetCore.Authorization;
using Moryx.Identity.AccessManagement;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddMoryxAccessManagement(
            builder.Configuration.GetSection("Jwt"),
            builder.Configuration.GetConnectionString("ConnectionString"), // e.g. "Username=postgres;Password=postgres;Host=localhost;Port=5432;Persist Security Info=True;Database=IdentityServer"
            corsOptions =>
            {
                corsOptions.AddPolicy("CorsPolicy", builder => builder
                .SetIsOriginAllowed(origin => true) // We recommend a more strict setting for production environments
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMoryxAccessManagement("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

//app.MapGet("/", () => "Hello World!");


app.Run();
