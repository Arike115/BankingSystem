namespace BankingSystem.Domain.Model
{
    public class ErrorResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string TraceId { get; set; }
        public int? ErrorCode { get; set; }
    }
}
