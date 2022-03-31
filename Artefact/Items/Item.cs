using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    internal class Item
    {
        public static Item TestItem => new Item("Test", Rarity.Common);
        public static Item Test2Item => new Item("Test2", Rarity.Epic);

        public string Name { get; }
        public Rarity Rarity { get; }

        public int Quantity { get; set; }

        public Item(string name, Rarity rarity)
        {
            Name = name;
            Rarity = rarity;
            Quantity = 1;
        }

        public void Print()
        {
            switch (Rarity)
            {
                case Rarity.Common:
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    break;
                case Rarity.Uncommon:
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    break;
                case Rarity.Rare:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                    break;
                case Rarity.Epic:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    }
                    break;
            }
            Console.Write(Name);
            Console.ResetColor();
            Console.Write($": {Quantity}");
        }

        /*public Item Clone()
        {
            return (Item)MemberwiseClone();
        }*/
    }

    enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic
    }
}
