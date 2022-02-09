using Artefact.Entities;
using Artefact.Tiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Worlds
{
    internal class CaveWorld : World
    {
        public CaveWorld(int width, int height, int checkTilesAmount) : base(width, height, checkTilesAmount)
        {

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
            LadderTile ladderTile = SetTile(0, 0, Tile.LadderTile);
            ladderTile.PlayerPosWorld = new Vector2i(PlayerEntity.Instance.Position);
            ladderTile.WorldToGo = OverworldWorld.OverworldInstance;
        }

        protected override void SpawnPlayer()
        {
            PlayerEntity.Instance.Position = new Vector2i(0, 1);
        }
    }
}
