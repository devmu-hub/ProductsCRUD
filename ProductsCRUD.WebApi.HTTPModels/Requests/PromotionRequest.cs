namespace ProductsCRUD.WebApi.HTTPModels.Requests
{
    public class PromotionRequest
    {
        public int Id { get; set; }
        public int PromotionTypeId { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
