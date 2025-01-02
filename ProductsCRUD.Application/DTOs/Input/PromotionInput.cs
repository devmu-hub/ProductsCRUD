namespace ProductsCRUD.Application.DTOs.Input
{
    public class PromotionInput
    {
        public int Id { get; set; }
        public int PromotionTypeId { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
