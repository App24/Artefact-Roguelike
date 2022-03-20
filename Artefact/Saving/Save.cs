using Artefact.Entities;
using Artefact.Settings;
using Artefact.Utils;
using Artefact.Worlds;
using System;

namespace Artefact.Saving
{
    [Serializable]
    internal class Save
    {
        public World CurrentWorld { get; }
        public PlayerEntity Player { get; }

        public Save()
        {
            CurrentWorld = World.Instance;
            Player = PlayerEntity.Instance;
        }
    }
}
