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

        public LadderTile() : base("#", ConsoleColor.Gray, ConsoleColor.DarkYellow, false)
        {
        }

        public override void OnCollide(Entity entity)
        {
            Console.Clear();
            World.Instance.QuitUpdate = true;
            World.Instance = WorldToGo;
            PlayerEntity.Instance.Position = PlayerPosWorld;
            World.Instance.PrintTiles();
        }
    }
}
