using Aeternum.Entities.User;
using Aeternum.Middlewares.Authorization;
using Aeternum.Profiles;
using Aeternum.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrace služeb
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();

// Pøidání DbContextu a Identity služeb
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Pøidání služeb AutoMapperu do kontejneru DI
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Pøidání Identity služeb
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Možnosti pro nastavení identity (napø. požadavky na heslo)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Pøidání autentizace a autorizace
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(); // Use default cookie scheme without specifying the scheme explicitly

// Pøidání autorizace
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin").RequireClaim("Permission", "CanManageUsers"));

    options.AddPolicy("ManagerPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Permission" && c.Value == "CanManageProjects") &&
            context.User.IsInRole("Manager")));
});

// Pøidání kontrolerù
builder.Services.AddControllers(); // Pøidání této øádky

// Pøidání Swaggeru
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Konfigurace HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ujistìte se, že autentizace a autorizace middleware jsou na správných místech
app.UseAuthentication();  // Pøidání autentizace
app.UseMiddleware<AuthorizationMiddleware>(); // Vlastní middleware pro autorizaci
app.UseAuthorization();   // Pøidání autorizace

app.MapControllers(); // Zaregistrujte kontrolery

app.Run();
