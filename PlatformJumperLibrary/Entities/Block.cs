using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PlatformJumperLibrary.Managers;
using PlatformJumperLibrary.Utilities;
using System.Linq;

namespace PlatformJumperLibrary.Entities
{
    public class Block
    {
        public bool HasDied { get; private set; }

        public int PlatformCount { get; private set; }

        private Texture2D texture;

        private Vector2 position;
        private Vector2 size;
        private Vector2 velocity;

        private readonly PlatformManager platformManager;

        public Block(Texture2D texture, Vector2 position, PlatformManager platformManager)
        {
            HasDied = false;
            PlatformCount = 0;

            this.texture = texture;
            this.position = position;
            size = new Vector2(texture.Width, texture.Height);
            this.platformManager = platformManager;
        }

        public void Update(GameTime gameTime)
        {
            var previousPosition = position;

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            var keyboardState = Keyboard.GetState();
            var touchCollection = TouchPanel.GetState();
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                velocity.X = 500f;
            }
            else if (keyboardState.IsKeyDown(Keys.Left))
            {
                velocity.X = -500f;
            }
            else if (touchCollection.Count > 0)
            {
                if (touchCollection.Any(touchLocation => touchLocation.State == TouchLocationState.Pressed &&
                                        touchLocation.Position.X > (Resolution.Width / 2) &&
                                        touchLocation.Position.Y > (Resolution.Height - (Resolution.Height / 5))))
                {
                    velocity.X = 500f;
                }
                else  if (touchCollection.Any(touchLocation => touchLocation.State == TouchLocationState.Pressed &&
                                              touchLocation.Position.X < (Resolution.Width / 2) &&
                                              touchLocation.Position.Y > (Resolution.Height - (Resolution.Height / 5))))
                {
                    velocity.X = -500f;
                }
            }
            else
            {
                velocity.X = 0f;
            }

            if (position.X > Resolution.VirtualWidth - texture.Width)
            {
                position.X = Resolution.VirtualWidth - texture.Width;
            }
            else if (position.X < 0)
            {
                position.X = 0;
            }

            if (position.Y > Resolution.VirtualHeight - texture.Height)
            {
                HasDied = true;
            }
            else if (position.Y < 0)
            {
                position.Y = 0;
                velocity.Y = 0f;
            }

            ResolveCollisions(previousPosition);

            if (IsOnPlatform(out Platform platform) == true)
            {
                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    // Jump!
                    velocity.Y = -1250f;
                }
                else if (touchCollection.Count > 0)
                {
                    if (touchCollection.Any(touchLocation => touchLocation.State == TouchLocationState.Pressed &&
                                            touchLocation.Position.Y < (Resolution.Height - (Resolution.Height / 5))))
                    {
                        velocity.Y = -1250f;
                    }
                }
                else
                {
                    velocity.Y = platform.Speed;
                }
            }
            else
            {
                var i = 150f;
                velocity.Y += 0.25f * i;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        private bool IsCollided(out Platform collidedPlatform)
        {
            collidedPlatform = null;
            foreach (var platform in platformManager.Platforms.ToArray())
            {
                if (RectangleUtilities.GetHitbox(platform.Position, platform.Size).Intersects(RectangleUtilities.GetHitbox(position, size)) == true)
                {
                    collidedPlatform = platform;

                    return true;
                }
            }

            return false;
        }

        private void ResolveCollisions(Vector2 previousPosition)
        {
            if (IsCollided(out Platform collidedPlatform) == true)
            {
                var previousHitbox = RectangleUtilities.GetHitbox(previousPosition, size);
                var currentHitbox = RectangleUtilities.GetHitbox(position, size);
                var platformHitbox = RectangleUtilities.GetHitbox(collidedPlatform.Position, collidedPlatform.Size);

                if (RectangleUtilities.IsTopLeft(previousHitbox, platformHitbox) == true)
                {
                    if (MathUtilities.Slope(RectangleUtilities.BottomRightPoint(previousHitbox), RectangleUtilities.BottomRightPoint(currentHitbox)) <
                        MathUtilities.Slope(RectangleUtilities.BottomRightPoint(previousHitbox), RectangleUtilities.TopLeftPoint(platformHitbox)))
                    {
                        position.Y = collidedPlatform.Position.Y - texture.Height;
                    }
                    else
                    {
                        position.X = collidedPlatform.Position.X - texture.Width;
                    }
                }
                else if (RectangleUtilities.IsTop(previousHitbox, platformHitbox) == true)
                {
                    position.Y = collidedPlatform.Position.Y - texture.Height;
                }
                else if (RectangleUtilities.IsTopRight(previousHitbox, platformHitbox) == true)
                {
                    if (MathUtilities.Slope(RectangleUtilities.BottomLeftPoint(previousHitbox), RectangleUtilities.BottomLeftPoint(currentHitbox)) >
                        MathUtilities.Slope(RectangleUtilities.BottomLeftPoint(previousHitbox), RectangleUtilities.TopRightPoint(platformHitbox)))
                    {
                        position.Y = collidedPlatform.Position.Y - texture.Height;
                    }
                    else
                    {
                        position.X = collidedPlatform.Position.X + collidedPlatform.Texture.Width;
                    }
                }
                else if (RectangleUtilities.IsRight(previousHitbox, platformHitbox) == true)
                {
                    position.X = collidedPlatform.Position.X + collidedPlatform.Texture.Width;
                }
                else if (RectangleUtilities.IsBottomRight(previousHitbox, platformHitbox) == true)
                {
                    if (MathUtilities.Slope(RectangleUtilities.TopLeftPoint(previousHitbox), RectangleUtilities.TopLeftPoint(currentHitbox)) <
                        MathUtilities.Slope(RectangleUtilities.TopLeftPoint(previousHitbox), RectangleUtilities.BottomRightPoint(platformHitbox)))
                    {
                        position.Y = collidedPlatform.Position.Y + collidedPlatform.Texture.Height;
                        velocity.Y = collidedPlatform.Speed;
                    }
                    else
                    {
                        position.X = collidedPlatform.Position.X + collidedPlatform.Texture.Width;
                    }
                }
                else if (RectangleUtilities.IsBottom(previousHitbox, platformHitbox) == true)
                {
                    position.Y = collidedPlatform.Position.Y + collidedPlatform.Texture.Height;
                    velocity.Y = collidedPlatform.Speed;
                }
                else if (RectangleUtilities.IsBottomLeft(previousHitbox, platformHitbox) == true)
                {
                    if (MathUtilities.Slope(RectangleUtilities.TopRightPoint(previousHitbox), RectangleUtilities.TopRightPoint(currentHitbox)) >
                        MathUtilities.Slope(RectangleUtilities.TopRightPoint(previousHitbox), RectangleUtilities.BottomLeftPoint(platformHitbox)))
                    {
                        position.Y = collidedPlatform.Position.Y + collidedPlatform.Texture.Height;
                        velocity.Y = collidedPlatform.Speed;
                    }
                    else
                    {
                        position.X = collidedPlatform.Position.X - texture.Width;
                    }
                }
                else if (RectangleUtilities.IsLeft(previousHitbox, platformHitbox) == true)
                {
                    position.X = collidedPlatform.Position.X - texture.Width;
                }
                else
                {
                    // This code should only ever get hit when jumping from below as there is a race condition where by
                    // if the block was moving up and a platform was moving down, and they collided. Then it compares the previous
                    // position of the block based on the current position of the platform and as the platform has moved down
                    // there can be an issue where it can't fiure out which face to resolve the position of the block as the previous
                    // position of the block is collided with the current position of the platform.

                    // As this only code should only get hit when the above is true we can set the Y position of the block to
                    // equal the bottom of the platform.
                    position.Y = collidedPlatform.Position.Y + collidedPlatform.Texture.Height;
                    velocity.Y = collidedPlatform.Speed;

                }
            }
        }

        private bool IsOnPlatform(out Platform p)
        {
            p = null;
            if (position.Y + texture.Height < Resolution.VirtualHeight)
            {
                foreach (var platform in platformManager.Platforms.ToArray())
                {
                    if (RectangleUtilities.IsOnTop(
                            RectangleUtilities.GetHitbox(position, size),
                            RectangleUtilities.GetHitbox(platform.Position, platform.Size)) == true)
                    {
                        p = platform;
                        if (p.Landed == false)
                        {
                            p.Landed = true;

                            PlatformCount++;
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
