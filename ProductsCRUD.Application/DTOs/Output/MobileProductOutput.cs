namespace ProductsCRUD.Application.DTOs.Output
{
    public class MobileProductOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<string> Promotions { get; set; }
    }
}
