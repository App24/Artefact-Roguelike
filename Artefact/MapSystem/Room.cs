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
        private const float CHEST_CHANCE = 0.05f;
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

            Random random = new Random();

            if (width > 10 && height > 10)
            {
                int midX = (width + random.Next(-2, 3)) / 2;
                int midY = (height + random.Next(-2, 3)) / 2;
                int radius = height / 5;
                for (int y = -radius; y <= radius; y++)
                {
                    for (int x = -radius; x <= radius; x++)
                    {
                        if ((x * x) + (y * y) <= (radius * radius) * Math.Clamp(random.NextDouble(), 0.25f, 1f))
                        {
                            SetTile(x + midX, y + midY, Tile.WallTile);
                        }
                    }
                }
            }

            List<ChestTile> chests = new List<ChestTile>();

            for (int y = 2; y < height - 2; y++)
            {
                for (int x = 2; x < width - 2; x++)
                {
                    if (chests.Count < MAX_CHESTS)
                    {
                        if (GetTile(new Vector2(x, y)).Collidable)
                            continue;
                        if (random.NextDouble() <= CHEST_CHANCE)
                        {
                            ChestTile chestTile = new ChestTile();
                            double rarityChance = random.NextDouble();
                            Rarity rarity;
                            if (rarityChance < 0.5f)
                            {
                                rarity = Rarity.Common;
                            }
                            else if (rarityChance < 0.8f)
                            {
                                rarity = Rarity.Uncommon;
                            }
                            else if (rarityChance < 0.95f)
                            {
                                rarity = Rarity.Rare;
                            }
                            else
                            {
                                rarity = Rarity.Epic;
                            }

                            double itemChance = random.NextDouble();
                            chestTile.AddItem(new HealthPotionItem(rarity), random.Next(1, 6));

                            if (itemChance < 0.3f)
                            {
                                double quality = random.NextDouble();
                                int damage;
                                string name;
                                if (quality < 0.5f)
                                {
                                    damage = 3;
                                    name = "Sword";
                                }
                                else if (quality < 0.75f)
                                {
                                    damage = 5;
                                    name = "Great Sword";
                                }
                                else if (quality < 0.9f)
                                {
                                    damage = 6;
                                    name = "Bow";
                                }
                                else
                                {
                                    damage = 7;
                                    name = "Magic Staff";
                                }
                                chestTile.AddItem(new WeaponItem(name, rarity, damage));
                            }

                            if (itemChance < 0.1f)
                            {
                                double quality = random.NextDouble();
                                int defense;
                                string name;
                                if (quality < 0.5f)
                                {
                                    defense = 1;
                                    name = "Duct tape";
                                }
                                else if (quality < 0.75f)
                                {
                                    defense = 3;
                                    name = "Cloth";
                                }
                                else if (quality < 0.9f)
                                {
                                    defense = 4;
                                    name = "Chainmail";
                                }
                                else
                                {
                                    defense = 6;
                                    name = "Plate";
                                }
                                chestTile.AddItem(new ArmorItem($"{name} Boots", rarity, ArmorType.Boots, defense));
                            }
                            else if (itemChance < 0.2f)
                            {
                                double quality = random.NextDouble();
                                int defense;
                                string name;
                                if (quality < 0.5f)
                                {
                                    defense = 1;
                                    name = "Duct tape";
                                }
                                else if (quality < 0.75f)
                                {
                                    defense = 3;
                                    name = "Cloth";
                                }
                                else if (quality < 0.9f)
                                {
                                    defense = 4;
                                    name = "Chainmail";
                                }
                                else
                                {
                                    defense = 6;
                                    name = "Plate";
                                }
                                chestTile.AddItem(new ArmorItem($"{name} Helmet", rarity, ArmorType.Helmet, defense));
                            }
                            else if (itemChance < 0.3f)
                            {
                                double quality = random.NextDouble();
                                int defense;
                                string name;
                                if (quality < 0.5f)
                                {
                                    defense = 1;
                                    name = "Duct tape";
                                }
                                else if (quality < 0.75f)
                                {
                                    defense = 3;
                                    name = "Cloth";
                                }
                                else if (quality < 0.9f)
                                {
                                    defense = 4;
                                    name = "Chainmail";
                                }
                                else
                                {
                                    defense = 6;
                                    name = "Plate";
                                }
                                chestTile.AddItem(new ArmorItem($"{name} Leggings", rarity, ArmorType.Leggings, defense));
                            }
                            else if (itemChance < 0.4f)
                            {
                                double quality = random.NextDouble();
                                int defense;
                                string name;
                                if (quality < 0.5f)
                                {
                                    defense = 1;
                                    name = "Duct tape";
                                }
                                else if (quality < 0.75f)
                                {
                                    defense = 3;
                                    name = "Cloth";
                                }
                                else if (quality < 0.9f)
                                {
                                    defense = 4;
                                    name = "Chainmail";
                                }
                                else
                                {
                                    defense = 6;
                                    name = "Plate";
                                }
                                chestTile.AddItem(new ArmorItem($"{name} Chestplate", rarity, ArmorType.Chestplate, defense));
                            }

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
            Random random = new Random();
            Vector2 position;
            do
            {
                position = random.NextVector2(new Vector2(2, 2), new Vector2(Width - 3, Height - 3));
            } while (GetTile(position).Collidable);
            return position;
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
