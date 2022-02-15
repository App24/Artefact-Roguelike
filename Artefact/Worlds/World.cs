﻿using Artefact.Entities;
using Artefact.Tiles;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Worlds
{
    internal abstract class World
    {
        public int Width { get; }
        public int Height { get; }

        private Tile[] tiles;

        public List<Tile> Tiles { get { return new List<Tile>(tiles); } }

        private List<Entity> entities = new List<Entity>();

        public static World Instance { get; set; }

        public bool QuitUpdate { get; set; }

        private static List<ConsoleColor> lightColors = new List<ConsoleColor>()
        {
            ConsoleColor.White,
            ConsoleColor.Yellow,
            ConsoleColor.Gray,
            ConsoleColor.Cyan
        };

        protected Random Random { get; }

        public int Seed { get; }

        public const int TILE_CHAR_WIDTH = 2;

        const int TILE_SIGHT = 5;

        public World(int width, int height, int checkTilesAmount, int seed)
        {
            Width = width;
            Height = height;
            tiles = new Tile[Width * Height];
            Random = new Random(seed);
            Seed = seed;
            GenerateWorld(checkTilesAmount);
        }

        private void GenerateWorld(int checkTilesAmount)
        {
            GenerateTiles();

            for (int i = 0; i < checkTilesAmount; i++)
            {
                CheckTiles();
            }

            GenerateFeatures();

            AddEntity(PlayerEntity.Instance);
        }

        protected abstract void GenerateTiles();
        protected abstract void CheckTiles();
        protected abstract void GenerateFeatures();
        public abstract void PlacePlayer();

        public void PrintTiles()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    PrintTile(new Vector2i(x, y));
                }
            }
            foreach (Entity entity in entities)
            {
                PrintEntity(entity);
            }
            Console.ResetColor();
            PlayerStats.DrawStats();
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
                Tile tile = GetTile(entity.Position);
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
                    PrintTile(previousPosition);
                    List<Entity> otherEntities = entities.FindAll(e => e.Position == entity.Position);
                    foreach(Entity otherEntity in otherEntities)
                    {

                    }
                }


                PrintEntity(entity);
            }

            Console.ResetColor();
            PlayerStats.DrawStats();
            Console.CursorTop = PlayerEntity.Instance.Position.Y;
            if (Console.CursorTop >= Console.WindowHeight - TILE_SIGHT)
            {
                Console.CursorTop += TILE_SIGHT;
            }else if(Console.CursorTop > TILE_SIGHT-1)
            {
                Console.CursorTop -= TILE_SIGHT;
            }
        }

        private void PrintTile(Vector2i position)
        {
            Tile tile = GetTile(position);
            Console.CursorLeft = position.X * TILE_CHAR_WIDTH;
            Console.CursorTop = position.Y;
            Console.BackgroundColor = tile.BackgroundColor;
            Console.ForegroundColor = tile.ForegroundColor;
            for (int i = 0; i < TILE_CHAR_WIDTH; i++)
            {
                Console.Write(tile.Representation);
            }
            Console.ResetColor();
        }

        private void PrintEntity(Entity entity)
        {
            Tile tile = GetTile(entity.Position);

            Console.CursorLeft = entity.Position.X * TILE_CHAR_WIDTH;
            Console.CursorTop = entity.Position.Y;

            if (lightColors.Contains(tile.BackgroundColor))
                Console.ForegroundColor = ConsoleColor.Black;
            else
                Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = tile.BackgroundColor;
            Console.Write(entity.Representation);
            Console.ResetColor();
        }

        protected int GetIndex(Vector2i position)
        {
            return position.X + Width * position.Y;
        }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        #region Tile Related

        protected T SetTile<T>(Vector2i position, T tile) where T : Tile
        {
            if (position.X < 0 || position.Y < 0 || position.X >= Width || position.Y >= Height)
            {
                return null;
            }
            T t = (T)tile.Clone();
            tiles[GetIndex(position)] = t;
            return t;
        }

        public Tile GetTile(Vector2i position)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= Width || position.Y >= Height)
            {
                return null;
            }
            return tiles[GetIndex(position)];
        }

        protected Tile GetTile(Vector2i position, Direction direction)
        {
            position = new Vector2i(position);
            if (direction.HasFlag(Direction.Left))
            {
                position += Vector2i.Left;
            }

            if (direction.HasFlag(Direction.Right))
            {
                position += Vector2i.Right;
            }

            if (direction.HasFlag(Direction.Up))
            {
                position += Vector2i.Up;
            }

            if (direction.HasFlag(Direction.Down))
            {
                position += Vector2i.Down;
            }

            return GetTile(position);
        }

        /// <summary>
        /// Check if tiles in a 3x3 grid around a position are the same
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="tiles"></param>
        /// <returns></returns>
        protected bool CheckTilesAroundSame(Vector2i position, bool ignoreNull, params Tile[] tiles)
        {
            // n = negative
            // p = positive
            // x = x axis
            // y = y axis
            Tile nxTile = GetTile(position, Direction.Left);
            Tile pxTile = GetTile(position, Direction.Right);
            Tile nyTile = GetTile(position, Direction.Up);
            Tile pyTile = GetTile(position, Direction.Down);
            Tile nxnyTile = GetTile(position, Direction.Up | Direction.Left);
            Tile nxpyTile = GetTile(position, Direction.Down | Direction.Left);
            Tile pxnyTile = GetTile(position, Direction.Up | Direction.Left);
            Tile pxpyTile = GetTile(position, Direction.Down | Direction.Right);

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

        /// <summary>
        /// Check if tiles above, below and to the sides are the same
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ignoreNull"></param>
        /// <param name="tiles"></param>
        /// <returns></returns>
        protected bool CheckTilesDirectAroundSame(Vector2i position, bool ignoreNull, params Tile[] tiles)
        {
            // n = negative
            // p = positive
            // x = x axis
            // y = y axis
            Tile nxTile = GetTile(position, Direction.Left);
            Tile pxTile = GetTile(position, Direction.Right);
            Tile nyTile = GetTile(position, Direction.Up);
            Tile pyTile = GetTile(position, Direction.Down);

            List<Tile> tileList = new List<Tile>(tiles);

            if (!ignoreNull)
            {
                return tileList.Contains(nxTile) &&
                    tileList.Contains(pxTile) &&
                    tileList.Contains(nyTile) &&
                    tileList.Contains(pyTile);
            }
            else
            {
                return (tileList.Contains(nxTile) || nxTile == null) &&
                    (tileList.Contains(pxTile) || pxTile == null) &&
                    (tileList.Contains(nyTile) || nyTile == null) &&
                    (tileList.Contains(pyTile) || pyTile == null);
            }
        }

        protected void ReplaceTilesInRange(Vector2i position, int radius, float variantion, Tile tileToReplace, Tile tile)
        {
            ReplaceTilesInRange(position, radius, variantion, new Tile[] { tileToReplace }, new Tile[] { tile });
        }

        protected void ReplaceTilesInRange(Vector2i position, int radius, float variantion, Tile[] tilesToReplace, Tile tile)
        {
            ReplaceTilesInRange(position, radius, variantion, tilesToReplace, new Tile[] { tile });
        }

        protected void ReplaceTilesInRange(Vector2i position, int radius, float variantion, Tile tileToReplace, Tile[] tiles)
        {
            ReplaceTilesInRange(position, radius, variantion, new Tile[] { tileToReplace }, tiles);
        }

        protected void ReplaceTilesInRange(Vector2i position, int radius, float variantion, Tile[] tilesToReplace, Tile[] tiles)
        {
            List<Tile> tilesToReplaceList = new List<Tile>(tilesToReplace);
            for (int y = -radius; y < radius; y++)
            {
                for (int x = -radius; x < radius; x++)
                {
                    Tile tileToReplace = GetTile(new Vector2i(position.X + x, position.Y + y));
                    if (tilesToReplaceList.Contains(tileToReplace))
                    {
                        if ((x * x + y * y) <= radius * Math.Clamp(Random.NextDouble(), variantion, 1f))
                        {
                            Tile tile = tiles[Random.Next(tiles.Length)].Clone();
                            if (tile is ReplaceBackgroundTile)
                            {
                                tile.BackgroundColor = tileToReplace.BackgroundColor;
                                if (tile.ForegroundColor == tileToReplace.ForegroundColor)
                                    tile.ForegroundColor = ConsoleColor.Green;
                            }
                            SetTile(new Vector2i(position.X + x, position.Y + y), tile);
                        }
                    }
                }
            }
        }

        protected void SetTilesInRange(Vector2i position, int radius, float variantion, Tile tile)
        {
            SetTilesInRange(position, radius, variantion, new Tile[] { tile });
        }

        protected void SetTilesInRange(Vector2i position, int radius, float variantion, params Tile[] tiles)
        {
            ReplaceTilesInRange(position, radius, variantion, Tile.Tiles.ToArray(), tiles);
        }

        protected Vector2i GetRandomTilePos(params Tile[] tiles)
        {
            List<Tile> tilesList = new List<Tile>(tiles);
            Vector2i pos = Vector2i.Zero;

            if (!Tiles.Exists(t => tilesList.Contains(t))) return new Vector2i(-1, -1);

            while (true)
            {
                pos = Random.NextVector2i(new Vector2i(Width, Height));
                if (tilesList.Contains(GetTile(pos)))
                    break;
            }

            return pos;
        }

        #endregion
    }
}
