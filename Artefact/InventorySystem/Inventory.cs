using Artefact.Items;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.InventorySystem
{
    [Serializable]
    internal class Inventory
    {
        public List<Item> items = new List<Item>();
        private const int MAX_ITEMS_PER_LINE = 5;
        private const int ITEM_SPACING = 25;
        private const int MAX_ITEMS = 20;
        private const int MAX_STACK = 99;

        public int itemIndex = -1;

        public SwordItem EquippedSword { get; private set; }

        public bool AddItem(Item item, int quantity = 1, bool force=false)
        {
            if (items.Count >= MAX_ITEMS && !force)
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
                if (GetItem(item).Quantity > MAX_STACK)
                    GetItem(item).Quantity = MAX_STACK;
            }
            PrintInventory();
            return true;
        }

        public bool RemoveItem(Item item, int quantity = 1)
        {
            if (!HasItem(item))
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

            if (itemIndex >= items.Count)
                itemIndex = items.Count - 1;

            PrintInventory();
            return true;
        }

        public Item GetItem(Item item)
        {
            return items.Find(i => i.Name == item.Name && i.Rarity == item.Rarity);
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
            int xPos = GetEndOfInventory();

            for (int i = Map.Instance.Height + 2; i < Console.WindowHeight; i++)
            {
                Console.SetCursorPosition(xPos, i);
                Console.Write(new string(' ', Console.WindowWidth - xPos));
            }
        }

        int GetEndOfInventory()
        {
            return ((items.Count / MAX_ITEMS_PER_LINE) + 2) * ITEM_SPACING;
        }

        private void PrintItemUsage(Item item)
        {
            ClearItemUsage();

            int xPos = GetEndOfInventory();
            Console.SetCursorPosition(xPos, Map.Instance.Height + 2);
            Console.Write(item.Name);
            Console.CursorTop++;
            Console.CursorLeft = xPos;
            if (item is IUsable usable)
            {
                Console.Write($"E. {usable.UseText}");
                Console.CursorTop++;
                Console.CursorLeft = xPos;
            }
            Console.Write("Q. Drop");
            Console.CursorTop++;
            Console.CursorLeft = xPos;
        }

        public void PrintInventory()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = Map.Instance.Height + 2;
            Console.Write("Equpped Weapon: ");
            if (EquippedSword == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("None");
            }
            else
            {
                switch (EquippedSword.Rarity)
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
                Console.Write(EquippedSword.Name);
            }
            Console.ResetColor();

            for (int i = 0; i < items.Count; i++)
            {
                int xPos = ((i / MAX_ITEMS_PER_LINE) + 1) * ITEM_SPACING;
                Console.SetCursorPosition(xPos, Map.Instance.Height + 2 + (i % MAX_ITEMS_PER_LINE));
                Console.Write(new string(' ', ITEM_SPACING));
                Console.CursorLeft = xPos;
                Item item = items[i];

                if (itemIndex == i)
                {
                    PrintItemUsage(item);
                    Console.SetCursorPosition(xPos, Map.Instance.Height + 2 + (i % MAX_ITEMS_PER_LINE));

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

        public void Equip(Item item, EquipmentType equipmentType)
        {
            switch (equipmentType)
            {
                case EquipmentType.Sword:
                    {
                        SwordItem swordItem = (SwordItem)item.Clone();
                        swordItem.Quantity = 1;
                        EquippedSword = swordItem;
                    }break;
            }
        }

        public enum EquipmentType
        {
            Sword
        }
    }
}
