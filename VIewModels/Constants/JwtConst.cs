namespace ViewModels.ants
{
    public class JwtModel
    {
        public  string Secret { get; set; }
        public  string Issuer { get; set; }
        public  string Audience { get; set; }
        public  int ExpiryMinutes { get; set; }
        public  int RefreshExpiration { get; set; }
        public  string UserId { get; set; }
        public  string RoleId { get; set; }
    }
}
