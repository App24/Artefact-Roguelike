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

        public abstract void Move();
    }
}
