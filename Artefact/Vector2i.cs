﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Artefact
{
    internal class Vector2i
    {
        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
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
            return a.X == b.X && a.Y == b.Y;
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
            return a.X != b.X || a.Y != b.Y;
        }

        public static Vector2i operator +(Vector2i a, Vector2i b) => new Vector2i(a.X + b.X, a.Y + b.Y);
        public static Vector2i operator -(Vector2i a, Vector2i b) => new Vector2i(a.X - b.X, a.Y - b.Y);
        public static Vector2i operator *(Vector2i a, Vector2i b) => new Vector2i(a.X * b.X, a.Y * b.Y);
        public static Vector2i operator /(Vector2i a, Vector2i b) => new Vector2i(a.X / b.X, a.Y / b.Y);
    }
}
