namespace WebCafe.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string CEP { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public DateTime? DataCriacao { get; set; }
    }
}
