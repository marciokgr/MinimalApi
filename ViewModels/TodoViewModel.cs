using Flunt.Notifications;
using Flunt.Validations;
using MinimalApi.Models;

namespace MinimalApi.ViewModels
{
    public class TodoViewModel : Notifiable<Notification>
    {
        public string? Title { get; set; }

        public Todo MapTo()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Title, "Informe o título da tarefa")
                .IsGreaterThan(Title, 5, "O título deve conter mais de 5 caracteres"));

            return new Todo()
            {
                Id = Guid.NewGuid(),
                Title = Title,
                Done = false
            };
        }
    }
}
