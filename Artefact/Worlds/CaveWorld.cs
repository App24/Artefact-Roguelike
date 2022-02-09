using Artefact.Entities;
using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Worlds
{
    internal class CaveWorld : World
    {
        List<LadderTile> exitLadders = new List<LadderTile>();

        public CaveWorld(int width, int height, int checkTilesAmount, World world) : base(width, height, checkTilesAmount)
        {
            exitLadders.ForEach(t =>
              {
                  t.WorldToGo = world;
              });
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
                ladderTile.PlayerPosWorld = new Vector2i(PlayerEntity.Instance.Position);
                exitLadders.Add(ladderTile);
            }
            Instance.QuitUpdate = true;
        }

        protected override void SpawnPlayer()
        {
            PlayerEntity.Instance.Position = new Vector2i(0, 1);
        }
    }
}
