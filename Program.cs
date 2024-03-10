using System.Text;
using BankProject;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.Configure<ApiBehaviorOptions>(x => x.InvalidModelStateResponseFactory = ctx => new ValidationProblemDetailsResult());

builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
    {
        new OpenApiSecurityScheme
        {
        Reference = new OpenApiReference
            {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
            },
            Scheme = "oauth2",
            Name = "Bearer",
            In = ParameterLocation.Header,

        },
        new List<string>()
        }
    });
});

builder.Services.AddDbContext<BankAppContext>(options => {
    options.UseSqlite(builder.Configuration.GetConnectionString("BankDB"));
});

builder.Services.AddScoped<IOperationsService, OperationsService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAssignorRepository, AssignorRepository>();
builder.Services.AddScoped<IPayableRepository, PayableRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();

byte[] key = Encoding.ASCII.GetBytes(Settings.SecretJWTToken);

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
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

app.UseAuthorization();

app.MapControllers();

app.Run();
