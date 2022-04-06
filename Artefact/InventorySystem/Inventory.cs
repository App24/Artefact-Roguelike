using Artefact.Items;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.InventorySystem
{
    internal class Inventory
    {
        public List<Item> items = new List<Item>();

        const int MAX_ITEMS_PER_LINE = 5;
        const int ITEM_SPACING = 25;

        const int MAX_ITEMS = 20;

        const int MAX_STACK = 99;

        public int itemIndex = -1;

        public bool AddItem(Item item, int quantity = 1)
        {
            if(items.Count >= MAX_ITEMS)
            {
                return false;
            }
            if (!HasItem(item))
            {
                item.Quantity = quantity;
                items.Add(item);
            }
            else
            {
                GetItem(item).Quantity += quantity;
                if(GetItem(item).Quantity > MAX_STACK)
                    GetItem(item).Quantity = MAX_STACK;
            }
            PrintInventory();
            return true;
        }

        public bool RemoveItem(Item item, int quantity=1)
        {
            if(!HasItem(item))
                return false;

            Item toRemove = GetItem(item);
            if (toRemove.Quantity < quantity)
                return false;
            toRemove.Quantity -= quantity;

            if (toRemove.Quantity <= 0)
            {
                items.Remove(toRemove);
                ClearInventorySpace();
            }

            PrintInventory();
            return true;
        }

        public Item GetItem(Item item)
        {
            return items.Find(i => i.Name == item.Name && i.Rarity==item.Rarity);
        }

        public bool HasItem(Item item, int quantity = 0)
        {
            Item invItem = GetItem(item);
            if (invItem == null)
                return false;
            return invItem.Quantity >= quantity;
        }

        private void ClearInventorySpace()
        {
            for (int i = 0; i < MAX_ITEMS_PER_LINE; i++)
            {
                Console.SetCursorPosition(0, Map.Instance.Height + 2 + i);
                Console.Write(new String(' ', Console.WindowWidth));
            }
        }

        public void ClearItemUsage()
        {
            int xPos = ((items.Count / MAX_ITEMS_PER_LINE) + 1) * ITEM_SPACING;

            for (int i = Map.Instance.Height + 2; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(xPos, i);
                Console.Write(new string(' ', Console.WindowWidth - xPos));
            }
        }

        void PrintItemUsage(Item item)
        {
            ClearItemUsage();

            int xPos = ((items.Count / MAX_ITEMS_PER_LINE) + 1) * ITEM_SPACING;
            Console.SetCursorPosition(xPos, Map.Instance.Height + 2);
            Console.Write(item.Name);
            Console.CursorTop++;
            Console.CursorLeft = xPos;
            if(item is IUsable)
            {
                Console.Write("E. Use");
                Console.CursorTop++;
                Console.CursorLeft = xPos;
            }
            Console.Write("Q. Drop");
            Console.CursorTop++;
            Console.CursorLeft = xPos;
        }

        public void PrintInventory()
        {
            for (int i = 0; i < items.Count; i++)
            {
                Console.SetCursorPosition((i / MAX_ITEMS_PER_LINE) * ITEM_SPACING, Map.Instance.Height + 2 + (i % MAX_ITEMS_PER_LINE));
                Console.Write(new string(' ', ITEM_SPACING));
                Console.CursorLeft = (i / MAX_ITEMS_PER_LINE) * ITEM_SPACING;
                Item item = items[i];

                if (itemIndex == i)
                {
                    PrintItemUsage(item);
                    Console.SetCursorPosition((i / MAX_ITEMS_PER_LINE) * ITEM_SPACING, Map.Instance.Height + 2 + (i % MAX_ITEMS_PER_LINE));

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(">> ");

                }

                switch (item.Rarity)
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
                Console.Write(item.Name);
                Console.ResetColor();
                Console.Write($": {item.Quantity}");
                Console.WriteLine();
            }
        }
    }
}
