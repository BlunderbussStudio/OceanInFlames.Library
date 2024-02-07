using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Structures
{
    public struct Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int SlotWidth { get; set; }
        public int SlotHeight { get; set; }
        public int StackAmount { get; set; }
        public bool IsStackable { get; set; }
        public bool IsDroppable { get; set; }
        public int Weight { get; set; }
        public float BaseValueDollars { get; set; }

    }
}
