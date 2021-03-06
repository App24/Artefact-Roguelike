using Artefact.InventorySystem;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    [Serializable]
    internal abstract class EquipmentItem : Item, IUsable
    {
        public Inventory.EquipmentType EquipmentType { get; }

        protected EquipmentItem(string name, Rarity rarity, Inventory.EquipmentType equipmentType) : base($"{name} {Map.Instance.Level}", rarity)
        {
            EquipmentType = equipmentType;
        }

        public string UseText => "Equip";

        public abstract bool OnUse();
    }
}
