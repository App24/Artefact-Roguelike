using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    [Serializable]
    internal abstract class Item
    {
        public string Name { get; }
        public Rarity Rarity { get; }

        public ConsoleColor ItemColor
        {
            get
            {
                switch (Rarity)
                {
                    case Rarity.Common:
                        {
                            return ConsoleColor.White;
                        }
                    case Rarity.Uncommon:
                        {
                            return ConsoleColor.Green;
                        }
                    case Rarity.Rare:
                        {
                            return ConsoleColor.DarkBlue;
                        }
                    case Rarity.Epic:
                        {
                            return ConsoleColor.DarkMagenta;
                        }
                    default:
                        return ConsoleColor.White;
                }
            }
        }

        public int Quantity { get; set; }

        public Item(string name, Rarity rarity)
        {
            Name = name;
            Rarity = rarity;
            Quantity = 1;
        }

        public Item Clone()
        {
            return (Item)MemberwiseClone();
        }
    }

    internal enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic
    }
}
