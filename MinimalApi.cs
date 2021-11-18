using MinimalApi.ViewModels;
using MinimalApi.Data;

namespace MinimalApi
{
    public static class MinimalApi
    {
        public static void MapMinimalRoutes(this IEndpointRouteBuilder app)
        {
            //GET dos Todos
            app.MapGet("v1/todos", (AppDbContext context) =>
            {
                var todos = context.Todos;
                return todos is not null ? Results.Ok(todos) : Results.NotFound();
            });

            //Salvando o Todo
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

            //Buscando todo by ID GUID
            app.MapGet("/todos/{id}", (Guid id, AppDbContext context) =>
            {
                var todo = context.Todos.Find(id);

                return todo is not null ? Results.Ok(todo) : Results.NotFound();
            });
        }
    }
}
