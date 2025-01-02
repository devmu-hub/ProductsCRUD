namespace ProductsCRUD.WebApi.Settings
{
    public class JwtTokenSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SigningKey { get; set; }
    }
}
