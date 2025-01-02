namespace ProductsCRUD.Application.DTOs.Output
{
    public class PromotionOutput
    {
        public int Id { get; set; }
        public int PromotionTypeId { get; set; }
        public string PromotionType { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
