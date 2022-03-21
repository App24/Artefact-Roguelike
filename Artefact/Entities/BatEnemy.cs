using Artefact.Utils;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    internal class BatEnemy : AIEnemy
    {
        public override int DistanceToAgro => 5;

        public override int MaxHealth => 3;

        public override string Representation => "BT";

        public override void Move()
        {
            switch (State)
            {
                case EnemyState.Chasing:
                    {
                        if (Random.NextDouble() > 0.2f)
                        {
                            Vector2 position = AStarPathfinding.Calculate(Position, PlayerEntity.Instance.Position)[0];
                            if (position != PlayerEntity.Instance.Position)
                                Position = position;
                        }
                    }
                    break;
                case EnemyState.Wandering:
                    {
                        if (Position.DistanceTo(OriginalPosition) >= 5)
                        {
                            Vector2 position = AStarPathfinding.Calculate(Position, OriginalPosition)[0];
                            Position = position;
                        }
                        else
                        {
                            Vector2 randomPos;
                            do
                            {
                                randomPos = Random.NextVector2i(new Vector2(-1), new Vector2(1));
                                Vector2 newPosition = Position + randomPos;
                                if (newPosition.X < 0 || newPosition.X >= World.Instance.Width)
                                    randomPos.X = 0;

                                if (newPosition.Y < 0 || newPosition.Y >= World.Instance.Height)
                                    randomPos.Y = 0;
                            } while (World.Instance.GetTile(Position + randomPos).Collidable);
                            Vector2 position = AStarPathfinding.Calculate(Position, Position + randomPos)[0];
                            Position = position;
                        }
                    }
                    break;
            }
        }
    }
}
