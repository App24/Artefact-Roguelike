using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    class Tile : IPrototype<Tile>
    {
        public static Tile GrassTile { get; } = new Tile(".", ConsoleColor.DarkGreen, false);
        public static Tile SandTile { get; } = new Tile(",", ConsoleColor.Yellow, false);
        public static Tile WaterTile { get; } = new Tile("#", ConsoleColor.Blue, true);
        public static Tile DeepWaterTile { get; } = new Tile("#", ConsoleColor.DarkBlue, true);
        public static Tile MountainTile { get; } = new Tile("#", ConsoleColor.Gray, true);
        public static Tile DeepMountainTile { get; } = new Tile("#", ConsoleColor.DarkGray, true);
        public static Tile RoseFlowerTile { get; } = new Tile("#", ConsoleColor.DarkGreen, ConsoleColor.Red, false);
        public static Tile BluebellFlowerTile { get; } = new Tile("#", ConsoleColor.DarkGreen, ConsoleColor.Blue, false);
        public static List<Tile> Tiles { get; private set; }

        public int ID { get; }

        public string Representation { get; }
        public ConsoleColor BackgroundColor { get; }
        public ConsoleColor ForegroundColor { get; }

        public bool Collidable { get; }

        public Tile(string representation, ConsoleColor color, bool collidable):this(representation, color, color, collidable)
        {
        }


        public Tile(string representation, ConsoleColor backgroundColor, ConsoleColor foregroundColor, bool collidable)
        {
            if (Tiles == null) Tiles = new List<Tile>();
            ID = Tiles.Count;
            Representation = representation;
            BackgroundColor = backgroundColor;
            ForegroundColor = foregroundColor;
            Collidable = collidable;
            Tiles.Add(this);
        }

        public Tile Clone()
        {
            Tile tile = (Tile)MemberwiseClone();

            return tile;
        }

        public static Tile GetTile(int id)
        {
            return Tiles[id];
        }

        public override bool Equals(object obj)
        {
            if(obj is Tile tile)
            {
                return tile.ID == ID;
            }
            return false;
        }

        public static bool operator ==(Tile a, Tile b)
        {
            if (a is null && b is null)
            {
                return true;
            }else if(a is null)
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(Tile a, Tile b)
        {
            if (a is null && b is null)
            {
                return false;
            }
            else if (a is null)
            {
                return true;
            }
            return !a.Equals(b);
        }
    }
}
