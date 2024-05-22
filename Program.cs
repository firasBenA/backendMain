global using Microsoft.EntityFrameworkCore;
global using System.Collections.Generic;
global using TestApi.Models;
global using TestApi.Repositories;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using System.Text;
global using Microsoft.Extensions.Configuration;
using TestApi.Configurations;
using Microsoft.AspNetCore.Identity;
using TestApi.Helpers;
using API.FileProcessing.Service;
using JwtAuthAspNet7WebAPI.Core.DbContext;
using JwtAuthAspNet7WebAPI.Core.Entities;
using Braintree;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using ConversationApi.Repositories;
using UserConversationApi.Repositories;
using EmailApi.Repositories;
using PrivateChatApp.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    options.UseSqlServer(connectionString);
});

builder.Services.AddSingleton<IBraintreeGateway>(provider =>
        {
            return new BraintreeGateway
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "q5g7crgx9xvybygy",
                PublicKey = "bjz6c3qw27fwvwjr",
                PrivateKey = "05d4d682103f51947bca38f69132955a"
            };
        });
// Add services to the container.
builder.Services.AddDbContextPool<AuthContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

//builder.Services.AddSingleton<IConfiguration>(builder.Configuration); // Inject builder.Configuration
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBoatRepository, BoatRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IManageImage, ManageImage>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessagesRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IFeedBackRepository, FeedBackRepository>();
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<IUserConversationRepository, UserConversationRepository>();
builder.Services.AddTransient<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddScoped<EmailService>();





builder.Services.AddSignalR();


builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<AuthContext>()
        .AddDefaultTokenProviders();

builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(jwt =>
{
    var secret = builder.Configuration.GetSection("JWT:Key").Value;
    if (string.IsNullOrEmpty(secret))
    {
        throw new InvalidOperationException("JWT secret is missing or empty in appsettings.json.");
    }

    var key = Encoding.ASCII.GetBytes(secret);

    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = false,
        RequireExpirationTime = false
    };
});



builder.Services.AddMvc();

builder.WebHost.UseUrls("http://localhost:5013");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("chatHub"); // Map SignalR hub endpoint
});

app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
