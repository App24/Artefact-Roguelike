using Artefact.MapSystem;
using Artefact.Utils;
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

        public override int HitDamage => 1;

        [NonSerialized]
        private Random random = new Random();
        private int radius;

        public EnemyEntity(string representation, int maxHealth, int radius)
        {
            Representation = representation;
            MaxHealth = maxHealth;
            this.radius = radius;
            Heal(MaxHealth);
        }

        public override void Move()
        {
            if (CurrentRoom == PlayerEntity.Instance.CurrentRoom)
            {
                if (PlayerEntity.Instance.RelativePosition.DistanceTo(RelativePosition) <= radius)
                {
                    if (random.NextDouble() < 0.4f)
                    {
                        Vector2 position = AStarPathfinding.Calculate(this.position, PlayerEntity.Instance.position)[0];
                        if (Map.Instance.GetRoom(position) == CurrentRoom)
                            this.position = position;
                    }
                }
            }
        }

        public override void Update()
        {

        }

        protected override void Die()
        {

        }
    }
}
