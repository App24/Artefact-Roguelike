using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    [Serializable]
    internal class Vector2
    {
        public static Vector2 Zero { get { return new Vector2(0); } }
        public static Vector2 Up { get { return new Vector2(0, -1); } }
        public static Vector2 Down { get { return new Vector2(0, 1); } }
        public static Vector2 Left { get { return new Vector2(-1, 0); } }
        public static Vector2 Right { get { return new Vector2(1, 0); } }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2(int value) : this(value, value)
        {
        }

        public Vector2(Vector2 copy) : this(copy.X, copy.Y)
        {

        }

        public int X { get; set; }
        public int Y { get; set; }

        public static bool operator ==(Vector2 a, Vector2 b)
        {
            if (a is null && b is null)
            {
                return true;
            }
            else if (a is null || b is null)
            {
                return false;
            }
            return a.Equals(b);
        }

        internal int DistanceTo(Vector2 position)
        {
            Vector2 vec = this - position;
            float value = vec.X * vec.X;
            value += vec.Y * vec.Y;
            return (int)MathF.Sqrt(value);
        }

        public static bool operator !=(Vector2 a, Vector2 b)
        {
            if (a is null && b is null)
            {
                return false;
            }
            else if (a is null || b is null)
            {
                return true;
            }
            return !a.Equals(b);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2 vec)
            {
                return vec.X == X && vec.Y == Y;
            }
            return false;
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.X / b.X, a.Y / b.Y);
    }
}
