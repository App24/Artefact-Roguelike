﻿using Artefact.Entities;
using Artefact.MapSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    [Serializable]
    internal class RevealTile : Tile
    {
        private Room room;
        private List<TeleportTile> tiles = new List<TeleportTile>();

        public RevealTile(List<TeleportTile> tiles, Room room) : base(" ", false, ConsoleColor.White)
        {
            this.tiles = new List<TeleportTile>(tiles);
            this.room = room;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity == PlayerEntity.Instance)
            {
                tiles.ForEach(t => t.Revealed = true);
                Map.Instance.PrintRoom(room);
                Map.Instance.PrintEntities();
            }
        }
    }
}
