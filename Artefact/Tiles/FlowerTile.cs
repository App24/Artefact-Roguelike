using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal class FlowerTile : ReplaceBackgroundTile
    {
        public FlowerTile(string representation, ConsoleColor foregroundColor) : base(representation, Tile.GrassTile.BackgroundColor, foregroundColor, false)
        {

        }

        public override void OnCollide(Entity entity)
        {

        }
    }
}
