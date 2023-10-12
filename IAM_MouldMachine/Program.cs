using Microsoft.AspNetCore.Authorization;
using Moryx.Identity.AccessManagement;
using Moryx.Launcher;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add services to the container.
builder.Services.AddMoryxAccessManagement(
            builder.Configuration.GetSection("Jwt"),
            builder.Configuration.GetConnectionString("DefaultConnection"), // e.g. "Username=postgres;Password=postgres;Host=localhost;Port=5432;Persist Security Info=True;Database=IdentityServer"
            corsOptions =>
            {
                corsOptions.AddPolicy("CorsPolicy", builder => builder
                .SetIsOriginAllowed(origin => true) // We recommend a more strict setting for production environments
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            });
builder.Services.AddSingleton<IShellNavigator, ShellNavigator>();
builder.Services.AddRazorPages();

var app = builder.Build();


await app.Migrate().Seed();



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMoryxAccessManagement("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

//app.MapGet("/", () => "Hello World!");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
