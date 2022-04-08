using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    [Serializable]
    internal abstract class Tile
    {
        public static BaseTile AirTile { get; } = new BaseTile(" ", false);
        public static BaseTile WallTile { get; } = new BaseTile("#", true, ConsoleColor.DarkGray);

        public virtual string Representation { get; }
        public virtual bool Collidable { get; }
        public virtual ConsoleColor Foreground { get; }

        public Tile(string representation, bool collidable, ConsoleColor foreground)
        {
            Representation = representation;
            Collidable = collidable;
            Foreground = foreground;
        }

        public Tile Clone()
        {
            return (Tile)MemberwiseClone();
        }

        public abstract void OnCollision(Entity entity);
    }
}
