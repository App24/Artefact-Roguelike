using Artefact.Audio;
using Artefact.Audio.SoundEffectsSystem;
using Artefact.Entities;
using Artefact.Items;
using Artefact.MapSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Tiles
{
    [Serializable]
    internal class ChestTile : Tile
    {
        public override ConsoleColor Foreground => items.Count <= 0 ? ConsoleColor.White : base.Foreground;

        private List<Item> items = new List<Item>();
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
                if (items.Count > 0)
                {
                    List<Item> remainingItems = new List<Item>();

                    foreach (Item item in items)
                    {
                        if (!PlayerEntity.Instance.Inventory.AddItem(item, item.Quantity))
                        {
                            remainingItems.Add(item);
                        }
                    }

                    items.Clear();
                    items.AddRange(remainingItems);

                    if (items.Count <= 0 && previousTile != null)
                    {
                        entity.CurrentRoom.SetTile(position, previousTile);
                    }

                    SFXSystem.AddSoundEffect(new SoundEffect(SoundEffectType.Tile, new Note(Tone.B, Duration.SIXTEENTH), new Note(Tone.B, Duration.SIXTEENTH), new Note(Tone.Gsharp, Duration.EIGHTH)));
                }
            }
        }

        public void AddItem(Item item, int quantity = 1)
        {
            Item toAdd = item.Clone();
            toAdd.Quantity = quantity;
            items.Add(toAdd);
        }
    }
}
