using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    internal static class Extensions
    {
        public static Vector2 NextVector2i(this Random random, Vector2 min, Vector2 max)
        {
            return new Vector2(random.Next(min.X, max.X), random.Next(min.Y, max.Y));
        }
        public static Vector2 NextVector2i(this Random random, Vector2 max)
        {
            return random.NextVector2i(Vector2.Zero, max);
        }

        public static bool NextBool(this Random random)
        {
            return random.Next(2) == 1;
        }
    }
}
