using Artefact.InventorySystem;
using Artefact.Items;
using Artefact.Tiles;
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
                Item item = Inventory.items[Inventory.itemIndex];

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
                    if (item is IUsable usable)
                    {
                        if (usable.OnUse()) {
                            Inventory.RemoveItem(item);
                            if (Inventory.items.Count <= 0)
                                ToggleInventory();
                        }
                    }
                }

                if (InputSystem.IsKeyHeld(ConsoleKey.Q))
                {
                    Tile currentTile = CurrentRoom.GetTile(RelativePosition);
                    if (Inventory.RemoveItem(item))
                    {
                        if (currentTile is ChestTile chestTile)
                        {
                            chestTile.AddItem(item);
                        }
                        else
                        {
                            CurrentRoom.SetTile(RelativePosition, new ChestTile(currentTile.Clone(), RelativePosition)).AddItem(item);
                        }
                        if (Inventory.items.Count <= 0)
                            ToggleInventory();
                    }
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
