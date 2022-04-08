using Artefact.Entities;
using Artefact.InventorySystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    [Serializable]
    internal class WeaponItem : EquipmentItem
    {
        public int BaseDamage { get; }
        public int Damage { get { return BaseDamage + (int)Rarity + 1; } }

        public WeaponItem(string name, Rarity rarity, int baseDamage) : base(name, rarity, Inventory.EquipmentType.Weapon)
        {
            BaseDamage = baseDamage;
        }

        public override bool OnUse()
        {
            WeaponItem currentSword = PlayerEntity.Instance.Inventory.EquippedWeapon;

            if (currentSword != null)
            {
                PlayerEntity.Instance.Inventory.AddItem(currentSword, force: true);
            }

            PlayerEntity.Instance.Inventory.Equip(this);
            return true;
        }
    }
}
