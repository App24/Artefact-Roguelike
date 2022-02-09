using Artefact.Entities;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal class CaveTile : Tile
    {
        public CaveTile() : base("#", ConsoleColor.DarkMagenta, true)
        {
        }

        public override void OnCollide(Entity entity)
        {
            if (entity is PlayerEntity player)
            {
                Console.Clear();
                World currentWorld = World.Instance;
                World.Instance = new CaveWorld(20, 20, 3, currentWorld);
            }
        }
    }
}
