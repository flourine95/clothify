namespace clothify_api.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserAddressId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        public void AddItem(Product product, int quantity, string size)
        {
            var cartItem = new CartItem
            {
                ProductId = product.Id,
                Quantity = quantity,
                Size = size, // Lưu kích thước
                Price = product.Price
            };

            Items.Add(cartItem); // Thêm vào danh sách các sản phẩm trong giỏ hàng
        }


        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
                TotalAmount = Items.Sum(i => i.Price);
            }
        }

        public void UpdateQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
                item.Price = item.Product.Price * quantity;
                item.UpdatedAt = DateTime.Now;

                TotalAmount = Items.Sum(i => i.Price);
            }
        }

        public decimal GetTotalPrice()
        {
            return TotalAmount;
        }
    }
}
