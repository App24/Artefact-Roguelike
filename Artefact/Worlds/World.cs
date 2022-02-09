﻿using Artefact.Entities;
using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Worlds
{
    abstract class World
    {
        public int Width { get; }
        public int Height { get; }

        Tile[] tiles;

        public List<Tile> Tiles { get { return new List<Tile>(tiles); } }

        List<Entity> entities = new List<Entity>();

        public static World Instance { get; set; }

        public bool QuitUpdate { get; set; }


        static List<ConsoleColor> lightColors = new List<ConsoleColor>()
        {
            ConsoleColor.White,
            ConsoleColor.Yellow,
            ConsoleColor.Gray,
        };

        public World(int width, int height, int checkTilesAmount)
        {
            Width = width;
            Height = height;
            tiles = new Tile[Width * Height];
            SpawnTiles();

            for (int i = 0; i < checkTilesAmount; i++)
            {
                CheckTiles();
            }

            SpawnFeatures();

            SpawnPlayer();
            AddEntity(PlayerEntity.Instance);

            PrintTiles();
        }

        protected abstract void SpawnTiles();
        protected abstract void CheckTiles();
        protected abstract void SpawnFeatures();
        protected abstract void SpawnPlayer();

        public void PrintTiles()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    PrintTile(x, y);
                }
            }
            foreach (Entity entity in entities)
            {
                PrintEntity(entity);
            }
            Console.CursorLeft = Width * 2;
            Console.ResetColor();
        }

        public void Update()
        {
            foreach (Entity entity in entities)
            {
                entity.Update();
            }
            foreach (Entity entity in entities)
            {
                Vector2i previousPosition = new Vector2i(entity.Position);
                entity.Move();
                Tile tile = GetTile(entity.Position.X, entity.Position.Y);
                if (tile.Collidable)
                    entity.Position = previousPosition;
                tile.OnCollide(entity);

                if (QuitUpdate)
                {
                    QuitUpdate = false;
                    break;
                }

                if (entity.Position != previousPosition)
                {
                    PrintTile(previousPosition.X, previousPosition.Y);
                }

                PrintEntity(entity);
            }
            Console.CursorLeft = Width * 2;
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
            Console.ResetColor();
        }

        void PrintEntity(Entity entity)
        {
            Tile tile = GetTile(entity.Position.X, entity.Position.Y);

            Console.CursorLeft = entity.Position.X * 2;
            Console.CursorTop = entity.Position.Y;

            if (lightColors.Contains(tile.BackgroundColor))
                Console.ForegroundColor = ConsoleColor.Black;
            else
                Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = tile.BackgroundColor;
            Console.Write(entity.Representation);
            Console.ResetColor();
        }

        protected int GetIndex(int x, int y)
        {
            return x + Width * y;
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        #region Tile Related

        protected T SetTile<T>(int x, int y, T tile) where T : Tile
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return null;
            }
            T t = (T)tile.Clone();
            tiles[GetIndex(x, y)] = t;
            return t;
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return null;
            }
            return tiles[GetIndex(x, y)];
        }

        protected bool AreTilesAroundSame(int x, int y, bool ignoreNull, params Tile[] tiles)
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

        protected void ReplaceTilesInRange(int x, int y, int radius, bool variantion, Tile tileToReplace, Tile tile)
        {
            ReplaceTilesInRange(x, y, radius, variantion, new Tile[] { tileToReplace }, new Tile[] { tile });
        }

        protected void ReplaceTilesInRange(int x, int y, int radius, bool variantion, Tile[] tilesToReplace, Tile tile)
        {
            ReplaceTilesInRange(x, y, radius, variantion, tilesToReplace, new Tile[] { tile });
        }

        protected void ReplaceTilesInRange(int x, int y, int radius, bool variantion, Tile tileToReplace, Tile[] tiles)
        {
            ReplaceTilesInRange(x, y, radius, variantion, new Tile[] { tileToReplace }, tiles);
        }

        protected void ReplaceTilesInRange(int x, int y, int radius, bool variantion, Tile[] tilesToReplace, Tile[] tiles)
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

        protected Vector2i GetRandomTilePos(params Tile[] tiles)
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
