using Artefact.Utils;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    internal class TestEnemy : AIEnemy
    {
        public override string Representation => "TE";

        public override int MaxHealth => 5;

        public override int DistanceToAgro => 5;

        public override void Move()
        {
            Random random = new Random();
            switch (State)
            {
                case EnemyState.Chasing:
                    {
                        if (random.NextDouble() > 0.2f)
                        {
                            Vector2i position = AStarPathfinding.Calculate(Position, PlayerEntity.Instance.Position)[0];
                            if (position != PlayerEntity.Instance.Position)
                                Position = position;
                        }
                    }
                    break;
                case EnemyState.Wandering:
                    {
                        if (Position.DistanceTo(OriginalPosition) >= 5)
                        {
                            Vector2i position = AStarPathfinding.Calculate(Position, OriginalPosition)[0];
                            Position = position;
                        }
                        else
                        {
                            Vector2i randomPos = Vector2i.Zero;
                            do
                            {
                                randomPos = random.NextVector2i(new Vector2i(-1), new Vector2i(1));
                                Vector2i newPosition = Position + randomPos;
                            } while (World.Instance.GetTile(Position+randomPos).Collidable);
                            Vector2i position = AStarPathfinding.Calculate(Position, Position + randomPos)[0];
                            Position = position;
                        }
                    }
                    break;
            }
        }

        public override void Update()
        {
            if(PlayerEntity.Instance.Position.DistanceTo(Position) <= DistanceToAgro)
            {
                //State = EnemyState.Chasing;
            }
            else
            {
                State = EnemyState.Wandering;
            }
        }
    }
}
