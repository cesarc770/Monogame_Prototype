using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Rapid_Prototype_1.Tools;
using System.Globalization;

namespace Rapid_Prototype_1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int WINDOW_WIDTH = 1920;
        const int WINDOW_HEIGHT = 1080;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D background_Sprite;

        MouseState mouseState;

        //shapes temporary
        GameController gameController = new GameController();

        //**************//

        const float TEMP_BPM = 130;

        Rhythm rhythm = new Rhythm(TEMP_BPM);
        Song song;
        private bool started = false;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

           // if we wanted to set a window size
           graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
           graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;

            IsMouseVisible = true;
            Window.AllowUserResizing = false;          
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            song = Content.Load<Song>("tempLoop");
            background_Sprite = Content.Load<Texture2D>("background");
         
            foreach (Shape shape in gameController.allFallingShapes)
            {
                shape.setSprite(Content.Load<Texture2D>(shape.getSpriteName()));
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private bool showBackground = true;
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!started) {
                started = true;
                rhythm.Start();
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = true;
            }
            rhythm.Update(gameTime, () => showBackground = false);
            base.Update(gameTime);

            mouseState = Mouse.GetState();
            gameController.ControllerUpate(gameTime);

            foreach (Shape shape in gameController.fallingShapes)
            {
                shape.ShapeUpdate(gameTime);
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (showBackground) {
                spriteBatch.Draw(background_Sprite, GraphicsDevice.Viewport.Bounds, Color.White);
            }
            else {
                spriteBatch.Draw(background_Sprite, GraphicsDevice.Viewport.Bounds, Color.White * 0.95f);
                showBackground = true;
            }

            foreach (Shape shape in gameController.fallingShapes)
            {
                Vector2 shapePos = shape.GetPosition();
                int centerDist = shape.GetCenter();
                spriteBatch.Draw(shape.getSprite(), new Vector2(shapePos.X - centerDist, shapePos.Y - centerDist) , Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
