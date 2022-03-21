using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    [Serializable]
    internal abstract class Entity
    {
        public int Health { get; }
        public abstract int MaxHealth { get; }

        public Vector2 Position { get; set; }
        public Vector2 OriginalPosition { get; set; }

        public abstract string Representation { get; }

        public Entity()
        {
            Position = Vector2.Zero;
            Health = MaxHealth;
        }

        public abstract void Move();

        public abstract void Update();

        public abstract void CollidePlayer(PlayerEntity playerEntity);
    }
}
