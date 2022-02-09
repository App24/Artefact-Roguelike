using Artefact.Entities;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal class LadderTile : Tile
    {
        public World WorldToGo { get; set; }
        public Vector2i PlayerPosWorld { get; set; }

        public LadderTile() : base("#", ConsoleColor.Gray, ConsoleColor.DarkYellow, true)
        {
        }

        public override void OnCollide(Entity entity)
        {
            if (!(entity is PlayerEntity)) return;
            Console.Clear();
            World.Instance.QuitUpdate = true;

            if (WorldToGo is CaveWorld caveWorld)
            {
                caveWorld.ExitLadders.ForEach(ladder => ladder.PlayerPosWorld = PlayerEntity.Instance.Position);
            }

            World.Instance = WorldToGo;
            PlayerEntity.Instance.Position = PlayerPosWorld;
            World.Instance.PrintTiles();
        }
    }
}
