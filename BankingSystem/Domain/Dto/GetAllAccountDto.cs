
namespace BankingSystem.Domain.Dto
{
    public class GetAllAccountDto
    {
        public string AccountName { get; set; }
        public long AccountNumber { get; set; }   
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public double Balance {  get; set; }
        public DateTime? CreatedOn { get; internal set; }
        public Guid AccountId { get; internal set; }
    }
}
