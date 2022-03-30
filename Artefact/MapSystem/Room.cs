using Artefact.Tiles;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MapSystem
{
    internal class Room
    {
        public int Width { get; }
        public int Height { get; }
        public Vector2 Position { get; }

        public bool Known { get; set; }

        private Tile[] tiles;

        public Room(int width, int height, Vector2 position)
        {
            Width = width;
            Height = height;
            Position = position;

            tiles = new Tile[Width * Height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (x == 0 || y == 0 || x >= Width - 1 || y >= Height - 1)
                    {
                        SetTile(x, y, Tile.WallTile);
                    }
                    else
                    {
                        SetTile(x, y, Tile.AirTile);
                    }
                }
            }
        }

        public Vector2 GetAvailablePosition()
        {
            return new Vector2(1, 1);
        }

        public Tile GetTile(Vector2 position)
        {
            if (position.x < 0 || position.y < 0 || position.x >= Width || position.y >= Height)
                return null;
            return tiles[GetIndex(position)];
        }

        public T SetTile<T>(Vector2 position, T tile) where T : Tile
        {
            if (position.x < 0 || position.y < 0 || position.x >= Width || position.y >= Height)
                return null;
            Tile toReturn=tile.Clone();
            tiles[GetIndex(position)] = toReturn;
            return (T)toReturn;
        }

        public T SetTile<T>(int x, int y, T tile) where T : Tile
        {
            return SetTile(new Vector2(x, y), tile);
        }

        private int GetIndex(Vector2 position)
        {
            return position.y * Width + position.x;
        }
    }
}
