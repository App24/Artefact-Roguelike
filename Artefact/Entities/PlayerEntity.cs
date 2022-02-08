using Artefact.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    class PlayerEntity : Entity
    {
        public override string Representation => "PL";

        public static PlayerEntity Instance { get; private set; }

        public PlayerEntity()
        {
            Instance = this;
        }

        public override void Move()
        {
            switch (Program.KeyInfo.Key)
            {
                case ConsoleKey.RightArrow:
                case ConsoleKey.D:
                    {
                        Position.X++;
                    }
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.A:
                    {
                        Position.X--;
                    }
                    break;

                case ConsoleKey.DownArrow:
                case ConsoleKey.S:
                    {
                        Position.Y++;
                    }
                    break;

                case ConsoleKey.UpArrow:
                case ConsoleKey.W:
                    {
                        Position.Y--;
                    }
                    break;
            }

            Position.Y = Math.Clamp(Position.Y, 0, World.Instance.Height - 1);
            Position.X = Math.Clamp(Position.X, 0, World.Instance.Width - 1);
        }

        public override void Update()
        {

        }
    }
}
