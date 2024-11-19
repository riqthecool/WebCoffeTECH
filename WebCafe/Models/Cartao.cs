using System.Text.Json.Serialization;

namespace WebCafe.Models
{
    public class Cartao
    {
        public int Id { get; set; }

        [JsonPropertyName("contaId")]
        public int ContaId { get; set; }

        [JsonPropertyName("numeroCartao")]
        public string NumeroCartao { get; set; } = string.Empty;

        [JsonPropertyName("validade")]
        public string Validade { get; set; } = string.Empty;

        [JsonPropertyName("cvv")]
        public string CVV { get; set; } = string.Empty;

        [JsonIgnore] // Ignora a propriedade Conta ao serializar para JSON
        public Conta? Conta { get; set; }
    }
}
