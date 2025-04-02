using ChatApp.Data;
using ChatApp.Hubs;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR().AddHubOptions<ChatHub>(o =>
{
    o.EnableDetailedErrors = true;
});
builder.Services.AddDbContext<ChatDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("ChatHubContext"));
});
builder.Services.AddIdentity<ChatUser, IdentityRole>()
    .AddEntityFrameworkStores<ChatDbContext>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureApplicationCookie(o =>
{
    o.LoginPath = "/Account/Login";
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
IdentitySeedData.SeedData(app);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chatHub", o =>
{
    o.ApplicationMaxBufferSize = 512 * 1024;
    o.TransportMaxBufferSize = 512 * 1024;
    o.LongPolling.PollTimeout = TimeSpan.FromSeconds(10);
});
app.Run();
