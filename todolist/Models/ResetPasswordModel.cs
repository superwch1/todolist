namespace todolist.Models
{
	public class ResetPasswordModel
	{
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ResetToken { get; set; }
    }
}