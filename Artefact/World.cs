using Artefact.Entities;
using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact
{
    class World
    {
        public int Width { get; }
        public int Height { get; }

        Tile[] tiles;

        List<Entity> entities = new List<Entity>();

        public static World Instance { get; private set; }

        static List<ConsoleColor> lightColors = new List<ConsoleColor>()
        {
            ConsoleColor.White,
            ConsoleColor.Yellow,
            ConsoleColor.Gray,
        };

        public World()
        {
            Instance = this;
            Width = 60;
            Height = 60;
            tiles = new Tile[Width * Height];
            SpawnTiles();
            SpawnPlayer();
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

        public void Update()
        {
            foreach (Entity entity in entities)
            {
                Vector2i previousPosition = entity.Position;
                entity.Move();
                if (entity.Position != previousPosition)
                {
                    PrintTile(previousPosition.X, previousPosition.Y);
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

        void SpawnTiles()
        {
            PlaceGrass();
            PlaceWater();
            PlaceMountains();

            for (int i = 0; i < 5; i++)
            {
                CheckTiles();
            }

            PlaceFeatures();
        }

        void PlaceFeatures()
        {
            PlaceFlowers();
            PlaceTrees();
        }

        void PlaceFlowers()
        {
            Random random = new Random();
            for (int y = 0; y < Height; y += 5)
            {
                for (int x = 0; x < Width; x += 5)
                {
                    if (GetTile(x, y) == Tile.GrassTile)
                    {
                        if (random.NextDouble() < 0.3f)
                        {
                            Tile tile = Tile.RoseFlowerTile;
                            if (random.Next(2) == 1)
                            {
                                tile = Tile.BluebellFlowerTile;
                            }
                            ReplaceTilesInRange(x, y, 5, true, Tile.GrassTile, tile);
                        }
                    }
                }
            }
        }

        void PlaceTrees()
        {
            Random random = new Random();
            int amountTrees = random.Next(15, 25);
            int i = 0;
            while (i < amountTrees)
            {
                Vector2i pos = GetRandomTilePos(Tile.GrassTile);
                int size = random.Next(2, 4);
                bool cont = false;
                for (int j = 0; j < size * size; j++)
                {
                    Tile tile = GetTile(pos.X + (j % size), pos.Y + (j / size));
                    if (tile != Tile.GrassTile)
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

        public void PrintTiles()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    PrintTile(x, y);
                    foreach (Entity entity in entities)
                    {
                        if (entity.Position == new Vector2i(x, y))
                        {
                            Console.CursorLeft = x * 2;
                            Console.CursorTop = y;
                            if (lightColors.Contains(GetTile(x, y).BackgroundColor))
                                Console.ForegroundColor = ConsoleColor.Black;
                            else
                                Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(entity.Representation);
                        }
                    }
                }
            }
            Console.ResetColor();
        }

        void PrintTile(int x, int y)
        {
            Tile tile = GetTile(x, y);
            Console.CursorLeft = x * 2;
            Console.CursorTop = y;
            Console.BackgroundColor = tile.BackgroundColor;
            Console.ForegroundColor = tile.ForegroundColor;
            Console.Write(tile.Representation);
            Console.Write(tile.Representation);
        }

        void CheckTiles()
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

        int GetIndex(int x, int y)
        {
            return x + Width * y;
        }
        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        void SpawnPlayer()
        {
            PlayerEntity playerEntity = new PlayerEntity();
            Tile tile = GetTile(playerEntity.Position.X, playerEntity.Position.Y);
            while (tile.Collidable)
            {
                playerEntity.Position.X++;
                tile = GetTile(playerEntity.Position.X, playerEntity.Position.Y);
            }
            entities.Add(playerEntity);
        }

        #region Tile Related

        void SetTile(int x, int y, Tile tile)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return;
            }
            tiles[GetIndex(x, y)] = tile.Clone();
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return null;
            }
            return tiles[GetIndex(x, y)];
        }

        bool AreTilesAroundSame(int x, int y, bool ignoreNull, params Tile[] tiles)
        {
            // n = negative
            // p = positive
            // x = x axis
            // y = y axis
            Tile nxTile = GetTile(x - 1, y);
            Tile pxTile = GetTile(x + 1, y);
            Tile nyTile = GetTile(x, y - 1);
            Tile pyTile = GetTile(x, y + 1);
            Tile nxnyTile = GetTile(x - 1, y - 1);
            Tile nxpyTile = GetTile(x - 1, y + 1);
            Tile pxnyTile = GetTile(x + 1, y - 1);
            Tile pxpyTile = GetTile(x + 1, y + 1);

            List<Tile> tileList = new List<Tile>(tiles);

            if (!ignoreNull)
            {
                return tileList.Contains(nxTile) &&
                    tileList.Contains(pxTile) &&
                    tileList.Contains(nyTile) &&
                    tileList.Contains(pyTile) &&
                    tileList.Contains(nxnyTile) &&
                    tileList.Contains(nxpyTile) &&
                    tileList.Contains(pxnyTile) &&
                    tileList.Contains(pxpyTile);
            }
            else
            {
                return (tileList.Contains(nxTile) || nxTile == null) &&
                    (tileList.Contains(pxTile) || pxTile == null) &&
                    (tileList.Contains(nyTile) || nyTile == null) &&
                    (tileList.Contains(pyTile) || pyTile == null) &&
                    (tileList.Contains(nxnyTile) || nxnyTile == null) &&
                    (tileList.Contains(nxpyTile) || nxpyTile == null) &&
                    (tileList.Contains(pxnyTile) || pxnyTile == null) &&
                    (tileList.Contains(pxpyTile) || pxpyTile == null);
            }
        }

        void ReplaceTilesInRange(int x, int y, int radius, bool variantion, Tile tileToReplace, Tile tile)
        {
            ReplaceTilesInRange(x, y, radius, variantion, new Tile[] { tileToReplace }, new Tile[] { tile });
        }

        void ReplaceTilesInRange(int x, int y, int radius, bool variantion, Tile[] tilesToReplace, Tile tile)
        {
            ReplaceTilesInRange(x, y, radius, variantion, tilesToReplace, new Tile[] { tile });
        }

        void ReplaceTilesInRange(int x, int y, int radius, bool variantion, Tile tileToReplace, Tile[] tiles)
        {
            ReplaceTilesInRange(x, y, radius, variantion, new Tile[] { tileToReplace }, tiles);
        }

        void ReplaceTilesInRange(int x, int y, int radius, bool variantion, Tile[] tilesToReplace, Tile[] tiles)
        {
            Random random = new Random();
            List<Tile> tilesToReplaceList = new List<Tile>(tilesToReplace);
            for (int replaceY = -radius; replaceY < radius; replaceY++)
            {
                for (int replaceX = -radius; replaceX < radius; replaceX++)
                {
                    if (tilesToReplaceList.Contains(GetTile(x + replaceX, y + replaceY)))
                    {
                        float variant = variantion ? (float)Math.Clamp(random.NextDouble(), 0.25f, 1f) : 1;
                        if ((replaceX * replaceX + replaceY * replaceY) <= radius * variant)
                        {
                            SetTile(x + replaceX, y + replaceY, tiles[random.Next(tiles.Length)]);
                        }
                    }
                }
            }
        }

        Vector2i GetRandomTilePos(params Tile[] tiles)
        {
            List<Tile> tilesList = new List<Tile>(tiles);
            Random random = new Random();
            int x = 0;
            int y = 0;

            while (true)
            {
                x = random.Next(Width);
                y = random.Next(Height);
                if (tilesList.Contains(GetTile(x, y)))
                    break;
            }

            return new Vector2i(x, y);
        }

        #endregion
    }
}
