using Microsoft.Xna.Framework;
using PlatformJumperLibrary;

namespace PlatformJumperAndroid
{
    public class Game1 : Game
    {
        private PlatformJumperGame game;

        private GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            game = new PlatformJumperGame(graphics);
            Window.Title = "Platform Jumper";
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            game.Initialize(graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width, graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            game.LoadContent(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            game.Update(this, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            game.Draw();

            base.Draw(gameTime);
        }
    }
}
