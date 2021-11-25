namespace MinimalApi.Models
{
    public class Usuario
    {
        public Guid Id { get; set; }
                
        public string ?Login { get; set; }

        public string ?Senha { get; set; }
    }
}
