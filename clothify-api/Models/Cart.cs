namespace clothify_api.Models

{
    public class Cart
    {
        public int UserId { get; set; } // ID của người dùng
        public List<CartItem> Items { get; set; } = new List<CartItem>(); // Danh sách các sản phẩm trong giỏ hàng

        // Thêm sản phẩm vào giỏ
        public void AddItem(Product product, int quantity)
        {
            var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity; // Tăng số lượng nếu sản phẩm đã có trong giỏ
            }
            else
            {
                Items.Add(new CartItem { ProductId = product.Id, Product = product, Quantity = quantity });
            }
        }

        // Xóa sản phẩm khỏi giỏ
        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        // Tính tổng giá trị của giỏ hàng
        public decimal GetTotalPrice()
        {
            return Items.Sum(item => item.TotalPrice);
        }

        // Cập nhật số lượng sản phẩm trong giỏ
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
            }
        }
    }
}