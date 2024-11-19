namespace WebCafe.Models
{
    public class PedidoViewModel
    {
        public int Id { get; set; } // Não será usado na criação, mas pode ser necessário em outras funcionalidades
        public int ContaId { get; set; } // Associar ao usuário logado
        public string EnderecoEntrega { get; set; } = string.Empty; // Preenchido pelo formulário
        public string TipoPagamento { get; set; } = string.Empty; // Preenchido pelo formulário
        public string ResumoPedido { get; set; } = string.Empty; // Pode ser calculado no backend da API
        public string StatusPedido { get; set; } = "Pendente"; // Status padrão inicial
        public string NotaFiscal { get; set; } // Gerado pela API
        public DateTime? DataCriacao { get; set; } = DateTime.UtcNow; // Definido automaticamente
        public int QuantidadeItens { get; set; } // Preenchido pelo formulário
        public decimal ValorTotal { get; set; } // Preenchido pelo formulário
    }
}
