using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

//
// ======================================
// DATABASE CONFIGURATION (EF CORE)
// ======================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//
// ======================================
// MVC SUPPORT (Controllers + Views)
// ======================================
builder.Services.AddControllersWithViews();

//
// ======================================
// 🔥 HTTP CLIENT (OPTIONAL FOR APIS)
// ======================================
builder.Services.AddHttpClient();

//
// ======================================
// 📧 EMAIL SERVICE (GMAIL OTP)
// ======================================
builder.Services.AddScoped<EmailService>();

//
// ======================================
// SESSION SUPPORT (LOGIN + OTP SYSTEM)
// ======================================
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // session expires after 30 min
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

//
// ======================================
// ERROR HANDLING + SECURITY
// ======================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//
// ======================================
// MIDDLEWARE PIPELINE ORDER (IMPORTANT)
// ======================================

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//
// 🔥 SESSION MUST BE BEFORE AUTH
//
app.UseSession();

app.UseAuthorization();

//
// ======================================
// DEFAULT ROUTE
// ======================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run(); ;