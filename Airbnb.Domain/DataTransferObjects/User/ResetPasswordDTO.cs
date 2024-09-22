namespace Airbnb.Domain.DataTransferObjects.User
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string Otp { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string PasswordComfirmation { get; set; }
    }
}
