namespace FirmezaPro.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public decimal Price { get; private set; }
        public int Stock { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        private Product()
        {
        } // EF Core

        public Product(string name, string description, decimal price, int stock, bool isActive)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            IsActive = isActive;
        }

        public void Edit(string name, string description, decimal price, int stock, bool isActive)
        {
            Name = name;
            Description = description;
            Price = price;
            Stock = stock;
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}