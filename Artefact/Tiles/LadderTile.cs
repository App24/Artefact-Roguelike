﻿using Artefact.Entities;
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

        public bool GoingDown { get; set; }

        public LadderTile() : base("#", ConsoleColor.Gray, ConsoleColor.DarkYellow, true)
        {
        }

        public override void OnCollide(Entity entity)
        {
            if (!(entity is PlayerEntity)) return;
            Console.Clear();

            if (WorldToGo is CaveWorld caveWorld)
            {
                if (GoingDown)
                {
                    caveWorld.ExitLadders.ForEach(ladder => ladder.PlayerPosWorld = new Vector2i(PlayerEntity.Instance.Position));
                }
                else
                {
                    caveWorld.NextLadders.ForEach(ladder => ladder.PlayerPosWorld = new Vector2i(PlayerEntity.Instance.Position));
                }
            }

            World.Instance.QuitUpdate = true;
            World.Instance = WorldToGo;
            PlayerEntity.Instance.Position = PlayerPosWorld;
            World.Instance.PrintTiles();
        }
    }
}
