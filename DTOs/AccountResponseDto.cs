using OceansInFlame.Library.Structures;

namespace OceansInFlame.Orchestration.API.DTOs
{
    public class AccountResponseDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? AuthToken { get; set; }
        public Stash PlayerStash { get; set; }
        public PlayerEquipment PlayerEquipedGear { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastLogin { get; set; }

        public AccountResponseDto(AccountData accountData)
        {
            Name = accountData.Name;
            Email = accountData.Email;
            AuthToken = accountData.AuthToken ?? null;
            PlayerStash = accountData.PlayerStash;
            PlayerEquipedGear = accountData.PlayerEquipedGear;
            CreationTime = accountData.CreationTime;
            LastLogin = accountData.LastLogin;
        }
    }
}
