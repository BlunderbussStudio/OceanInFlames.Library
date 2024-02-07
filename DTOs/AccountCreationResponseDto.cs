using OceansInFlame.Library.Structures;

namespace OceansInFlame.Library.DTOs
{
    public class AccountCreationResponseDto
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public AccountResponseDto? Data { get; set; }


        public AccountCreationResponseDto(bool success, string error)
        {
            Success = success;
            ErrorMessage = error;
        }

        public AccountCreationResponseDto(bool success, AccountResponseDto data)
        {
            Success = success;
            Data = data;
        }
    }
}
