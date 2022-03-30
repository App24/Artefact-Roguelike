using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal abstract class Tile
    {
        public static BaseTile AirTile { get; } = new BaseTile(" ", false);
        public static BaseTile WallTile { get; } = new BaseTile("#", true, ConsoleColor.DarkGray);

        public string Representation { get; }
        public bool Collidable { get; }
        public ConsoleColor Foreground { get; }

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
