using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    internal abstract class Entity
    {
        public int Health { get; }
        public abstract int MaxHealth { get; }

        public Vector2i Position { get; set; }

        public abstract string Representation { get; }

        public Entity()
        {
            Position = new Vector2i(0, 0);
            Health = MaxHealth;
        }

        public abstract void Move();

        public abstract void Update();
    }
}
