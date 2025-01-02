namespace ProductsCRUD.Application.DTOs.Input
{
    public class ProductSearchInput : PaginationInput
    {
        public bool IsFeatured { get; set; }
    }
}
