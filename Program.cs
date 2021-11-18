using MinimalApi.Data;
using MinimalApi;

//Criando projeto WEB
var builder = WebApplication.CreateBuilder(args);

//Instanciando a conexão com banco de dados.
builder.Services.AddDbContext<AppDbContext>();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Instancia do APP
var app = builder.Build();

//Criando as rotas da API em classe separada
app.MapMinimalRoutes();

//Swagger
app.UseSwagger();

//Definindo padrão URL
app.UseSwaggerUI(s => {
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    s.RoutePrefix = "swagger";
});


//Executando o APP
app.Run();
