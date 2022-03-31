using Artefact.Entities;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal class ChestTile : Tile
    {
        public override ConsoleColor Foreground => looted ? ConsoleColor.White : base.Foreground;
        bool looted = false;

        public ChestTile() : base("[]", false, ConsoleColor.DarkYellow)
        {
        }

        public override void OnCollision(Entity entity)
        {
            looted = true;
        }
    }
}
