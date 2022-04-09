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
    [Serializable]
    internal class PlayerEntity : Entity
    {
        public static PlayerEntity Instance { get; set; }

        public Inventory Inventory { get; }

        public override string Representation => "PL";

        public override int MaxHealth => 10;

        public override int HitDamage => Inventory.HitDamage;
        public override int Defense => Inventory.Defense;


        private const int HEALTH_POS = 1;
        private const int EQUIPMENT_START_POS = 3;
        private const int EQUIPMENT_STATS_START_POS = EQUIPMENT_START_POS + 6;

        private const string HEALTH_TEXT = "Health: ";
        private const string WEAPON_TEXT = "Current Weapon: ";
        private const string HELMET_TEXT = "Current Helmet: ";
        private const string CHESTPLATE_TEXT = "Current Chestplate: ";
        private const string LEGGINGS_TEXT = "Current Leggings: ";
        private const string BOOTS_TEXT = "Current Boots: ";
        private const string ATTACK_TEXT = "Attack Damage: ";
        private const string DEFENSE_TEXT = "Defense: ";

        private const string NONE_TEXT = "None";

        private PlayerState state;

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

        private void ToggleInventory()
        {
            state = state == PlayerState.Inventory ? PlayerState.Move : PlayerState.Inventory;
            if (state == PlayerState.Inventory)
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

        /*public override void Heal(int amount)
        {
            ClearStat(HEALTH_POS, HEALTH_TEXT.Length, Health.ToString().Length);
            base.Heal(amount);
            PrintHealth();
        }

        public override void Damage(int amount)
        {
            ClearStat(HEALTH_POS, HEALTH_TEXT.Length, Health.ToString().Length);
            base.Damage(amount);
            PrintHealth();
        }*/

        public void PrintHealth()
        {
            Console.SetCursorPosition(Map.Instance.Width * 2 + 2, HEALTH_POS);
            Console.Write(HEALTH_TEXT);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Health);
            Console.ResetColor();
        }

        private void ClearStat(int yPos, int offset, int length)
        {
            Console.SetCursorPosition(Map.Instance.Width * 2 + 2 + offset, yPos);
            Console.Write(new string(' ', length));
        }

        public void ClearEquipment()
        {
            ClearStat(EQUIPMENT_START_POS + 0, WEAPON_TEXT.Length, (Inventory.EquippedWeapon != null ? Inventory.EquippedWeapon.Name : NONE_TEXT).Length);
            ClearStat(EQUIPMENT_START_POS + 1, HELMET_TEXT.Length, (Inventory.EquippedHelmet != null ? Inventory.EquippedHelmet.Name : NONE_TEXT).Length);
            ClearStat(EQUIPMENT_START_POS + 2, CHESTPLATE_TEXT.Length, (Inventory.EquippedChestplate != null ? Inventory.EquippedChestplate.Name : NONE_TEXT).Length);
            ClearStat(EQUIPMENT_START_POS + 3, LEGGINGS_TEXT.Length, (Inventory.EquippedLeggings != null ? Inventory.EquippedLeggings.Name : NONE_TEXT).Length);
            ClearStat(EQUIPMENT_START_POS + 4, BOOTS_TEXT.Length, (Inventory.EquippedBoots != null ? Inventory.EquippedBoots.Name : NONE_TEXT).Length);

            ClearStat(EQUIPMENT_STATS_START_POS + 0, ATTACK_TEXT.Length, Inventory.HitDamage.ToString().Length);
            ClearStat(EQUIPMENT_STATS_START_POS + 1, DEFENSE_TEXT.Length, Inventory.Defense.ToString().Length);
        }

        public void PrintEquipment()
        {
            #region Equipment
            Console.CursorLeft = Map.Instance.Width * 2 + 2;
            Console.CursorTop = EQUIPMENT_START_POS;
            Console.Write(WEAPON_TEXT);
            if (Inventory.EquippedWeapon == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(NONE_TEXT);
            }
            else
            {
                Console.ForegroundColor = Inventory.EquippedWeapon.ItemColor;
                Console.Write(Inventory.EquippedWeapon.Name);
            }
            Console.ResetColor();

            #region Armor

            Console.CursorLeft = Map.Instance.Width * 2 + 2;
            Console.CursorTop = EQUIPMENT_START_POS + 1;
            Console.Write(HELMET_TEXT);
            if (Inventory.EquippedHelmet == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(NONE_TEXT);
            }
            else
            {
                Console.ForegroundColor = Inventory.EquippedHelmet.ItemColor;
                Console.Write(Inventory.EquippedHelmet.Name);
            }
            Console.ResetColor();

            Console.CursorLeft = Map.Instance.Width * 2 + 2;
            Console.CursorTop = EQUIPMENT_START_POS + 2;
            Console.Write(CHESTPLATE_TEXT);
            if (Inventory.EquippedChestplate == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(NONE_TEXT);
            }
            else
            {
                Console.ForegroundColor = Inventory.EquippedChestplate.ItemColor;
                Console.Write(Inventory.EquippedChestplate.Name);
            }
            Console.ResetColor();

            Console.CursorLeft = Map.Instance.Width * 2 + 2;
            Console.CursorTop = EQUIPMENT_START_POS + 3;
            Console.Write(LEGGINGS_TEXT);
            if (Inventory.EquippedLeggings == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(NONE_TEXT);
            }
            else
            {
                Console.ForegroundColor = Inventory.EquippedLeggings.ItemColor;
                Console.Write(Inventory.EquippedLeggings.Name);
            }
            Console.ResetColor();

            Console.CursorLeft = Map.Instance.Width * 2 + 2;
            Console.CursorTop = EQUIPMENT_START_POS + 4;
            Console.Write(BOOTS_TEXT);
            if (Inventory.EquippedBoots == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(NONE_TEXT);
            }
            else
            {
                Console.ForegroundColor = Inventory.EquippedBoots.ItemColor;
                Console.Write(Inventory.EquippedBoots.Name);
            }
            Console.ResetColor();
            #endregion
            #endregion

            #region Stats
            Console.CursorLeft = Map.Instance.Width * 2 + 2;
            Console.CursorTop = EQUIPMENT_STATS_START_POS;
            Console.Write(ATTACK_TEXT);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Inventory.HitDamage);
            Console.ResetColor();

            Console.CursorLeft = Map.Instance.Width * 2 + 2;
            Console.CursorTop = EQUIPMENT_STATS_START_POS + 1;
            Console.Write(DEFENSE_TEXT);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(Inventory.Defense);
            Console.ResetColor();
            #endregion
        }

        public override void OnCollide(Entity entity)
        {

        }

        private enum PlayerState
        {
            Move,
            Inventory
        }
    }
}
