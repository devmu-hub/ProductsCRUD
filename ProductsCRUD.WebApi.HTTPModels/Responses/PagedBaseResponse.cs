namespace ProductsCRUD.WebApi.HTTPModels.Responses
{
    public class PagedBaseResponse<T>
    {
        public IEnumerable<T> List { get; set; }
        public int Count { get; set; }
    }
}
