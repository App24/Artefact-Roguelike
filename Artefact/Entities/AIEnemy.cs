using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    [Serializable]
    internal abstract class AIEnemy : Entity
    {
        public abstract int DistanceToAgro { get; }
        public EnemyState State { get; protected set; } = EnemyState.Wandering;

        protected Random Random { get { return World.Instance.Random; } }

        public override void Update()
        {
            if (PlayerEntity.Instance.Position.DistanceTo(Position) <= DistanceToAgro)
            {
                State = EnemyState.Chasing;
            }
            else
            {
                State = EnemyState.Wandering;
            }
        }

        public override void CollidePlayer(PlayerEntity playerEntity)
        {
            // Fight
        }
    }

    internal enum EnemyState
    {
        Wandering,
        Chasing
    }
}
