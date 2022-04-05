using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    internal class BasicItem : Item
    {
        public BasicItem(string name, Rarity rarity) : base(name, rarity)
        {
        }

        protected override bool OnUse()
        {
            return false;
        }
    }
}
