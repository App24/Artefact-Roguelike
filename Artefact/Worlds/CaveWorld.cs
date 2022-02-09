using Artefact.Entities;
using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Worlds
{
    internal class CaveWorld : World
    {
        public List<LadderTile> ExitLadders { get; } = new List<LadderTile>();
        public List<LadderTile> NextLadders { get; } = new List<LadderTile>();

        private Vector2i origPlayerPos;

        public CaveWorld(int width, int height, int checkTilesAmount, World world) : this(width, height, checkTilesAmount, world, -1)
        {
        }

        public CaveWorld(int width, int height, int checkTilesAmount, World world, int seed) : base(width, height, checkTilesAmount, seed)
        {
            ExitLadders.ForEach(t =>
            {
                t.WorldToGo = world;
            });
            origPlayerPos = new Vector2i(0, 1);
        }

        protected override void GenerateTiles()
        {
            GenerateStone();
        }

        private void GenerateStone()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetTile(x, y, Tile.StoneTile);
                }
            }
        }

        protected override void CheckTiles()
        {

        }

        protected override void GenerateFeatures()
        {
            GenerateLadder();
        }

        private void GenerateLadder()
        {
            for (int i = 0; i < 2; i++)
            {
                LadderTile ladderTile = SetTile(i, 0, Tile.LadderTile);
                ExitLadders.Add(ladderTile);
            }
            if (Random.NextDouble() < 0.99f)
            {
                CaveWorld caveWorld = new CaveWorld(20, 20, 3, this, seed >= 0 ? seed + 1 : seed);

                for (int i = 0; i < 2; i++)
                {
                    LadderTile ladderTile = SetTile(Width - 1 - i, Height - 1, Tile.LadderTile);
                    ladderTile.WorldToGo = caveWorld;
                    ladderTile.PlayerPosWorld = new Vector2i(caveWorld.origPlayerPos);
                    ladderTile.GoingDown = true;
                    NextLadders.Add(ladderTile);
                }
            }
        }

        public override void PlacePlayer()
        {
            PlayerEntity.Instance.Position = new Vector2i(origPlayerPos);
        }
    }
}
