using Artefact.Entities;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal class CaveTile : Tile
    {
        public CaveWorld CaveWorld { get; set; }

        public CaveTile() : base("#", ConsoleColor.DarkMagenta, true)
        {
        }

        public override void OnCollide(Entity entity)
        {
            if (!(entity is PlayerEntity)) return;
            Console.Clear();
            CaveWorld.ExitLadders.ForEach(t =>
            {
                t.PlayerPosWorld = new Vector2i(PlayerEntity.Instance.Position);
            });
            World.Instance.QuitUpdate = true;
            World.Instance = CaveWorld;
            World.Instance.PlacePlayer();
            World.Instance.PrintTiles();
        }
    }
}
