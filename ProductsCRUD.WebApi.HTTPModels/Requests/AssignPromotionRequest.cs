namespace ProductsCRUD.WebApi.HTTPModels.Requests
{
    public class AssignPromotionRequest
    {
        public int ProductId { get; set; }
        public int PromotionId { get; set; }
    }
}
