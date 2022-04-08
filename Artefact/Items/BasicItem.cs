using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    [Serializable]
    internal class BasicItem : Item
    {
        public BasicItem(string name, Rarity rarity) : base(name, rarity)
        {
        }
    }
}
