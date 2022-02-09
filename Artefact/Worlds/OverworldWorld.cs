using Artefact.Entities;
using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Worlds
{
    internal class OverworldWorld : World
    {
        public OverworldWorld(int width, int height) : this(width, height, -1)
        {
        }

        public OverworldWorld(int width, int height, int seed) : base(width, height, 5, seed)
        {
        }

        #region World Gen
        protected override void GenerateTiles()
        {
            GenerateGrass();
            GenerateWater();
            GenerateMountains();
        }

        private void GenerateGrass()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetTile(x, y, Tile.GrassTile);
                }
            }
        }

        private void GenerateWater()
        {
            int amount = Random.Next(10, 15);
            for (int i = 0; i < amount; i++)
            {
                int waterX = Random.Next(Width);
                int waterY = Random.Next(Height);
                int radius = Random.Next(10, 15);

                for (int y = -radius; y < radius; y++)
                {
                    for (int x = -radius; x < radius; x++)
                    {
                        if ((x * x + y * y) <= radius * Math.Clamp(Random.NextDouble(), 0.5f, 1f))
                            SetTile(waterX + x, waterY + y, Tile.WaterTile);
                    }
                }
            }
        }

        private void GenerateMountains()
        {
            int amount = Random.Next(5, 8);
            for (int i = 0; i < amount; i++)
            {
                int waterX = Random.Next(Width);
                int waterY = Random.Next(Height);
                int radius = Random.Next(20, 25);

                for (int y = -radius; y < radius; y++)
                {
                    for (int x = -radius; x < radius; x++)
                    {
                        if ((x * x + y * y) <= radius * Math.Clamp(Random.NextDouble(), 0.5f, 1f))
                            SetTile(waterX + x, waterY + y, Tile.MountainTile);
                    }
                }
            }
        }

        protected override void GenerateFeatures()
        {
            GenerateFlowers();
            GenerateTrees();
            GenerateCaves();
        }

        private void GenerateFlowers()
        {
            List<Tile> flowerTiles = Tile.Tiles.FindAll(t => t is FlowerTile);
            for (int y = 0; y < Height; y += 5)
            {
                for (int x = 0; x < Width; x += 5)
                {
                    if (GetTile(x, y) == Tile.GrassTile)
                    {
                        if (Random.NextDouble() < 0.3f)
                        {
                            Tile tile = flowerTiles[Random.Next(flowerTiles.Count)];
                            ReplaceTilesInRange(x, y, 5, true, Tile.GrassTile, tile);
                        }
                    }
                }
            }
        }

        private void GenerateTrees()
        {
            int amountTrees = Random.Next(20, 30);
            int i = 0;
            while (i < amountTrees)
            {
                Vector2i pos = GetRandomTilePos(Tile.GrassTile);
                int size = Random.Next(1, 3);
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

        private void GenerateCaves()
        {
            int amountCaves = Random.Next(5, 10);
            int i = 0;
            while (i < amountCaves)
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

                CaveWorld caveWorld = new CaveWorld(20, 20, 3, this, seed);

                for (int j = 0; j < size * size; j++)
                {
                    CaveTile caveTile = SetTile(pos.X + (j % size), pos.Y + (j / size), Tile.CaveTile);
                    if (caveTile != null)
                        caveTile.CaveWorld = caveWorld;
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
                        }
                        else if (AreTilesAroundSame(x, y, true, Tile.DeepMountainTile, Tile.MountainTile))
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

        public override void PlacePlayer()
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
