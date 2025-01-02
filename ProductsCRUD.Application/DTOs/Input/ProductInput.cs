namespace ProductsCRUD.Application.DTOs.Input
{
    public class ProductInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsFeatured { get; set; }
    }
}
