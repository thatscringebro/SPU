using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Hubs;
using SPU.Domain;
using SPU.Domain.Entites;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddDbContext<SpuContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("defaultConnection")));
builder.Services.AddIdentity<Utilisateur, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<SpuContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); //Mettre le login au lieu de l'index quand on va être prêts pour Identity
app.MapHub<ChatHub>("/chatHub");

app.Run();
