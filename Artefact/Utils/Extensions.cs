using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    internal static class Extensions
    {
        public static Vector2i NextVector2i(this Random random, Vector2i min, Vector2i max)
        {
            return new Vector2i(random.Next(min.X, max.X), random.Next(min.Y, max.Y));
        }
        public static Vector2i NextVector2i(this Random random, Vector2i max)
        {
            return random.NextVector2i(Vector2i.Zero, max);
        }
    }
}
