using AnyProduct.OutBox.EF.Services;
using AnyProduct.Products.Api.Extensions;
using AnyProduct.Products.Application.Behaviours;
using AnyProduct.Products.Application.Commands;
using AnyProduct.Products.Application.IntegrationEvents;
using AnyProduct.Products.Application.Services;
using AnyProduct.Products.Domain.Repositories;
using AnyProduct.Products.Domain.Services;
using AnyProduct.Products.Infrastructure;
using AnyProduct.Products.Infrastructure.Repositories;
using AnyProduct.Products.Infrastructure.Services;
using eShop.Ordering.API.Application.IntegrationEvents;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AnyProduct.Products.Application.IntegrationEventHandlers;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.JsonWebTokens;
using AnyProduct.OutBox.EF;
using AnyProduct.Inbox.EF;
using OpenTelemetry.Resources;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using OpenTelemetry.Logs;

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
    });



builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("AnyProduct.Products"))
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


builder.Services.AddDbContext<ProductContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(CreateProductCommand));
    cfg.AddOpenBehavior(typeof(ValidatorBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});


builder.Services.AddSingleton<IFileService, LocalFileService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IIntegrationEventService, IntegrationEventService>();
builder.Services.AddTransient<IDateTimeProvider, DateTimeProvider>();
builder.AddOutbox<ProductContext>(typeof(IIntegrationEventService).Assembly);
builder.AddInbox<ProductContext>();

builder.AddKafkaEventBus()
       .AddSubscription<OrderPaidIntergationEvent, OrderPaidIntergationEventHandler>()
       .AddSubscription<OrderStartedIntegrationEvent, OrderStartedIntergationEventHandler>()
       .ConfigureJsonOptions();

builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "UploadedFiles")),
    RequestPath = "/UploadedFiles"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.EnsureMigrations();

app.Run();
