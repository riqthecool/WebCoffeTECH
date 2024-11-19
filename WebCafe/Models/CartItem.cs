namespace WebCafe.Models
{
    public class CartItem
    {
        public string? UniqueId { get; set; }  // Combinação de ID e variante
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? ImageUrl { get; set; }
        public string? Variant { get; set; }  // Exemplo: tamanho, cor, etc.
    }



}
