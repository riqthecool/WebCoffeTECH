using System.Text.Json.Serialization;

namespace WebCafe.Models
{
    public class Endereco
    {
        public int Id { get; set; }
        public int ContaId { get; set; } // Chave estrangeira para o usuário

        [JsonPropertyName("rua")]
        public string Rua { get; set; } = string.Empty;

        [JsonPropertyName("cep")]
        public string CEP { get; set; } = string.Empty;

        [JsonPropertyName("numero")]
        public string Numero { get; set; } = string.Empty;

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; } = string.Empty;

        [JsonPropertyName("cidade")]
        public string Cidade { get; set; } = string.Empty;

        [JsonPropertyName("conta")]
        public Conta? Conta { get; set; } // Propriedade de navegação (opcional)
    }
}
