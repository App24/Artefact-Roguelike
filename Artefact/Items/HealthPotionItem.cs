using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    [Serializable]
    internal class HealthPotionItem : Item, IUsable
    {
        public HealthPotionItem(Rarity rarity) : base("Health Potion", rarity)
        {
        }

        public bool OnUse()
        {
            if (PlayerEntity.Instance.Health < PlayerEntity.Instance.MaxHealth)
            {
                PlayerEntity.Instance.Heal((((int)Rarity) + 1));
                return true;
            }
            return false;
        }
    }
}
