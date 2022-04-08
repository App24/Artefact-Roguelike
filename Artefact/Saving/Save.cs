using Artefact.Entities;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Saving
{
    [Serializable]
    internal class Save
    {
        public Map map;
        public PlayerEntity player;

        public Save()
        {
            this.map = Map.Instance;
            this.player = PlayerEntity.Instance;
        }
    }
}
