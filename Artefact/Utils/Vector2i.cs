using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact.Utils
{
    internal class Vector2i
    {
        public static Vector2i Zero { get { return new Vector2i(0); } }
        public static Vector2i Up { get { return new Vector2i(0, -1); } }
        public static Vector2i Down { get { return new Vector2i(0, 1); } }
        public static Vector2i Left { get { return new Vector2i(-1, 0); } }
        public static Vector2i Right { get { return new Vector2i(1, 0); } }

        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2i(int value):this(value, value)
        {
        }

        public Vector2i(Vector2i copy) : this(copy.X, copy.Y)
        {

        }

        public int X { get; set; }
        public int Y { get; set; }

        public static bool operator ==(Vector2i a, Vector2i b)
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

        internal int DistanceTo(Vector2i position)
        {
            Vector2i vec = this - position;
            float value = vec.X * vec.X;
            value += vec.Y * vec.Y;
            return (int)MathF.Sqrt(value);
        }

        public static bool operator !=(Vector2i a, Vector2i b)
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
            if (obj is Vector2i vec)
            {
                return vec.X == X && vec.Y == Y;
            }
            return false;
        }

        public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.X + b.X, a.Y + b.Y);
        public static Vector2i operator -(Vector2i a, Vector2i b) => new Vector2i(a.X - b.X, a.Y - b.Y);
        public static Vector2i operator *(Vector2i a, Vector2i b) => new Vector2i(a.X * b.X, a.Y * b.Y);
        public static Vector2i operator /(Vector2i a, Vector2i b) => new Vector2i(a.X / b.X, a.Y / b.Y);
    }
}
