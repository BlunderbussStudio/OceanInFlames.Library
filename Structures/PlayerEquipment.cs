using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Structures
{
    public struct PlayerEquipment
    {
        public HeadGear? HeadGear { get; set; }
        public Armor? EquipedArmor { get; set; }
        public Rig? EquipedRig { get; set; }
        public Weapon? EquipedMainWeapon { get; set; }
        public Weapon? SideWeapon { get; set; }
        public Bag? EquipedBag { get; set; }

    }
}
