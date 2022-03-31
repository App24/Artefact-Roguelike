using Artefact.Items;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.InventorySystem
{
    internal class Inventory
    {
        List<Item> items = new List<Item>();

        public void AddItem(Item item, int quantity = 1)
        {
            if (!HasItem(item))
            {
                items.Add(item);
            }
            else
            {
                items.Find(i => i.Name == item.Name).Quantity += quantity;
            }
        }

        public bool HasItem(Item item)
        {
            return items.Exists(i => i.Name == item.Name);
        }

        public void PrintInventory()
        {
            Console.SetCursorPosition(0, Map.Instance.Height + 2);
            foreach(Item item in items)
            {
                item.Print();
                Console.WriteLine();
            }
        }
    }
}
