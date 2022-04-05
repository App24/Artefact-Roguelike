using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    internal abstract class Item
    {
        public static BasicItem TestItem => new BasicItem("Test", Rarity.Common);
        public static BasicItem Test2Item => new BasicItem("Test2", Rarity.Epic);

        public string Name { get; }
        public Rarity Rarity { get; }

        public int Quantity { get; set; }

        public Item(string name, Rarity rarity)
        {
            Name = name;
            Rarity = rarity;
            Quantity = 1;
        }

        protected abstract bool OnUse();

        public void Use()
        {
            if (OnUse())
            {
                PlayerEntity.Instance.Inventory.RemoveItem(this);
            }
        }
    }

    enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic
    }
}
