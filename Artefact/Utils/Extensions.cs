using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    internal static class Extensions
    {
        public static Vector2 NextVector2(this Random random, Vector2 min, Vector2 max)
        {
            return new Vector2(random.Next(min.x, max.x), random.Next(min.y, max.y));
        }
        public static Vector2 NextVector2(this Random random, Vector2 max)
        {
            return random.NextVector2(new Vector2(0), max);
        }

        public static bool NextBool(this Random random)
        {
            return random.Next(2) != 0;
        }
    }
}
