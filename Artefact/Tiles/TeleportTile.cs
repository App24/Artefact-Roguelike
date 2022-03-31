﻿using Artefact.Entities;
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

        public override string Representation => Revealed ? base.Representation : replaceRepresentation;
        public override ConsoleColor Foreground => Revealed ? base.Foreground : replaceForeground;

        public bool Revealed { get; set; }

        string replaceRepresentation;
        ConsoleColor replaceForeground;

        public TeleportTile(Room room, Vector2 exit, Tile replaceTile) : base("XX", true, ConsoleColor.DarkMagenta)
        {
            this.room = room;
            this.exit = exit;
            replaceForeground = replaceTile.Foreground;
            replaceRepresentation = replaceTile.Representation;
        }

        public override void OnCollision(Entity entity)
        {
            entity.position = room.Position + room.GetAvailablePosition();
            entity.CurrentRoom.Known = true;
            entity.position = room.Position + exit;
            entity.CurrentRoom.GetTile(entity.RelativePosition).OnCollision(entity);
        }
    }
}