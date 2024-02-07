using OceansInFlame.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Structures
{
    public struct Weapon
    {
        public required Item Info { get; set; }
        public required string WeaponId { get; set; }
        public required int DefaultAmmo { get; set; }
        public required int BaseDamage { get; set; }
        public required int ArmorPenetration { get; set; }
        public required int MelleeDamage { get; set; }
        public required int MagazineSize { get; set; }
        public required float CycleRate { get; set; }
        public required float RecoilModifier { get; set; }
        public required float NoiseModifier { get; set; }
        public required float RecoilClamp { get; set; }
        public required float Stability { get; set; }
        public required float ReloadModifier { get; set; }
        public required float Unwieldiness { get; set; }
        public required List<FireModeEnum> PossibleFireModes { get; set; }
        public required List<ProjectileType> ProjectileType { get; set; }
        public required WeaponTypeEnum WeaponType { get; set; }
        public required List<WeaponAttachment> WeaponAttachments { get; set; }
    }
}
