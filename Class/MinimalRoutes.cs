using MinimalApi.ViewModels;
using MinimalApi.Data;
using MinimalApi.Services;
using Microsoft.AspNetCore.Authorization;
using MinimalApi.Models;

namespace MinimalApi
{
    public static class MinimalRoutes
    {
        public static void MapMinimalRoutes(this IEndpointRouteBuilder app, string jwtkey, string jwtIssuer)
        {
            //health check
            app.MapGet("v1/health", [AllowAnonymous] (Service myService) => new { Healthy = myService.Healthy });
         

            app.MapPost("v1/login", [AllowAnonymous] (AppDbContext context, CreateUsuarioViewModel model, HttpContext http, ITokenService tokenService) => 
            {
                var usuarios = context.Usuarios;
                if (usuarios.Count() == 0)
                {
                    return Results.NotFound();
                }
                var usuario = usuarios.FirstOrDefault(x => string.Equals(x.Login, model.Login) && string.Equals(x.Senha, model.Senha));

                if (usuario is null)
                {
                    return Results.NotFound();
                }
                var objUsuario = new Usuario
                {
                    Login = usuario.Login,
                    Senha = usuario.Senha
                };

                var token = tokenService.BuildToken(jwtkey, jwtIssuer, objUsuario);

                return Results.Ok(new { token = token });

            });

            //validar token..
            app.MapGet("/validatoken", (Func<string>)([Authorize]() => "Autorizado"));

            //GET dos Todos
            app.MapGet("v1/todos", [Authorize] (AppDbContext context) =>
            {
                var todos = context.Todos;
                return todos is not null ? Results.Ok(todos) : Results.NotFound();
            });

            //Salvando o Todo
            app.MapPost("v1/todos", [Authorize] (AppDbContext context, CreateTodoViewModel model) =>
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
            app.MapGet("v1/todos/{id}", [Authorize] (Guid id, AppDbContext context) =>
            {
                var todo = context.Todos.Find(id);
                return todo is not null ? Results.Ok(todo) : Results.NotFound();
            });

            //Setando todo como Feito
            app.MapPut("v1/todos/{id}", [Authorize] (Guid id, AppDbContext context) =>
            {
                var todo = context.Todos.Find(id);

                if (todo is null) return Results.NotFound();

                todo.Done = true;
                context.SaveChanges();

                return Results.Created($"/v1/todos/{todo.Id}", todo);
            });

            //
            app.MapPost("v1/Usuario", [AllowAnonymous] (AppDbContext context, CreateUsuarioViewModel model) =>
            {
                var usuario = model.MapTo();

                //Valida o objeto
                if (!model.IsValid)
                    return Results.BadRequest(model.Notifications);

                //Criando objeto no banco de dados
                usuario.Login = model.Login;
                usuario.Senha = model.Senha;
                context.Usuarios.Add(usuario);

                //Salva no banco de dados.
                context.SaveChanges();

                return Results.Created($"/v1/todos/{usuario.Id}", usuario);
            });
        }
    }
}
