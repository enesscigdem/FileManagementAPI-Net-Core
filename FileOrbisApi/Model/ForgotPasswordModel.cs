namespace FileOrbisApi.Model
{
    public class ForgotPasswordModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string ResetToken { get; set; }

        public string NewPassword { get; set; }
    }
}
