namespace Order
{
    public class CreateOrder
    {
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }

        public CreateOrder(int Price, int Quantity, int ProductId, string ProductName, string Status)
        {
            this.Price = Price;
            this.Quantity = Quantity;
            this.ProductId = ProductId;
            this.ProductName = ProductName;
            this.Status = Status;
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }

        public Order(int Id, int Price, int Quantity, int ProductId, string ProductName, string Status)
        {
            this.Id = Id;
            this.Price = Price;
            this.Quantity = Quantity;
            this.ProductId = ProductId;
            this.ProductName = ProductName;
            this.Status = Status;
        }
    }
}