using AnyProduct.Orders.Api.Extensions;
using AnyProduct.Orders.Api.Services;
using AnyProduct.Orders.Application.Behaviours;
using AnyProduct.Orders.Application.Commands.Order;
using AnyProduct.Orders.Application.DomainEventHandlers;
using AnyProduct.Orders.Application.IntegrationEventHandlers;
using AnyProduct.Orders.Application.IntegrationEvents;
using AnyProduct.Orders.Application.Services;
using AnyProduct.Orders.Application.Validators.Basket;
using AnyProduct.Orders.Domain.Events;
using AnyProduct.Orders.Domain.Repositories;
using AnyProduct.Orders.Domain.Services;
using AnyProduct.Orders.Infrastructure;
using AnyProduct.Orders.Infrastructure.Repositories;
using AnyProduct.Orders.Infrastructure.Services;
using AnyProduct.OutBox.EF.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using AnyProduct.OutBox.EF;
using AnyProduct.Inbox.EF;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OpenTelemetry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.DocInclusionPredicate((docName, description) => true);
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {

        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Description = "Enter JWT token only"
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                  },
                  Scheme = JwtBearerDefaults.AuthenticationScheme,
                  Name = "Bearer",
                  In = ParameterLocation.Header,

                },
                Array.Empty<string>()
            }
    });
});

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = true;
#pragma warning disable CA5404 // Do not disable token validation checks
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = false,
            ValidateLifetime = false,
            RequireExpirationTime = false,
            SignatureValidator = delegate (string token, TokenValidationParameters parameters)
            {
                var jwt = new JsonWebToken(token);

                return jwt;
            },

        };
#pragma warning restore CA5404 // Do not disable token validation checks
    });

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("AnyProduct.Orders"))
    .WithMetrics(metrics =>
    {
        metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation();

        metrics.AddOtlpExporter();

    })
    .WithTracing(traces =>
    {
        if (builder.Environment.IsDevelopment())
        {
            traces.SetSampler(new AlwaysOnSampler());
        }

        traces
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation();

        traces.AddOtlpExporter();
    });

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeScopes = true;
    logging.IncludeFormattedMessage = true;
    logging.AddOtlpExporter();
});

builder.Services.AddDbContext<OrderContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(PlaceOrderCommand));
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

builder.Services.AddSingleton<IFileService, LocalFileService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPaymentGatewayService, PaymentGatewayService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddTransient<IIntegrationEventService, IntegrationEventService>();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
builder.AddOutbox<OrderContext>(typeof(IIntegrationEventService).Assembly);
builder.AddInbox<OrderContext>();

builder.AddKafkaEventBus()
       .AddSubscription<OrderShippedIntergationEvent, OrderShippedintegrationEventHandler>()
       .AddSubscription<OrderStockConfirmedIntergationEvent, OrderStockConfirmedIntergationEventHandler>()
       .AddSubscription<OrderStockRejectedIntergationEvent, OrderStockRejectedIntergationEventHandler>()
       .ConfigureJsonOptions();

builder.Services.AddValidatorsFromAssemblyContaining<PlaceOrderCommandValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.EnsureMigrations();

#pragma warning disable S6966 // Awaitable method should be used
app.Run();
#pragma warning restore S6966 // Awaitable method should be used
