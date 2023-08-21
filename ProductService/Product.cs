namespace ProductService
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

        public Product(int Id, string Name, string Type, string Status)
        {
            this.Id = Id;
            this.Name = Name;
            this.Type = Type;
            this.Status = Status;
        }
    }
}