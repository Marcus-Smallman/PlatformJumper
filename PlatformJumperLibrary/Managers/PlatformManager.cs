using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformJumperLibrary.Entities;
using PlatformJumperLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlatformJumperLibrary.Managers
{
    public class PlatformManager
    {
        public Queue<Platform> Platforms { get; }

        private Texture2D platformTexture;

        private int maxPlatformCount;

        private int platformCount;

        public PlatformManager(Texture2D platformTexture)
        {
            Platforms = new Queue<Platform>();

            maxPlatformCount = 5;
            platformCount = 0;

            this.platformTexture = platformTexture;
        }

        public void Update(GameTime gameTime)
        {
            TryAddPlatform();

            foreach (var platform in Platforms.ToArray())
            {
                HandleSpeed(platform);

                platform.Update(gameTime);
            }

            if (Platforms.Peek().Position.Y > Resolution.VirtualHeight)
            {
                Platforms.Dequeue();
            }

        }

        private void HandleSpeed(Platform platform)
        {
            if (platform.Speed < 240f)
            {
                if (platformCount % 10 == 0 &&
                    platform.IncreaseSpeed == true)
                {
                    platform.Speed += 20;
                    platform.IncreaseSpeed = false;
                }
                else if (platformCount % 10 != 0 &&
                         platform.IncreaseSpeed == false)
                {
                    platform.IncreaseSpeed = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var platform in Platforms)
            {
                platform.Draw(spriteBatch);
            }
        }

        public Vector2 PeekPlatformPosition()
        {
            TryAddPlatform();

            return Platforms.Peek().Position;
        }

        public void ClearPlatforms()
        {
            Platforms.Clear();

            platformCount = 0;
        }

        private bool TryAddPlatform()
        {
            if (Platforms.Count < maxPlatformCount)
            {
                if (Platforms.Any(p => p.Position.Y < (Resolution.VirtualHeight / maxPlatformCount)) == false)
                {
                    platformCount++;

                    var speed = (float)Math.Floor((double)(platformCount / 10)) * 20;
                    if (speed > 140)
                    {
                        speed = 140f;
                    }

                    Platforms.Enqueue(
                        new Platform(
                            platformTexture,
                            new Vector2(new Random().Next(0, Resolution.VirtualWidth - platformTexture.Width), -platformTexture.Height),
                            100f + speed));

                    return true;
                }
            }

            return false;
        }
    }
}
