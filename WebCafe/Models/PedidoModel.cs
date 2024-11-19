public class PedidoModel
{
    public int Id { get; set; }
    public string EnderecoEntrega { get; set; } = string.Empty;
    public string TipoPagamento { get; set; } = string.Empty;
    public string ResumoPedido { get; set; } = string.Empty;
    public string StatusPedido { get; set; } = string.Empty;
    public string NotaFiscal { get; set; } = string.Empty;
    public DateTime? DataCriacao { get; set; }
    public int QuantidadeItens { get; set; }
    public decimal ValorTotal { get; set; }
}
