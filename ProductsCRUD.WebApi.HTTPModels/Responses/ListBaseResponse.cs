namespace ProductsCRUD.WebApi.HTTPModels.Responses
{
    public class ListBaseResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
    }
}
