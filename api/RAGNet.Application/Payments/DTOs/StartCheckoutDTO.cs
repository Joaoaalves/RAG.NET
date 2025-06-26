namespace RAGNET.Application.Payments.DTOs
{
    public class StartCheckoutDTO
    {
        public string SuccessUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
    }
}