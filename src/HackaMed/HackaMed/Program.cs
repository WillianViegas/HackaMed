using Amazon.S3;
using Application.UseCases;
using Application.UseCases.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Infra.DatabaseConfig;
using Infra.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());
});


builder.Services.AddSingleton<MongoClient>(_ => new MongoClient());
builder.Services.AddSingleton<IMongoDatabase>(provider => provider.GetRequiredService<MongoClient>().GetDatabase("HackaMed"));
builder.Services.AddSingleton<IMongoCollection<Usuario>>(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<Usuario>("Usuario"));
builder.Services.AddSingleton<IMongoCollection<Agenda>>(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<Agenda>("Agenda"));
builder.Services.AddSingleton<IMongoCollection<Consulta>>(provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<Consulta>("Consulta"));

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IAgendaRepository, AgendaRepository>();
builder.Services.AddTransient<IConsultaRepository, ConsultaRepository>();
builder.Services.AddTransient<IProntuarioRepository, ProntuarioRepository>();

builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection(nameof(DatabaseConfig)));
builder.Services.AddSingleton<IDatabaseConfig>(sp => sp.GetRequiredService<IOptions<DatabaseConfig>>().Value);
builder.Services.AddTransient<IAmazonS3, AmazonS3Client>();

builder.Services.AddTransient<IUsuarioUseCase, UsuarioUseCase>();
builder.Services.AddTransient<IAgendaUseCase, AgendaUseCase>();
builder.Services.AddTransient<IConsultaUseCase, ConsultaUseCase>();
builder.Services.AddTransient<ILoginUseCase, LoginUseCase>();
builder.Services.AddTransient<IPacienteUseCase, PacienteUseCase>();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "HackaMed", Version = "v1" });

    // Adicione a configuração do JWT
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

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
            new string[] {}
        }
    });
});


var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

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
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UsePathBase(builder.Configuration["App:Pathbase"]);
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
