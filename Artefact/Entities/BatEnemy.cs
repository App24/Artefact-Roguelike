using Artefact.Utils;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    [Serializable]
    internal class BatEnemy : AIEnemy
    {
        public override int DistanceToAgro => 5;

        public override int MaxHealth => 3;

        public override string Representation => "BT";

        public override int SpawnWanderingRadius => 5;
    }
}
