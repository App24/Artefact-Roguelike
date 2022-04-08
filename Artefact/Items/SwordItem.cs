using Artefact.Entities;
using Artefact.InventorySystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    internal class SwordItem : Item, IUsable
    {
        public SwordItem(string name, Rarity rarity) : base(name, rarity)
        {
        }

        public string UseText => "Equip";

        public bool OnUse()
        {
            SwordItem currentSword = PlayerEntity.Instance.Inventory.EquippedSword;
            if (currentSword != null)
            {
                PlayerEntity.Instance.Inventory.AddItem(currentSword, force: true);
            }
            PlayerEntity.Instance.Inventory.Equip(this, Inventory.EquipmentType.Sword);
            return true;
        }
    }
}
