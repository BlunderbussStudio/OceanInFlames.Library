using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OceansInFlame.Library.Structures
{
    public struct Stash
    {
        public int StashLevel { get; set; }
        public List<StoredItem> OrderedItemsStoredInStash { get; set; }
        public int EmptySpace { get; set; }

        public Stash()
        {
            EmptySpace = 100 + (StashLevel - 1) * 50;
        }
    }
}
