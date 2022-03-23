using Artefact.Battles;
using Artefact.Utils;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    [Serializable]
    internal abstract class AIEnemy : Entity, IPrototype<AIEnemy>
    {
        public abstract int DistanceToAgro { get; }
        public abstract int SpawnWanderingRadius { get; }
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

        public override void Move()
        {
            switch (State)
            {
                case EnemyState.Chasing:
                    {
                        if (Random.NextDouble() > 0.2f)
                        {
                            MoveTowardsPlayer();
                        }
                    }
                    break;
                case EnemyState.Wandering:
                    {
                        if (Position.DistanceTo(OriginalPosition) >= SpawnWanderingRadius)
                        {
                            Vector2 position = AStarPathfinding.Calculate(Position, OriginalPosition)[0];
                            Position = position;
                        }
                        else
                        {
                            Vector2 randomPos;
                            do
                            {
                                randomPos = Random.NextVector2i(OriginalPosition-new Vector2(SpawnWanderingRadius), OriginalPosition+new Vector2(SpawnWanderingRadius));

                                if (randomPos.X < 0)
                                {
                                    randomPos.X = 0;
                                }
                                else if (randomPos.X >= World.Instance.Width)
                                {
                                    randomPos.X = World.Instance.Width - 1;
                                }

                                if (randomPos.Y < 0)
                                {
                                    randomPos.Y = 0;
                                }
                                else if (randomPos.Y >= World.Instance.Height)
                                {
                                    randomPos.Y = World.Instance.Height - 1;
                                }

                            } while (World.Instance.GetTile(randomPos).Collidable);
                            Vector2 position = AStarPathfinding.Calculate(Position, randomPos)[0];
                            Position = position;
                        }
                    }
                    break;
            }
        }

        public override void CollidePlayer(PlayerEntity playerEntity)
        {
            BattleSystem.StartFight(this);
        }

        protected void MoveTowardsPlayer()
        {
            Vector2 position = AStarPathfinding.Calculate(Position, PlayerEntity.Instance.Position)[0];
            Position = position;
        }

        public AIEnemy Clone()
        {
            AIEnemy enemy = (AIEnemy)MemberwiseClone();
            return enemy;
        }
    }

    internal enum EnemyState
    {
        Wandering,
        Chasing
    }
}
