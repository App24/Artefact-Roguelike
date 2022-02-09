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

        Vector2i origPlayerPos;

        public CaveWorld(int width, int height, int checkTilesAmount, World world) : base(width, height, checkTilesAmount)
        {
            ExitLadders.ForEach(t =>
            {
                t.WorldToGo = world;
            });
            origPlayerPos = new Vector2i(0, 1);
        }

        protected override void SpawnTiles()
        {
            PlaceStone();
        }

        void PlaceStone()
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

        protected override void SpawnFeatures()
        {
            SpawnLadder();
        }

        void SpawnLadder()
        {
            for (int i = 0; i < 2; i++)
            {
                LadderTile ladderTile = SetTile(i, 0, Tile.LadderTile);
                ExitLadders.Add(ladderTile);
            }
            Random rand = new Random();
            if (rand.NextDouble() < 0.5f)
            {
                CaveWorld caveWorld = new CaveWorld(20, 20, 3, this);
                for (int i = 0; i < 2; i++)
                {
                    LadderTile ladderTile = SetTile(Width - 1 - i, Height - 1, Tile.LadderTile);
                    ladderTile.WorldToGo = caveWorld;
                    ladderTile.PlayerPosWorld = caveWorld.origPlayerPos;
                }
            }
        }

        public override void PlacePlayer()
        {
            PlayerEntity.Instance.Position = new Vector2i(origPlayerPos);
        }
    }
}
