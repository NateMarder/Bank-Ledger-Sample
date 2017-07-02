namespace ConsoleBanking.Models
{
    public class LoginFromConsoleViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterNewUserViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string HomeTown { get; set; }
    }
}