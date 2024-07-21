using Domain.Entities;
using Domain.Repositories;
using Infra.DatabaseConfig;
using Infra.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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

builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection(nameof(DatabaseConfig)));
builder.Services.AddSingleton<IDatabaseConfig>(sp => sp.GetRequiredService<IOptions<DatabaseConfig>>().Value);


builder.Services.AddControllers();
builder.Services.AddSwaggerGen(opts => opts.EnableAnnotations());

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");
app.UsePathBase(builder.Configuration["App:Pathbase"]);
app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
