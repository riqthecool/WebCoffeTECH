namespace WebCafe.Models
{
    public class UserResponse
    {
        public string Message { get; set; } // Mensagem de retorno (opcional para usar)
        public int ContaId { get; set; }   // ID da conta do usuário
        public string NomeCompleto { get; set; } // Nome completo do usuário
        public string Email { get; set; }  // Email do usuário
    }
}
