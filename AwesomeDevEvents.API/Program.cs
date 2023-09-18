using AwesomeDevEvents.API.Mappers;
using AwesomeDevEvents.API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DevEventsCs");

//builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseInMemoryDatabase("DevEventsDb")); // guarda em um banco de dados em memória
builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseSqlServer(connectionString)); // guarda no banco de dados SQLServer

//Está registrando o AutoMapper como um serviço no contêiner de injeção de dependência e configurando-o para usar perfis de mapeamento específicos
builder.Services.AddAutoMapper(typeof(DevEventProfile).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo   // configuração do swagger para gerar a documentação
    {
        Title = "AwesomeDevEvents.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Adriana",
            Email = "adriana@mail.com",
            Url = new Uri("https://adriana.com.br")
        }
    });
    var xmlFile = "AwesomeDevEvents.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
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
