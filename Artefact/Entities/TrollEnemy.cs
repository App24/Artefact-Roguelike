using Artefact.Utils;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    [Serializable]
    internal class TrollEnemy : AIEnemy
    {
        public override string Representation => "TR";

        public override int MaxHealth => 5;

        public override int DistanceToAgro => 5;

        public override int SpawnWanderingRadius => 5;
    }
}
