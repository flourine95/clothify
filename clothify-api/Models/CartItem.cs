namespace clothify_api.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } // Liên kết đến sản phẩm
        public int Quantity { get; set; } // Số lượng của sản phẩm trong giỏ
        public decimal TotalPrice => Product.Price * Quantity; // Tính tổng giá trị của sản phẩm
    }
}