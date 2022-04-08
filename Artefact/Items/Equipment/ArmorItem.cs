using Artefact.Entities;
using Artefact.InventorySystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    internal class ArmorItem : EquipmentItem
    {
        public ArmorItem(string name, Rarity rarity, ArmorType armorType, int baseDefense) : base(name, rarity, Inventory.EquipmentType.Armor)
        {
            ArmorType = armorType;
            BaseDefense = baseDefense;
        }

        public ArmorType ArmorType { get; }
        public int BaseDefense { get; }
        public int Defense { get { return BaseDefense + (int)Rarity + 1; } }

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
        Helmet,
        Chestplate,
        Leggings,
        Boots
    }
}
