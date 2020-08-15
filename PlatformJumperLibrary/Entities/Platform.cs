using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformJumperLibrary.Entities
{
    public class Platform
    {
        public Texture2D Texture { get; }

        public float Speed { get; set; }

        public bool Landed { get; set; }

        public bool IncreaseSpeed { get; set; }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        private Vector2 position;

        public Vector2 Size
        {
            get
            {
                return size;
            }
        }

        private Vector2 size;

        public Platform(Texture2D texture, Vector2 position, float speed)
        {
            Texture = texture;
            Speed = speed;
            Landed = false;
            IncreaseSpeed = false;

            this.position = position;
            size = new Vector2(texture.Width, texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, Color.White);
        }
    }
}
