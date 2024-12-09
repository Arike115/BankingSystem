namespace BankingSystem.Domain.Model
{
    public class JsonWebToken
    {
        public string Token { get; set; }
        public long Expires { get; set; }
        public string UserType { get; set; }
        public string IsRegistrationVerified { get; set; }
        public string ProfileCode { get; set; }
        public string FullName { get; set; }
        public string ProfileId { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public string ReferalCode { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string Skills { get; set; }
        public string Interests { get; set; }
        public bool Islogin { get; set; }
    }

    public class SimpleJsonWebToken
    {
        public string Token { get; set; }
        public long Expires { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
