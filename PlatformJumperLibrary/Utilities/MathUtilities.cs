using Microsoft.Xna.Framework;
using System;

namespace PlatformJumperLibrary.Utilities
{
    public static class MathUtilities
    {
        public static float Slope(Point p1, Point p2)
        {
            float run = Math.Abs(p1.X - Math.Abs(p2.X));
            float rise = Math.Abs(p1.Y - Math.Abs(p2.Y));

            float slope = run / rise;

            return slope;
        }
    }
}
