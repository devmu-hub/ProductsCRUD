namespace ProductsCRUD.Application.DTOs.Output
{
    public class AdminServiceLayerListOutput<T>
    {
        public bool Success { get; set; }
        public bool IsExistException { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
        public IEnumerable<T> Data { get; set; }
        public int Count { get; set; }
    }
}
