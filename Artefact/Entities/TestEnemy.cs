using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    internal class TestEnemy : AIEnemy
    {
        public override string Representation => "TE";

        public override int MaxHealth => 5;

        public override void Move()
        {
            Vector2i position = AStarPathfinding.Calculate(Position, PlayerEntity.Instance.Position)[0];
            if (position != PlayerEntity.Instance.Position)
                Position = position;
        }

        public override void Update()
        {

        }
    }
}
