using Artefact.Entities;
using Artefact.Items;
using Artefact.MapSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    internal class ChestTile : Tile
    {
        public override ConsoleColor Foreground => items.Count <= 0 ? ConsoleColor.White : base.Foreground;

        List<Item> items = new List<Item>();
        private readonly Vector2 position;
        private readonly Tile previousTile;

        public ChestTile() : base("[]", false, ConsoleColor.DarkYellow)
        {
            this.items = new List<Item>();
        }

        public ChestTile(Tile previousTile, Vector2 position) : this()
        {
            this.position = position;
            this.previousTile = previousTile;
        }

        public override void OnCollision(Entity entity)
        {
            if (entity == PlayerEntity.Instance)
            {
                List<Item> remainingItems = new List<Item>();

                foreach(Item item in items)
                {
                    if (!PlayerEntity.Instance.Inventory.AddItem(item, item.Quantity))
                    {
                        remainingItems.Add(item);
                    }
                }

                items.Clear();
                items.AddRange(remainingItems);

                if(items.Count <= 0 && previousTile != null)
                {
                    entity.CurrentRoom.SetTile(position, previousTile);
                }
            }
        }

        public void AddItem(Item item, int quantity=1)
        {
            Item toAdd = item.Clone();
            toAdd.Quantity = quantity;
            items.Add(toAdd);
        }
    }
}
