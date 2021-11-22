using Flunt.Notifications;
using Flunt.Validations;
using MinimalApi.Models;

namespace MinimalApi.ViewModels
{
    public class CreateUsuarioViewModel : Notifiable<Notification>
    {
        public string ?Login { get; set; }

        public string? Senha { get; set; }

        public Usuario MapTo()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Login, "Informe o login")
                .IsGreaterThan(Login, 5, "O login deve conter mais de 5 caracteres"));

            AddNotifications(new Contract<Notification>()
               .Requires()
               .IsNotNull(Senha, "Informe a senha")
               .IsGreaterThan(Senha, 5, "A senha deve conter mais de 5 caracteres"));

            return new Usuario()
            {
                Id = Guid.NewGuid(),
                Login = Login,
                Senha = Senha
            };
        }
    }
}
