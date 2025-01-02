namespace ProductsCRUD.WebApi.HTTPModels.Responses
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? Calories { get; set; }
        public bool IsFeatured { get; set; }
    }
}
