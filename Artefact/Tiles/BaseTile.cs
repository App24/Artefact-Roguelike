using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    [Serializable]
    internal class BaseTile : Tile
    {
        public BaseTile(string representation, bool collidable, ConsoleColor foreground = ConsoleColor.White) : base(representation, collidable, foreground)
        {
        }

        public override void OnCollision(Entity entity)
        {

        }
    }
}
