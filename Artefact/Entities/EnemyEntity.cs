using Artefact.BattleSystem;
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

        public override int HitDamage { get; }
        public override int Defense { get; }

        public EnemyType EnemyType { get; }

        [NonSerialized]
        private Random random = new Random();
        private int radius;

        public EnemyEntity(EnemyType enemyType)
        {
            switch (enemyType)
            {
                case EnemyType.Snake:
                    {
                        Representation = "SN";
                        MaxHealth = 3;
                        radius = 5;
                        HitDamage = 1;
                        Defense = 0;
                    }
                    break;
            }
            Heal(MaxHealth);
            EnemyType = enemyType;
        }

        public override void Move()
        {
            if (CurrentRoom == PlayerEntity.Instance.CurrentRoom)
            {
                if (PlayerEntity.Instance.RelativePosition.DistanceTo(RelativePosition) <= radius)
                {
                    if (random.NextDouble() > 0.2f)
                    {
                        Vector2 position = AStarPathfinding.Calculate(this.position, PlayerEntity.Instance.position)[0];
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

        public override void OnCollide(Entity entity)
        {
            Battle.StartBattle(this);
        }
    }

    enum EnemyType
    {
        Snake
    }
}
