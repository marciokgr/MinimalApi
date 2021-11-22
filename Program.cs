using MinimalApi.Data;
using MinimalApi;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MinimalApi.Services;

//Criando projeto WEB
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = "MinimalAPI",    
    ContentRootPath = Directory.GetCurrentDirectory(),
    EnvironmentName = Environments.Staging
});

//Instanciando a conexão com banco de dados.
builder.Services.AddDbContext<AppDbContext>();

//Configuração
var configuration = builder.Configuration;

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Service>();
builder.Services.AddSingleton<ITokenService>(new TokenService());

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
    };
});


//Instancia do APP
var app = builder.Build();

//Criando as rotas da API em classe separada
app.MapMinimalRoutes(configuration["Jwt:Key"], configuration["Jwt:Issuer"]);


//Swagger
app.UseSwagger();

//Definindo padrão URL
app.UseSwaggerUI(s => {
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    s.RoutePrefix = "swagger";
});

//Executando a aplicação
app.Run();