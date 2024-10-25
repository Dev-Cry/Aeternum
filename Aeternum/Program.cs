using Aeternum.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Aeternum.Entities.User;
using Aeternum.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<RoleService>();

        // Add services to the container.
        builder.Services.AddControllers();

        // P�id�n� DbContextu a Identity slu�eb
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // P�idej vlastn� p�ipojovac� �et�zec

        // P�id�n� slu�eb AutoMapperu do kontejneru DI
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        // P�id�n� autentizace a autorizace
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
            options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
        })
        .AddCookie(IdentityConstants.ApplicationScheme);

        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();  // P�id�n� autentizace
        app.UseAuthorization();   // P�id�n� autorizace

        app.MapControllers();

        app.Run();
    }
}
