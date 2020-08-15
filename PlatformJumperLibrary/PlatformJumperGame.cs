using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using PlatformJumperLibrary.Entities;
using PlatformJumperLibrary.Managers;
using PlatformJumperLibrary.Utilities;
using System;
using System.IO;

namespace PlatformJumperLibrary
{
    public class PlatformJumperGame
    {
        private const int GAME_WINDOW_WIDTH = 720;
        private const int GAME_WINDOW_HEIGHT = 1280;

        private SpriteBatch spriteBatch;

        public SpriteFont font;
        public SpriteFont fontSmall;

        public Texture2D blockTexture;
        private Block block;

        public Texture2D platformTexture;
        private PlatformManager platformManager;

        private string scorePath;
        private bool started;
        private int highScore;

        public PlatformJumperGame(GraphicsDeviceManager graphics)
        {
            Resolution.Init(ref graphics);
        }

        public void Initialize(int gameWindowWidth = GAME_WINDOW_WIDTH, int gameWindowHeight = GAME_WINDOW_HEIGHT)
        {
            Resolution.SetVirtualResolution(GAME_WINDOW_WIDTH, GAME_WINDOW_HEIGHT);
            Resolution.SetResolution(gameWindowWidth, gameWindowHeight, false);

            started = false;

            var dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PlatformJumper");
            scorePath = $@"{dataPath}\Score.txt";
            if (Directory.Exists(dataPath) == false)
            {
                Directory.CreateDirectory(dataPath);
                SaveScore();
            }
            else
            {
                highScore = int.Parse(File.ReadAllText(scorePath));
            }
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager contentManager)
        {
            font = contentManager.Load<SpriteFont>("font");
            fontSmall = contentManager.Load<SpriteFont>("fontsmall");
            blockTexture = contentManager.Load<Texture2D>("block");
            platformTexture = contentManager.Load<Texture2D>("platform");

            spriteBatch = new SpriteBatch(graphicsDevice);
            platformManager = new PlatformManager(platformTexture);

            block = CreateBlock();
        }

        public void Update(Game game, GameTime gameTime)
        {
            HandleInput(game);

            if (started == true)
            {
                platformManager.Update(gameTime);
                block.Update(gameTime);

                if (block.HasDied == true)
                {
                    Reset();
                }
            }
        }

        private void HandleInput(Game game)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                game.Exit();
            }
            else if (keyboardState.IsKeyDown(Keys.Back) &&
                     started == true)
            {
                Reset();

                started = false;
            }

            if (started == false)
            {
                var mouseState = Mouse.GetState();
                var touchCollection = TouchPanel.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    started = true;
                }
                else if (touchCollection.Count > 0)
                {
                    if (touchCollection[0].State == TouchLocationState.Pressed)
                    {
                        started = true;
                    }
                }
            }
        }

        public void Draw()
        {
            Resolution.BeginDraw();

            spriteBatch.Begin(transformMatrix: Resolution.getTransformationMatrix());

            if (started == false)
            {
                DrawStart();
            }
            else
            {
                DrawGame();
            }

            spriteBatch.End();
        }

        private void DrawStart()
        {
            var startText = "TAP TO START";
            var startTextSize = font.MeasureString(startText);
            spriteBatch.DrawString(font, startText, new Vector2((Resolution.VirtualWidth / 2) - (startTextSize.X / 2), (Resolution.VirtualHeight / 2) - (startTextSize.Y / 2)), Color.White);

            var scoreText = highScore.ToString();
            var scoreTextSize = font.MeasureString(scoreText);
            spriteBatch.DrawString(font, scoreText, new Vector2((Resolution.VirtualWidth / 2) - (scoreTextSize.X / 2), Resolution.VirtualHeight - scoreTextSize.Y - 25), Color.White);

            var highScoreText = "HIGH SCORE";
            var highScoreTextSize = fontSmall.MeasureString(highScoreText);
            spriteBatch.DrawString(fontSmall, highScoreText, new Vector2((Resolution.VirtualWidth / 2) - (highScoreTextSize.X / 2), Resolution.VirtualHeight - highScoreTextSize.Y - scoreTextSize.Y - 25), Color.White);
        }

        private void DrawGame()
        {
            platformManager.Draw(spriteBatch);
            block.Draw(spriteBatch);

            var scoreText = block.PlatformCount.ToString();
            var scoreTextSize = font.MeasureString(scoreText);
            spriteBatch.DrawString(font, scoreText, new Vector2((Resolution.VirtualWidth / 2) - (scoreTextSize.X / 2), Resolution.VirtualHeight - scoreTextSize.Y - 25), Color.White);
        }

        private void Reset()
        {
            if (block.PlatformCount > highScore)
            {
                highScore = block.PlatformCount;
                SaveScore();
            }

            platformManager.ClearPlatforms();
            block = CreateBlock();
        }

        private Block CreateBlock()
        {
            return new Block(
                blockTexture,
                platformManager.PeekPlatformPosition() + new Vector2((platformTexture.Width / 2) - (blockTexture.Width / 2), -blockTexture.Height),
                platformManager);
        }

        private void SaveScore()
        {
            File.WriteAllText(scorePath, highScore.ToString());
        }
    }
}
