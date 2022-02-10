using Artefact.Entities;
using Artefact.Tiles;
using Artefact.Utils;
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

        public CaveWorld(int width, int height, World world, int seed) : base(width, height, 3, seed)
        {
            ExitLadders.ForEach(t =>
            {
                t.WorldToGo = world;
            });
            FindPlayerPosition();
        }

        protected override void GenerateTiles()
        {
            GenerateStone();
            GenerateCave();
        }

        private void GenerateStone()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetTile(new Vector2i(x, y), Tile.StoneTile);
                }
            }
        }

        private void GenerateCave()
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2i pos = GetRandomTilePos(Tile.StoneTile);

                ReplaceTilesInRange(pos, 5, 0.25f, Tile.StoneTile, Tile.DeepMountainTile);
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
                LadderTile ladderTile = SetTile(new Vector2i(i, 0), Tile.LadderTile);
                ExitLadders.Add(ladderTile);
            }
            if (Random.NextDouble() < 0.3f)
            {
                int width = Random.Next(20, 40);
                int height = Random.Next(20, 40);

                CaveWorld caveWorld = new CaveWorld(width, height, this, Seed >= 0 ? Seed + 1 : Seed);

                for (int i = 0; i < 2; i++)
                {
                    LadderTile ladderTile = SetTile(new Vector2i(Width - 1 - i, Height - 1), Tile.LadderTile);
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

        private void FindPlayerPosition()
        {
            origPlayerPos = new Vector2i(0, 1);
        }
    }
}
