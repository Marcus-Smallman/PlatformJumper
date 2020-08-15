using Microsoft.Xna.Framework;
using System;

namespace PlatformJumperLibrary.Utilities
{
    public static class RectangleUtilities
    {
        public static Rectangle GetHitbox(Vector2 position, Vector2 size)
        {
            return new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), (int)Math.Round(size.X), (int)Math.Round(size.Y));
        }

        public static Point TopLeftPoint(Rectangle rectangle)
        {
            return rectangle.Location;
        }

        public static Point TopRightPoint(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width, rectangle.Y);
        }

        public static Point BottomRightPoint(Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);
        }

        public static Point BottomLeftPoint(Rectangle rectangle)
        {
            return new Point(rectangle.X, rectangle.Y + rectangle.Height);
        }

        public static bool IsTopLeft(Rectangle rectangle1, Rectangle rectangle2)
        {
            return BottomRightPoint(rectangle1).X <= TopLeftPoint(rectangle2).X &&
                   BottomRightPoint(rectangle1).Y <= TopLeftPoint(rectangle2).Y;
        }

        public static bool IsTop(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (BottomLeftPoint(rectangle1).X >= TopLeftPoint(rectangle2).X &&
                    BottomLeftPoint(rectangle1).X <= TopRightPoint(rectangle2).X &&
                    BottomLeftPoint(rectangle1).Y <= TopLeftPoint(rectangle2).Y) ||
                   (BottomRightPoint(rectangle1).X >= TopLeftPoint(rectangle2).X &&
                    BottomRightPoint(rectangle1).X <= TopRightPoint(rectangle2).X &&
                    BottomRightPoint(rectangle1).Y <= TopLeftPoint(rectangle2).Y);
        }

        public static bool IsOnTop(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (BottomLeftPoint(rectangle1).X >= TopLeftPoint(rectangle2).X &&
                    BottomLeftPoint(rectangle1).X <= TopRightPoint(rectangle2).X &&
                    BottomLeftPoint(rectangle1).Y == TopLeftPoint(rectangle2).Y) ||
                   (BottomRightPoint(rectangle1).X >= TopLeftPoint(rectangle2).X &&
                    BottomRightPoint(rectangle1).X <= TopRightPoint(rectangle2).X &&
                    BottomRightPoint(rectangle1).Y == TopLeftPoint(rectangle2).Y);
        }

        public static bool IsTopRight(Rectangle rectangle1, Rectangle rectangle2)
        {
            return BottomLeftPoint(rectangle1).X >= TopRightPoint(rectangle2).X &&
                   BottomLeftPoint(rectangle1).Y <= TopRightPoint(rectangle2).Y;
        }

        public static bool IsRight(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (TopLeftPoint(rectangle1).Y >= TopRightPoint(rectangle2).Y &&
                    TopLeftPoint(rectangle1).Y <= BottomRightPoint(rectangle2).Y &&
                    TopLeftPoint(rectangle1).X >= TopRightPoint(rectangle2).X) ||
                   (BottomLeftPoint(rectangle1).Y >= TopRightPoint(rectangle2).Y &&
                    BottomLeftPoint(rectangle1).Y <= BottomRightPoint(rectangle2).Y &&
                    BottomLeftPoint(rectangle1).X >= TopRightPoint(rectangle2).X);
        }

        public static bool IsBottomRight(Rectangle rectangle1, Rectangle rectangle2)
        {
            return TopLeftPoint(rectangle1).X >= BottomRightPoint(rectangle2).X &&
                   TopLeftPoint(rectangle1).Y >= BottomRightPoint(rectangle2).Y;
        }

        public static bool IsBottom(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (TopLeftPoint(rectangle1).X >= BottomLeftPoint(rectangle2).X &&
                    TopLeftPoint(rectangle1).X <= BottomRightPoint(rectangle2).X &&
                    TopLeftPoint(rectangle1).Y >= BottomLeftPoint(rectangle2).Y) ||
                   (TopRightPoint(rectangle1).X >= BottomLeftPoint(rectangle2).X &&
                    TopRightPoint(rectangle1).X <= BottomRightPoint(rectangle2).X &&
                    TopRightPoint(rectangle1).Y >= BottomLeftPoint(rectangle2).Y);
        }

        public static bool IsBottomLeft(Rectangle rectangle1, Rectangle rectangle2)
        {
            return TopRightPoint(rectangle1).X <= BottomLeftPoint(rectangle2).X &&
                   TopRightPoint(rectangle1).Y >= BottomLeftPoint(rectangle2).Y;
        }

        public static bool IsLeft(Rectangle rectangle1, Rectangle rectangle2)
        {
            return (TopRightPoint(rectangle1).Y >= TopLeftPoint(rectangle2).Y &&
                    TopRightPoint(rectangle1).Y <= BottomLeftPoint(rectangle2).Y &&
                    TopRightPoint(rectangle1).X <= TopLeftPoint(rectangle2).X) ||
                   (BottomRightPoint(rectangle1).Y >= TopLeftPoint(rectangle2).Y &&
                    BottomRightPoint(rectangle1).Y <= BottomLeftPoint(rectangle2).Y &&
                    BottomRightPoint(rectangle1).X <= TopLeftPoint(rectangle2).X);
        }
    }
}
