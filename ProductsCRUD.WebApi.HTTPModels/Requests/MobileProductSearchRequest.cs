namespace ProductsCRUD.WebApi.HTTPModels.Requests
{
    public class MobileProductSearchRequest : PaginationRequest
    {
        public string Search { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsNew { get; set; }
    }
}
