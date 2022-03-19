using Artefact.Entities;
using Artefact.Tiles;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Worlds
{
    internal class OverworldWorld : World
    {
        public OverworldWorld(int width, int height) : base(width, height, 5)
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
                    SetTile(new Vector2i(x, y), Tile.GrassTile);
                }
            }

            for (int i = 0; i < Random.Next(40, 60); i++)
            {
                Vector2i pos = Random.NextVector2i(new Vector2i(Width, Height));

                SetTilesInRange(pos, 30, 0.5f, Tile.DarkGrassTile);
            }
        }

        private void GenerateWater()
        {
            int amount = Random.Next(10, 15);
            for (int i = 0; i < amount; i++)
            {
                Vector2i pos = Random.NextVector2i(new Vector2i(Width, Height));
                int radius = Random.Next(10, 15);

                SetTilesInRange(pos, radius, 0.5f, Tile.WaterTile);
            }
        }

        private void GenerateMountains()
        {
            int amount = Random.Next(5, 8);
            for (int i = 0; i < amount; i++)
            {
                Vector2i pos = Random.NextVector2i(new Vector2i(Width, Height));
                int radius = Random.Next(20, 25);

                SetTilesInRange(pos, radius, 0.5f, Tile.MountainTile);
            }
        }

        protected override void GenerateFeatures()
        {
            GenerateFlowers();
            GenerateTrees();
            GenerateCaves();

            SpawnEnemies();
        }

        private void GenerateFlowers()
        {
            List<Tile> flowerTiles = Tile.FlowerTiles;
            for (int i = 0; i < Random.Next(40, 50); i++)
            {
                Vector2i pos = GetRandomTilePos(Tile.GrassTiles.ToArray());
                Tile tile = GetTile(pos);

                if (Tile.GrassTiles.Contains(tile))
                {
                    Tile flowerTile = flowerTiles[Random.Next(flowerTiles.Count)];
                    ReplaceTilesInRange(pos, 5, 0.25f, Tile.GrassTiles.ToArray(), flowerTile);
                }
            }
        }

        private void GenerateTrees()
        {
            int amountTrees = Random.Next(20, 30);
            int i = 0;
            while (i < amountTrees)
            {
                Vector2i pos = GetRandomTilePos(Tile.GrassTiles.ToArray());
                int size = Random.Next(1, 3);
                bool cont = false;
                for (int j = 0; j < size * size; j++)
                {
                    if (!CheckTilesAroundSame(pos + new Vector2i(j % size, j / size), true, Tile.GrassTiles.ToArray()))
                    {
                        cont = true;
                        break;
                    }
                }

                if (cont) continue;

                for (int j = 0; j < size * size; j++)
                {
                    SetTile(pos + new Vector2i(j % size, j / size), Tile.TreeBarkTile);
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
                Vector2i pos = GetRandomTilePos(Tile.GrassTiles.ToArray());
                int size = 2;
                bool cont = false;
                for (int j = 0; j < size * size; j++)
                {
                    if (!CheckTilesAroundSame(pos + new Vector2i(j % size, j / size), true, Tile.GrassTiles.ToArray()))
                    {
                        cont = true;
                        break;
                    }
                }

                if (cont) continue;

                int width = Random.Next(20, 40);
                int height = Random.Next(20, 40);

                CaveWorld caveWorld = new CaveWorld(width, height, this, Seed);

                for (int j = 0; j < size * size; j++)
                {
                    CaveTile caveTile = SetTile(pos + new Vector2i(j % size, j / size), Tile.CaveTile);
                    if (caveTile != null)
                        caveTile.CaveWorld = caveWorld;
                }

                i++;
            }
        }

        private void SpawnEnemies()
        {
            TestEnemy enemy = new TestEnemy();
            enemy.Position = GetRandomPosition();
            AddEntity(enemy);
        }

        #endregion

        protected override void CheckTiles()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector2i position = new Vector2i(x, y);
                    Tile nxTile = GetTile(position, Direction.Left);
                    Tile pxTile = GetTile(position, Direction.Right);
                    Tile nyTile = GetTile(position, Direction.Up);
                    Tile pyTile = GetTile(position, Direction.Down);
                    Tile nxnyTile = GetTile(position, Direction.Up | Direction.Left);
                    Tile nxpyTile = GetTile(position, Direction.Down | Direction.Left);
                    Tile pxnyTile = GetTile(position, Direction.Up | Direction.Left);
                    Tile pxpyTile = GetTile(position, Direction.Down | Direction.Right);
                    Tile currentTile = GetTile(position);
                    if (currentTile == Tile.WaterTile)
                    {
                        if (CheckTilesAroundSame(position, true, Tile.WaterTile, Tile.DeepWaterTile, Tile.MountainTile, Tile.DeepMountainTile))
                        {
                            SetTile(position, Tile.DeepWaterTile);
                        }
                        else
                        {
                            ReplaceTilesInRange(position, 6, 0.25f, Tile.GrassTiles.ToArray(), Tile.SandTile);
                        }
                    }
                    else if (Tile.GrassTiles.Contains(currentTile))
                    {
                        if (CheckTilesDirectAroundSame(position, true, Tile.WaterTile, Tile.DeepWaterTile))
                        {
                            SetTile(position, Tile.WaterTile);
                        }
                        else if (CheckTilesDirectAroundSame(position, true, Tile.DeepMountainTile, Tile.MountainTile))
                        {
                            SetTile(position, Tile.MountainTile);
                        }
                    }
                    else if (currentTile == Tile.MountainTile)
                    {
                        if (CheckTilesAroundSame(position, true, Tile.MountainTile, Tile.DeepMountainTile))
                        {
                            SetTile(position, Tile.DeepMountainTile);
                        }
                    }
                }
            }
        }

        public override void PlacePlayer()
        {
            PlayerEntity playerEntity = PlayerEntity.Instance;
            playerEntity.Position = Vector2i.Zero;
            Tile tile = GetTile(playerEntity.Position);
            while (tile.Collidable)
            {
                playerEntity.Position.X++;
                tile = GetTile(playerEntity.Position);
            }
        }
    }
}
