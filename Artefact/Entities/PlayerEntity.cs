using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Entities
{
    class PlayerEntity : Entity
    {
        public override string Representation => "PL";

        public override void Move()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            Vector2i previousPos = new Vector2i(Position.X, Position.Y);

            switch (keyInfo.Key)
            {
                case ConsoleKey.D:
                    {
                        Position.X++;
                    }
                    break;
                case ConsoleKey.A:
                    {
                        Position.X--;
                    }
                    break;
                case ConsoleKey.S:
                    {
                        Position.Y++;
                    }
                    break;
                case ConsoleKey.W:
                    {
                        Position.Y--;
                    }
                    break;
            }

            Position.Y = Math.Clamp(Position.Y, 0, World.Instance.Height);
            Position.X = Math.Clamp(Position.X, 0, World.Instance.Width);

            if(World.Instance.GetTile(Position.X, Position.Y).Collidable)
            {
                Position = previousPos;
            }
        }
    }
}
