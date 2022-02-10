using Artefact.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal abstract class Tile : IPrototype<Tile>
    {
        public static BaseTile GrassTile { get; } = new BaseTile(".", ConsoleColor.Green, false);
        public static BaseTile DarkGrassTile { get; } = new BaseTile(".", ConsoleColor.DarkGreen, false);
        public static BaseTile SandTile { get; } = new BaseTile(",", ConsoleColor.Yellow, false);
        public static BaseTile WaterTile { get; } = new BaseTile("#", ConsoleColor.Blue, true);
        public static BaseTile DeepWaterTile { get; } = new BaseTile("#", ConsoleColor.DarkBlue, true);
        public static BaseTile MountainTile { get; } = new BaseTile("#", ConsoleColor.Gray, true);
        public static BaseTile DeepMountainTile { get; } = new BaseTile("#", ConsoleColor.DarkGray, true);
        public static BaseTile StoneTile { get; } = new BaseTile("#", ConsoleColor.Gray, false);
        
        // Flowers
        public static FlowerTile RoseFlowerTile { get; } = new FlowerTile("#", ConsoleColor.Red);
        public static FlowerTile BluebellFlowerTile { get; } = new FlowerTile("#", ConsoleColor.Blue);
        public static FlowerTile GrassFlowerTile { get; } = new FlowerTile("#", ConsoleColor.DarkGreen);
        public static FlowerTile SunFlowerTile { get; } = new FlowerTile("o", ConsoleColor.Yellow);


        public static BaseTile TreeBarkTile { get; } = new BaseTile("#", ConsoleColor.DarkYellow, true);
        public static CaveTile CaveTile { get; } = new CaveTile();
        public static LadderTile LadderTile { get; } = new LadderTile();

        public static List<Tile> Tiles { get; private set; }

        public static List<Tile> GrassTiles
        {
            get
            {
                return new List<Tile>()
                {
                    GrassTile,
                    DarkGrassTile
                };
            }
        }

        public int ID { get; }

        public string Representation { get; }
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor ForegroundColor { get; set; }

        public bool Collidable { get; }

        public Tile(string representation, ConsoleColor color, bool collidable) : this(representation, color, color, collidable)
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

        public abstract void OnCollide(Entity entity);

        public Tile Clone()
        {
            Tile tile = (Tile)MemberwiseClone();
            return tile;
        }

        public static Tile GetTile(int id)
        {
            if(id >= Tiles.Count) return null;
            return Tiles[id];
        }

        public override bool Equals(object obj)
        {
            if (obj is Tile tile)
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
            }
            else if (a is null)
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
