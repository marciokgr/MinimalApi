using MinimalApi.Data;
using MinimalApi.ViewModels;
//using MinimalApi.Models;

//Criando projeto WEB
var builder = WebApplication.CreateBuilder(args);

//Instanciando a conexão com banco de dados.
builder.Services.AddDbContext<AppDbContext>();

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Instancia do APP
var app = builder.Build();


//Verbos
app.MapGet("v1/todos", (AppDbContext context) =>
{
    var todos = context.Todos;
    return todos is not null ? Results.Ok(todos) : Results.NotFound();
});

//Recebendo Post
app.MapPost("/v1/todos", (AppDbContext context, CreateTodoViewModel model) =>
{
    var todo = model.MapTo();
    //Valida o objeto
    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    //Criando objeto no banco de dados
    context.Todos.Add(todo);
    
    //Salva no banco de dados.
    context.SaveChanges();

    return Results.Created($"/v1/todos/{todo.Id}", todo);
});

//Swagger
app.UseSwagger();

//Definindo padrão URL
app.UseSwaggerUI(s => {
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha Minimal API");
    s.RoutePrefix = "swagger";
});


//Executando o APP
app.Run();
