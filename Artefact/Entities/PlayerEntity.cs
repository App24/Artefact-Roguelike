using Artefact.InventorySystem;
using Artefact.Items;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    internal class PlayerEntity : Entity
    {
        public static PlayerEntity Instance { get; set; }

        public Inventory Inventory { get; }

        public override string Representation => "PL";

        public PlayerEntity()
        {
            Instance = this;
            Inventory = new Inventory();
            Inventory.AddItem(Item.TestItem);
            Inventory.AddItem(Item.TestItem);
            Inventory.AddItem(Item.Test2Item);
        }

        public override void Move()
        {
            if (InputSystem.IsKeyHeld(ConsoleKey.D))
            {
                position.x++;
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.A))
            {
                position.x--;
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.W))
            {
                position.y--;
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.S))
            {
                position.y++;
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.I))
            {
                Inventory.PrintInventory();
            }
        }
    }
}
