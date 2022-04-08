using Artefact.Items;
using Artefact.Tiles;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.MapSystem
{
    [Serializable]
    internal class Room
    {
        public int Width { get; }
        public int Height { get; }
        public Vector2 Position { get; }

        public bool Known { get; set; }

        private Tile[] tiles;
        private const int MAX_CHESTS = 3;

#if !CHEATS
        private const float CHEST_CHANCE = 0.01f;
#else
        private const float CHEST_CHANCE = 1f;
#endif

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

            List<ChestTile> chests = new List<ChestTile>();

            Random random = new Random();

            for (int y = 2; y < height - 2; y++)
            {
                for (int x = 2; x < width - 2; x++)
                {
                    if (chests.Count < MAX_CHESTS)
                    {
                        if (random.NextDouble() <= CHEST_CHANCE)
                        {
                            ChestTile chestTile = new ChestTile();
                            //chestTile.AddItem(new HealthPotionItem((Rarity)new Random().Next(0, ((int)Rarity.Epic) + 1)), 3);
                            chestTile.AddItem(new WeaponItem("Sword", (Rarity)new Random().Next(0, ((int)Rarity.Epic) + 1), 1));

                            chestTile.AddItem(new ArmorItem("Helmet", (Rarity)new Random().Next(0, ((int)Rarity.Epic) + 1), ArmorType.Helmet, 1));
                            chestTile.AddItem(new ArmorItem("Chestplate", (Rarity)new Random().Next(0, ((int)Rarity.Epic) + 1), ArmorType.Chestplate, 1));
                            chestTile.AddItem(new ArmorItem("Leggings", (Rarity)new Random().Next(0, ((int)Rarity.Epic) + 1), ArmorType.Leggings, 1));
                            chestTile.AddItem(new ArmorItem("Boots", (Rarity)new Random().Next(0, ((int)Rarity.Epic) + 1), ArmorType.Boots, 1));
                            chests.Add(SetTile(x, y, chestTile));
                        }
                    }
                }
            }
        }

        public Vector2 GetAvailablePosition()
        {
            return new Vector2(1, 1);
        }

        public Vector2 GetRandomPosition()
        {
            return new Random().NextVector2(new Vector2(2, 2), new Vector2(Width - 3, Height - 3));
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
            Tile toReturn = tile.Clone();
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
