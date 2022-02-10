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
            Position.Y--;
        }

        public override void Update()
        {

        }
    }
}
