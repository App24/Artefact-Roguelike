using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    [Serializable]
    internal class BaseTile : Tile
    {
        public BaseTile(string representation, ConsoleColor color, bool collidable) : this(representation, color, color, collidable)
        {
        }


        public BaseTile(string representation, ConsoleColor backgroundColor, ConsoleColor foregroundColor, bool collidable) : base(representation, backgroundColor, foregroundColor, collidable)
        {

        }

        public override void OnCollide(Entity entity)
        {

        }
    }
}
