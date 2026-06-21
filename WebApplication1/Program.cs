using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

var builder = WebApplication.CreateBuilder(args);

#region MVC SERVICES
builder.Services.AddControllersWithViews();
#endregion

#region DATABASE
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);
#endregion

#region HTTP CLIENT (API CONNECTION)
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7065/"); // 🔥 Your API URL
    client.Timeout = TimeSpan.FromSeconds(30);
});
#endregion

#region SESSION
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

#region JSON (OPTIONAL BUT RECOMMENDED)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
#endregion

var app = builder.Build();

#region PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// 🔐 If you add authentication later, it goes here
// app.UseAuthentication();

app.UseAuthorization();
#endregion

#region ROUTES
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
#endregion

app.Run();

// 🔥 REQUIRED for Integration Testing (VERY IMPORTANT FOR POE)
public partial class Program { }