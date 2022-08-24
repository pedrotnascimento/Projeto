
using BusinessRule.Interfaces;
using BusinessRule.Services;
using Application.AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Repositories;
using Repository.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Repository.Models;
using Application;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Application.Authorization;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DatabaseContextConnection") ?? throw new InvalidOperationException("Connection string 'DatabaseContextConnection' not found.");

// Add services to the container.
services.AddScoped<IChatRoom, ChatRoomService>();
services.AddScoped<IMessage, MessageService>();
services.AddScoped<IMessageRepository, MessageRepository>();
services.AddScoped<IChatRoomRepository, ChatRoomRepository>();
services.AddScoped<IAuthentication, Authentication>();
services.AddScoped<IIdentityManager, IdentityManager>();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDatabaseContext>()
    .AddDefaultTokenProviders();

var signingConfigurations = new SigningConfigurations();
services.AddSingleton(signingConfigurations);

var tokenConfigurations = new TokenConfigurations();
new ConfigureFromConfigurationOptions<TokenConfigurations>(
    configuration.GetSection("TokenConfigurations"))
        .Configure(tokenConfigurations);
services.AddSingleton(tokenConfigurations);

builder.Services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((host) => true)
                   .AllowCredentials();
        }));



services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearerOptions =>
{
    var paramsValidation = bearerOptions.TokenValidationParameters;
    paramsValidation.IssuerSigningKey = signingConfigurations.Key;
    paramsValidation.ValidAudience = tokenConfigurations.Audience;
    paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

    // validates signature of a given tokne
    paramsValidation.ValidateIssuerSigningKey = true;

    // Verifies if a token is still valid
    paramsValidation.ValidateLifetime = true;

    // Tolerance expiring time for token
    // (used when has time difference between computers)
    paramsValidation.ClockSkew = TimeSpan.Zero;
});

services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});


# region Database
var connection = configuration.GetSection("ConnectionStrings").GetValue<string>("Sqlite");
services.AddDbContext<AppDatabaseContext>(options =>
    options.UseSqlite(connection)
);

//services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<DatabaseContext>();

#endregion


#region AutoMapper

services.AddAutoMapper(
    typeof(DTO_BRProfileMapper),
    typeof(BR_DALProfileMapper),
    typeof(DAL_ModelProfileMapper)
    );
#endregion

var app = builder.Build();

app.UseCors("CorsPolicy");  

SetPreConfiguredData(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();



static void SetPreConfiguredData(WebApplication app)
{

    using (var scope = app.Services.CreateScope())
    {

        var identityManager = scope.ServiceProvider.GetService<IIdentityManager>();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
        identityManager.CreateRoles(roleManager);
    }
}