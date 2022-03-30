using Artefact.Entities;
using Artefact.MapSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal class TeleportTile : Tile
    {
        private Room room;
        private Vector2 exit;

        public TeleportTile(Room room, Vector2 exit) : base("XX", false, ConsoleColor.DarkYellow)
        {
            this.room = room;
            this.exit = exit;
        }

        public override void OnCollision(Entity entity)
        {
            entity.position = room.Position + room.GetAvailablePosition();
            entity.CurrentRoom.Known = true;
            entity.position = room.Position + exit;
        }
    }
}
