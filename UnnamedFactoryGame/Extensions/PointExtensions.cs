using Microsoft.Xna.Framework;
using System;

namespace Cosmi.Extensions;

internal static class PointExtensions
{
    extension(Point p)
    {
        public static Point operator &(Point a, Point b) => new(a.X & b.X, a.Y & b.Y);
        public static Point operator &(Point a, int b) => new(a.X & b, a.Y & b);
        public static Point operator |(Point a, Point b) => new(a.X | b.X, a.Y | b.Y);
        public static Point operator |(Point a, int b) => new(a.X | b, a.Y | b);
        public static Point operator <<(Point a, int b) => new(a.X << b, a.Y << b);
        public static Point operator >>(Point a, int b) => new(a.X >> b, a.Y >> b);
        public static Point operator ~(Point a) => new(~a.X, ~a.Y);
        public static Point operator /(Point a, int b) => new(a.X / b, a.Y / b);
        public static Point operator /(Point a, Point b) => new(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator *(Point a, Vector2 b) => new(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator /(Point a, Vector2 b) => new(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator *(Vector2 a, Point b) => new(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator /(Vector2 a, Point b) => new(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator *(Point a, float b) => new(a.X * b, a.Y * b);
        public static Vector2 operator /(Point a, float b) => new(a.X / b, a.Y / b);
        public static Vector2 operator *(Point a, int b) => new(a.X * b, a.Y * b);
    }
}
