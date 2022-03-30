using Artefact.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    internal class PlayerEntity : Entity
    {
        public static PlayerEntity Instance { get; set; }

        public override string Representation => "PL";

        public PlayerEntity()
        {
            Instance = this;
        }

        public override void Move()
        {
            if (InputSystem.IsKeyHeld(ConsoleKey.D))
            {
                position.x++;
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.A))
            {
                position.x--;
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.W))
            {
                position.y--;
            }

            if (InputSystem.IsKeyHeld(ConsoleKey.S))
            {
                position.y++;
            }
        }
    }
}
