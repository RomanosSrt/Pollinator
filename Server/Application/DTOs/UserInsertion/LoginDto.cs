namespace API.Application.DTOs.UserInsertion
{
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public LoginResponseDto(string token)
        {
            this.Token = token;
        }
    }
}
