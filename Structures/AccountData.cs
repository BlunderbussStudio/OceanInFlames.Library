namespace OceansInFlame.Library.Structures
{
    public struct AccountData
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string HexedPassword { get; set; }
        public string? AuthToken { get; set; }
        public required Stash PlayerStash { get; set; }
        public required PlayerEquipment PlayerEquipedGear { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastLogin { get; set; }


    }

    public struct AccountKey
    {
        public required Guid AccountId { get; set; }
        public required string AuthToken { get; set; }
    }
}