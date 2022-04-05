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

        public override int MaxHealth => 5;

        bool inInventory;

        public PlayerEntity()
        {
            Instance = this;
            Inventory = new Inventory();
        }

        public override void Move()
        {
            if (!inInventory)
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
            }
            else
            {
                if (InputSystem.IsKeyHeld(ConsoleKey.S))
                {
                    Inventory.itemIndex++;
                    if(Inventory.itemIndex >= Inventory.items.Count)
                    {
                        Inventory.itemIndex = Inventory.items.Count - 1;
                    }
                }

                if (InputSystem.IsKeyHeld(ConsoleKey.W))
                {
                    Inventory.itemIndex--;
                    if(Inventory.itemIndex < 0)
                        Inventory.itemIndex = 0;
                }

                if (InputSystem.IsKeyHeld(ConsoleKey.Enter))
                {
                    Inventory.items[Inventory.itemIndex].Use();
                    if (Inventory.items.Count <= 0)
                        ToggleInventory();
                }

                Inventory.PrintInventory();
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.I))
            {
                if (Inventory.items.Count > 0)
                {
                    ToggleInventory();
                }
            }
        }

        void ToggleInventory()
        {
            inInventory = !inInventory;
            if (inInventory)
            {
                Inventory.itemIndex = 0;
            }
            else
            {
                Inventory.itemIndex = -1;
            }
            Inventory.PrintInventory();
        }

        protected override void Die()
        {

        }

        public override void Update()
        {

        }
    }
}
