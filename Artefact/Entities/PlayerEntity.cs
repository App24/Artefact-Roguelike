using Artefact.Utils;
using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    [Serializable]
    internal class PlayerEntity : Entity
    {
        public override string Representation => "PL";

        public static PlayerEntity Instance { get; set; }

        public override int MaxHealth => 20;

        public PlayerEntity()
        {
            Instance = this;
        }

        public override void Move()
        {
            if (Input.IsKeyHeld(ConsoleKey.RightArrow, ConsoleKey.D))
            {
                Position.X++;
            }
            else if (Input.IsKeyHeld(ConsoleKey.LeftArrow, ConsoleKey.A))
            {
                Position.X--;
            }

            if (Input.IsKeyHeld(ConsoleKey.DownArrow, ConsoleKey.S))
            {
                Position.Y++;
            }
            else if (Input.IsKeyHeld(ConsoleKey.UpArrow, ConsoleKey.W))
            {
                Position.Y--;
            }

            Position.Y = Math.Clamp(Position.Y, 0, World.Instance.Height - 1);
            Position.X = Math.Clamp(Position.X, 0, World.Instance.Width - 1);
        }

        public override void Update()
        {

        }
    }
}
