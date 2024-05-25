using API.Data;
using API.Hubs;
using API.Services.AgenceService;
using API.Services.AuthService;
using API.Services.CarteService;
using API.Services.CommunicationService;
using API.Services.EventCarteConfigurationService;
using API.Services.EventCreditConfigurationService;
using API.Services.HangFireServices.CartEventSenderService;
using API.Services.HangFireServices.CreditEventSenderService;
using API.Services.HangFireServices.CreditPaymentService;
using API.Services.HangFireServices.NotificationSenderService;
using API.Services.MessageModelService;
using API.Services.MessageService;
using API.Services.NotificationService;
using API.Services.UserService;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins, builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();

    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHangfire(configuration => configuration
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangFireConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICarteService, CarteService>();
builder.Services.AddScoped<IEventCarteConfigurationService, EventCarteConfigurationService>();
builder.Services.AddScoped<IEventCreditConfigurationService, EventCreditConfigurationService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IAgenceService, AgenceService>();

builder.Services.AddTransient<ICommunicationService, CommunicationService>();
builder.Services.AddTransient<ICartEventSenderService, CartEventSenderService>();
builder.Services.AddTransient<INotificationSenderService, NotificationSenderService>();
builder.Services.AddTransient<ICreditEventSenderService, CreditEventSenderService>();
builder.Services.AddTransient<ICreditPaymentService, CreditPaymentService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenKey = builder.Configuration["AppSettings:Token"] ?? throw new InvalidOperationException("secret key is not found");
        // Retrieve the token key from appsettings.json

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false, // Set to true if you want to validate the issuer
            ValidateAudience = false // Set to true if you want to validate the audience
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(myAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();
app.MapHangfireDashboard();
app.MapHub<ChatHub>("/chathub");

RecurringJob.AddOrUpdate<ICartEventSenderService>(
    "SendingCarteEvent",
    x => x.SendingCarteEventAsync(),
    Cron.Minutely);

RecurringJob.AddOrUpdate<INotificationSenderService>(
    "SendingNotifications",
    x => x.SendingNotificationsAsync(),
    Cron.Minutely);

RecurringJob.AddOrUpdate<ICreditEventSenderService>(
    "SendingCreditEvent",
    x => x.SendingCreditEventAsync(),
    Cron.Minutely);

RecurringJob.AddOrUpdate<ICreditPaymentService>(
    "ProcessCreditPaymentsAsync",
    x => x.ProcessCreditPaymentsAsync(),
    Cron.Minutely);

app.Run();
