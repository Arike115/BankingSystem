namespace BankingSystem.Configs
{
    public class Authsettings
    {
        public double TokenExpiry { get; set; }
        public string? Password { get; set; }
        public string? Issuer { get; set; }
        public string? SecretKey { get; set; }
        public bool RequireHttps { get; set; }
    }
}
