using Artefact.Entities;
using Artefact.Items;
using Artefact.MapSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal class ChestTile : Tile
    {
        public override ConsoleColor Foreground => looted ? ConsoleColor.White : base.Foreground;
        bool looted = false;

        public ChestTile() : base("[]", false, ConsoleColor.DarkYellow)
        {
        }

        public override void OnCollision(Entity entity)
        {
            if (looted)
                return;
            if (entity == PlayerEntity.Instance)
            {
                if(PlayerEntity.Instance.Inventory.AddItem(new HealthPotionItem((Rarity)new Random().Next(0, ((int)Rarity.Epic)+1)), 3))
                {
                    looted = true;
                }
            }
        }
    }
}
