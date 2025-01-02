namespace ProductsCRUD.Application.DTOs.Input
{
    public class MobileProductSearchInput : PaginationInput
    {
        public string Search { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsNew { get; set; }
    }
}
