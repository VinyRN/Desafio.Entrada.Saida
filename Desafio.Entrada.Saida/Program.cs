using Desafio.Entrada.Saida.Dominio.Interfaces;
using Desafio.Entrada.Saida.Servico;
using Desafio.Entrada.Saida.Infraestrutura.Repositorios.Repository;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using Desafio.entrada.saida.Dominio;
using Desafio.Entrada.Saida.Queue;
using Desafio.Entrada.Saida.Queue.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configurar o Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Console()
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        AutoRegisterTemplate = true,
        IndexFormat = "logs-desafio-entrada-saida-{0:yyyy.MM}"
    })
    .CreateLogger();

// Configurar o builder para usar Serilog
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();
// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar serviços e repositórios
builder.Services.AddScoped<IEmbalagemService, EmbalagemService>();
builder.Services.AddScoped<IRepositorioCaixa, RepositorioCaixa>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IQueueConsumer, QueueConsumer>();

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

// Iniciar a aplicação com o Serilog configurado
try
{
    Log.Information("Iniciando a aplicação");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar");
    throw;
}
finally
{
    Log.CloseAndFlush();
}
