using Artefact.Entities;
using Artefact.InventorySystem;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    [Serializable]
    internal class ArmorItem : EquipmentItem
    {
        public ArmorItem(string name, Rarity rarity, ArmorType armorType, int baseDefense) : base(name, rarity, Inventory.EquipmentType.Armor)
        {
            ArmorType = armorType;
            BaseDefense = baseDefense * Map.Instance.Level;
        }

        public ArmorType ArmorType { get; }
        public int BaseDefense { get; }
        public int Defense { get { return BaseDefense + (int)Rarity + 1 + (int)ArmorType; } }

        public override bool OnUse()
        {
            ArmorItem currentArmorPiece = PlayerEntity.Instance.Inventory.GetArmor(ArmorType);

            if (currentArmorPiece != null)
            {
                PlayerEntity.Instance.Inventory.AddItem(currentArmorPiece, force: true);
            }

            PlayerEntity.Instance.Inventory.Equip(this);
            return true;
        }
    }

    internal enum ArmorType
    {
        Boots,
        Helmet,
        Leggings,
        Chestplate,
    }
}
