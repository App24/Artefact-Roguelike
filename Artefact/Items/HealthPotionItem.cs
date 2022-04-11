using Artefact.Entities;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    [Serializable]
    internal class HealthPotionItem : Item, IUsable
    {
        public HealthPotionItem(Rarity rarity) : base($"Health Potion {Map.Instance.Level}", rarity)
        {
        }

        public string UseText => "Use";

        public bool OnUse()
        {
            if (PlayerEntity.Instance.Health < PlayerEntity.Instance.MaxHealth)
            {
                PlayerEntity.Instance.Heal((((int)Rarity) + 1) * Map.Instance.Level);
                return true;
            }
            return false;
        }
    }
}
