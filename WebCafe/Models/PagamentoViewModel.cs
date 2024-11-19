using WebCafe.Models;

public class PagamentoViewModel
{
    public List<Endereco> Enderecos { get; set; }
    public List<Cartao> Cartoes { get; set; }
    public List<CartItem> ItensCarrinho { get; set; }
    public decimal Total { get; set; }
}
