using System.Reflection;
using System.Text;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SoftwareMind.Core.Exceptions;
using SoftwareMind.Core.Helpers;
using SoftwareMind.Core.IoC;
using SoftwareMind.Dal;
using SoftwareMind.Dal.IoC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddOData(options => options.Select().Filter().OrderBy().SkipToken().SetMaxTop(50));
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API", Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
var tokenKey = builder.Configuration.GetValue<string>("TokenKey");
var key = Encoding.ASCII.GetBytes(tokenKey);
builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddSingleton<ITokenRefresher>(x =>
    new TokenRefresher(key, x.GetService<IJwtAuthenticationManager>()));
builder.Services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddSingleton<IJwtAuthenticationManager>(x =>
    new JwtAuthenticationManager(tokenKey, x.GetService<IRefreshTokenGenerator>()));

builder.Services.AddDal();
builder.Services.AddCore();
builder.Services.AddProblemDetails(SetupProblemDetails);

var app = builder.Build();

await SeedDatabase(app.Services);

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

app.UseProblemDetails();

app.Run();

async Task SeedDatabase(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.EnsureDeletedAsync();
    await context.Database.EnsureCreatedAsync();
}

static ProblemDetails MapException(HttpContext ctx, MappedException ex)
{
    var problemFactory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
    return ex.ToProblemDetails(ctx, problemFactory);
}

void SetupProblemDetails(ProblemDetailsOptions options)
{
    options.IncludeExceptionDetails = (_, _) => builder.Environment.IsDevelopment();
    options.Map(new Func<HttpContext, MappedException, ProblemDetails>(MapException));
    options.Rethrow<NotSupportedException>();
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
}

public partial class Program
{
}