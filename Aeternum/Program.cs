using Aeternum.Entities.User;
using Aeternum.Middlewares.Authorization;
using Aeternum.Profiles;
using Aeternum.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Registrace slu�eb
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();

// P�id�n� DbContextu a Identity slu�eb
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// P�id�n� slu�eb AutoMapperu do kontejneru DI
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// P�id�n� Identity slu�eb
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Mo�nosti pro nastaven� identity (nap�. po�adavky na heslo)
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// P�id�n� autentizace a autorizace
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(); // Use default cookie scheme without specifying the scheme explicitly

// P�id�n� autorizace
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin").RequireClaim("Permission", "CanManageUsers"));

    options.AddPolicy("ManagerPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "Permission" && c.Value == "CanManageProjects") &&
            context.User.IsInRole("Manager")));
});

// P�id�n� kontroler�
builder.Services.AddControllers(); // P�id�n� t�to ��dky

// P�id�n� Swaggeru
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

// Ujist�te se, �e autentizace a autorizace middleware jsou na spr�vn�ch m�stech
app.UseAuthentication();  // P�id�n� autentizace
app.UseMiddleware<AuthorizationMiddleware>(); // Vlastn� middleware pro autorizaci
app.UseAuthorization();   // P�id�n� autorizace

app.MapControllers(); // Zaregistrujte kontrolery

app.Run();
