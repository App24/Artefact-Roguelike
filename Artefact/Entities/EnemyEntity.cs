using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    [Serializable]
    internal class EnemyEntity : Entity
    {
        public override string Representation { get; }

        public override int MaxHealth { get; }

        public EnemyEntity(string representation, int maxHealth)
        {
            Representation = representation;
            MaxHealth = maxHealth;
            Heal(MaxHealth);
        }

        public override void Move()
        {

        }

        public override void Update()
        {

        }

        protected override void Die()
        {

        }
    }
}
