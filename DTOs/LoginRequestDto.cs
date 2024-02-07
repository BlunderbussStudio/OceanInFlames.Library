namespace OceansInFlame.Orchestration.API.DTOs
{
    public class LoginRequestDto
    {
        public required string Name { get; set; } // TODO - change Name to Email for login
        public required string Password { get; set; }
    }
}
