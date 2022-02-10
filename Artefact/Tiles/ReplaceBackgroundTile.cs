using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal abstract class ReplaceBackgroundTile : Tile
    {
        protected ReplaceBackgroundTile(string representation, ConsoleColor color, bool collidable) : base(representation, color, collidable)
        {
        }

        protected ReplaceBackgroundTile(string representation, ConsoleColor backgroundColor, ConsoleColor foregroundColor, bool collidable) : base(representation, backgroundColor, foregroundColor, collidable)
        {
        }
    }
}
