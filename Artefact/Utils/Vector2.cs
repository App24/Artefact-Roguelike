using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    [Serializable]
    internal struct Vector2
    {
        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 Up => new Vector2(0, -1);
        public static Vector2 Down => new Vector2(0, 1);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Right => new Vector2(1, 0);

        public int x;
        public int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2(int value) : this(value, value)
        {

        }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2 vec)
            {
                return vec.x == x && vec.y == y;
            }
            return false;
        }

        public int DistanceTo(Vector2 position)
        {
            Vector2 vec = this - position;
            float value = vec.x * vec.x;
            value += vec.y * vec.y;
            return (int)MathF.Sqrt(value);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.x + b.x, a.y + b.y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.x - b.x, a.y - b.y);
        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.x * b.x, a.y * b.y);
        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);
    }
}
