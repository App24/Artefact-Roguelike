using Artefact.MapSystem;
using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    internal abstract class Entity
    {
        public Vector2 position;
        public Vector2 RelativePosition
        {
            get
            {
                if (CurrentRoom != null)
                {
                    return position - CurrentRoom.Position;
                }
                return position;
            }
        }

        public abstract string Representation { get; }

        public abstract int MaxHealth { get; }
        public int Health { get; private set; }

        public Room CurrentRoom
        {
            get
            {
                if (Map.Instance != null)
                {
                    return Map.Instance.GetRoom(position);
                }
                return null;
            }
        }

        public Entity()
        {
            Health = MaxHealth;
        }

        public abstract void Move();

        public virtual void Damage(int amount)
        {
            if(amount < 0)
            {
                Heal(-amount);
                return;
            }

            Health -= amount;

            if(Health < 0)
                Die();
        }

        public virtual void Heal(int amount)
        {
            if(amount < 0)
            {
                Damage(-amount);
                return;
            }

            Health += amount;
            if(Health > MaxHealth)
                Health = MaxHealth;
        }

        protected abstract void Die();

        public abstract void Update();
    }
}
