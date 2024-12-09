
namespace BankingSystem.Domain.Dto
{
    public class GetAllAccountDto
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }   
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; internal set; }
    }
}
