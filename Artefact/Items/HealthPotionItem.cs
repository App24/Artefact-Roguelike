using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Items
{
    internal class HealthPotionItem : Item
    {
        public HealthPotionItem(Rarity rarity) : base("Health Potion", rarity)
        {
        }

        protected override bool OnUse()
        {
            PlayerEntity.Instance.Heal((((int)Rarity) + 1));
            return true;
        }
    }
}
