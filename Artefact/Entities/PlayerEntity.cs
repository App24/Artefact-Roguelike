using Artefact.InventorySystem;
using Artefact.Items;
using Artefact.MapSystem;
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

        const int HEALTH_POS = 1;
        const string HEALTH_TEXT = "Health: ";

        PlayerState state;

        public PlayerEntity()
        {
            Instance = this;
            Inventory = new Inventory();
        }

        public override void Move()
        {
            switch (state)
            {
                case PlayerState.Move:
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
                    break;
                case PlayerState.Inventory:
                    {
                        Item item = Inventory.items[Inventory.itemIndex];

                        if (InputSystem.IsKeyHeld(ConsoleKey.S))
                        {
                            Inventory.itemIndex++;
                            if (Inventory.itemIndex >= Inventory.items.Count)
                            {
                                Inventory.itemIndex = Inventory.items.Count - 1;
                            }
                        }

                        if (InputSystem.IsKeyHeld(ConsoleKey.W))
                        {
                            Inventory.itemIndex--;
                            if (Inventory.itemIndex < 0)
                                Inventory.itemIndex = 0;
                        }

                        if (InputSystem.IsKeyHeld(ConsoleKey.E))
                        {
                            if (item is IUsable usable)
                            {
                                if (usable.OnUse())
                                {
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
                    break;
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
            state = state == PlayerState.Inventory ? PlayerState.Move : PlayerState.Inventory;
            if (state==PlayerState.Inventory)
            {
                Inventory.itemIndex = 0;
            }
            else
            {
                Inventory.itemIndex = -1;
            }
            Inventory.ClearItemUsage();
            Inventory.PrintInventory();
        }

        protected override void Die()
        {

        }

        public override void Update()
        {

        }

        public override void Heal(int amount)
        {
            Console.SetCursorPosition(Map.Instance.Width * 2 + 2 + HEALTH_TEXT.Length, HEALTH_POS);
            Console.Write(new string(' ', Health.ToString().Length));
            base.Heal(amount);
            PrintHealth();
        }

        public override void Damage(int amount)
        {
            Console.SetCursorPosition(Map.Instance.Width * 2 + 2 + HEALTH_TEXT.Length, HEALTH_POS);
            Console.Write(new string(' ', Health.ToString().Length));
            base.Damage(amount);
            PrintHealth();
        }

        public void PrintHealth()
        {
            Console.SetCursorPosition(Map.Instance.Width * 2 + 2, HEALTH_POS);
            Console.Write(HEALTH_TEXT);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Health);
            Console.ResetColor();
        }

        enum PlayerState
        {
            Move,
            Inventory
        }
    }
}
