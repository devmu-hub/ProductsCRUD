namespace ProductsCRUD.Application.DTOs.Output
{
    public class AdminServiceLayerOutput<T>
    {
        public bool Success { get; set; }
        public bool IsExistException { get; set; }
        public IEnumerable<string> ErrorMessages { get; set; }
        public T Data { get; set; }

    }
}
