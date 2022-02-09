using Artefact.Entities;
using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Worlds
{
    internal class OverworldWorld : World
    {
        public static OverworldWorld OverworldInstance { get; private set; }

        public OverworldWorld(int width, int height):base(width, height, 5)
        {
            OverworldInstance = this;
        }

        #region World Gen
        protected override void SpawnTiles()
        {
            PlaceGrass();
            PlaceWater();
            PlaceMountains();
        }

        void PlaceGrass()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetTile(x, y, Tile.GrassTile);
                }
            }
        }

        void PlaceWater()
        {
            Random random = new Random();
            int amount = random.Next(10, 15);
            for (int i = 0; i < amount; i++)
            {
                int waterX = random.Next(Width);
                int waterY = random.Next(Height);
                int radius = random.Next(10, 15);

                for (int y = -radius; y < radius; y++)
                {
                    for (int x = -radius; x < radius; x++)
                    {
                        if ((x * x + y * y) <= radius * Math.Clamp(random.NextDouble(), 0.5f, 1f))
                            SetTile(waterX + x, waterY + y, Tile.WaterTile);
                    }
                }
            }
        }

        void PlaceMountains()
        {
            Random random = new Random();
            int amount = random.Next(5, 8);
            for (int i = 0; i < amount; i++)
            {
                int waterX = random.Next(Width);
                int waterY = random.Next(Height);
                int radius = random.Next(20, 25);

                for (int y = -radius; y < radius; y++)
                {
                    for (int x = -radius; x < radius; x++)
                    {
                        if ((x * x + y * y) <= radius * Math.Clamp(random.NextDouble(), 0.5f, 1f))
                            SetTile(waterX + x, waterY + y, Tile.MountainTile);
                    }
                }
            }
        }

        protected override void SpawnFeatures()
        {
            PlaceFlowers();
            PlaceTrees();
            PlaceCaves();
        }

        void PlaceFlowers()
        {
            Random random = new Random();
            List<Tile> flowerTiles = Tile.Tiles.FindAll(t => t is FlowerTile);
            for (int y = 0; y < Height; y += 5)
            {
                for (int x = 0; x < Width; x += 5)
                {
                    if (GetTile(x, y) == Tile.GrassTile)
                    {
                        if (random.NextDouble() < 0.3f)
                        {
                            Tile tile = flowerTiles[random.Next(flowerTiles.Count)];
                            ReplaceTilesInRange(x, y, 5, true, Tile.GrassTile, tile);
                        }
                    }
                }
            }
        }

        void PlaceTrees()
        {
            Random random = new Random();
            int amountTrees = random.Next(20, 30);
            int i = 0;
            while (i < amountTrees)
            {
                Vector2i pos = GetRandomTilePos(Tile.GrassTile);
                int size = random.Next(1, 3);
                bool cont = false;
                for (int j = 0; j < size * size; j++)
                {
                    if (!AreTilesAroundSame(pos.X + (j % size), pos.Y + (j / size), true, Tile.GrassTile))
                    {
                        cont = true;
                        break;
                    }
                }

                if (cont) continue;

                for (int j = 0; j < size * size; j++)
                {
                    SetTile(pos.X + (j % size), pos.Y + (j / size), Tile.TreeBarkTile);
                }

                i++;
            }
        }

        void PlaceCaves()
        {
            Random random = new Random();
            int amountTrees = random.Next(5, 10);
            int i = 0;
            while (i < amountTrees)
            {
                Vector2i pos = GetRandomTilePos(Tile.GrassTile);
                int size = 2;
                bool cont = false;
                for (int j = 0; j < size * size; j++)
                {
                    if (!AreTilesAroundSame(pos.X + (j % size), pos.Y + (j / size), true, Tile.GrassTile))
                    {
                        cont = true;
                        break;
                    }
                }

                if (cont) continue;

                for (int j = 0; j < size * size; j++)
                {
                    SetTile(pos.X + (j % size), pos.Y + (j / size), Tile.CaveTile);
                }

                i++;
            }
        }

        #endregion

        protected override void CheckTiles()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Tile nxTile = GetTile(x - 1, y);
                    Tile pxTile = GetTile(x + 1, y);
                    Tile nyTile = GetTile(x, y - 1);
                    Tile pyTile = GetTile(x, y + 1);
                    Tile nxnyTile = GetTile(x - 1, y - 1);
                    Tile nxpyTile = GetTile(x - 1, y + 1);
                    Tile pxnyTile = GetTile(x + 1, y - 1);
                    Tile pxpyTile = GetTile(x + 1, y + 1);
                    Tile currentTile = GetTile(x, y);
                    if (currentTile == Tile.WaterTile)
                    {
                        if (AreTilesAroundSame(x, y, true, Tile.WaterTile, Tile.DeepWaterTile))
                        {
                            SetTile(x, y, Tile.DeepWaterTile);
                        }
                        else
                        {
                            ReplaceTilesInRange(x, y, 6, true, Tile.GrassTile, Tile.SandTile);
                        }
                    }
                    else if (currentTile == Tile.GrassTile)
                    {
                        if (AreTilesAroundSame(x, y, true, Tile.WaterTile, Tile.DeepWaterTile))
                        {
                            SetTile(x, y, Tile.WaterTile);
                        }else if(AreTilesAroundSame(x, y, true, Tile.DeepMountainTile, Tile.MountainTile))
                        {
                            SetTile(x, y, Tile.MountainTile);
                        }
                    }
                    else if (currentTile == Tile.MountainTile)
                    {
                        if (AreTilesAroundSame(x, y, true, Tile.MountainTile, Tile.DeepMountainTile))
                        {
                            SetTile(x, y, Tile.DeepMountainTile);
                        }
                    }
                }
            }
        }

        protected override void SpawnPlayer()
        {
            PlayerEntity playerEntity = PlayerEntity.Instance;
            playerEntity.Position = new Vector2i(0, 0);
            Tile tile = GetTile(playerEntity.Position.X, playerEntity.Position.Y);
            while (tile.Collidable)
            {
                playerEntity.Position.X++;
                tile = GetTile(playerEntity.Position.X, playerEntity.Position.Y);
            }
        }
    }
}
