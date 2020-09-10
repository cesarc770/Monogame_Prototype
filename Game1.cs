using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Rapid_Prototype_1.Tools;
using System.Globalization;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Rapid_Prototype_1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int WINDOW_WIDTH = 1920;
        const int WINDOW_HEIGHT = 1080;
        const float THRESHOLD_FOR_PLACING_PIECES = 100.0f;
        GraphicsDeviceManager graphics;
        private PresentationParameters pp;
        SpriteBatch spriteBatch;
        Song song;

        private RenderTarget2D renderTarget1, renderTarget2;

        Flickering flickering;
        float bloomSatPulse = 1f, bloomSatDir = .04f;

        Texture2D background_Sprite;
        SpriteFont spriteFont;

        MouseState mouseState;
        MouseState lastMouseState;

        Button startButton;

        //shapes temporary
        FallingShapes fallingShapes;

        //**************//
        int piecesPlaced = 0;
        float timer = 0f;

        private bool gameStarted = false;
        private bool gameWon = false;

        GameBoard gameBoard;
        Shape draggedShape = null;

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

            gameBoard = new GameBoard(GameBoard.BoardName.Unicorn);
            gameBoard.Initialize();

            pp = GraphicsDevice.PresentationParameters;

            renderTarget1 = new RenderTarget2D(GraphicsDevice, 
                pp.BackBufferWidth, pp.BackBufferHeight, 
                false, pp.BackBufferFormat, pp.DepthStencilFormat, 
                pp.MultiSampleCount, RenderTargetUsage.DiscardContents);
            renderTarget2 = new RenderTarget2D(GraphicsDevice,
                pp.BackBufferWidth, pp.BackBufferHeight,
                false, pp.BackBufferFormat, pp.DepthStencilFormat,
                pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            lastMouseState = Mouse.GetState();

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
            flickering = new Flickering(GraphicsDevice, spriteBatch);
            background_Sprite = Content.Load<Texture2D>("v2_ui");
            spriteFont = Content.Load<SpriteFont>("font");
            song = Content.Load<Song>("Chiptronical");
            MediaPlayer.Play(song);
            startButton = new Button("start", Content)
            {
                Position = new Vector2(WINDOW_WIDTH - 320 , WINDOW_HEIGHT - 120),
            };

            startButton.Click += StartButton_Click;

            fallingShapes = new FallingShapes(Content);

            flickering.LoadContent(Content, pp);

            gameBoard.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void StartButton_Click(object sender, System.EventArgs e)
        {
            timer = 0;
            piecesPlaced = 0;
            gameWon = false;
            gameBoard.ClearBoard();
            gameStarted = true;
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

            startButton.Update(gameTime);

            //we can change this but for now the pieces start falling only after start button clicked
            if (gameStarted && piecesPlaced < gameBoard.boardPieceCount)
            {
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                fallingShapes.Update(gameTime);
                fallingShapes.UpdateShatteredShapes(gameTime, Content);
                flickering.Settings = FlickeringSettings.PresetSettings[0];
            }
            else
            {
                if(piecesPlaced == gameBoard.boardPieceCount)
                {
                    gameWon = true;
                    flickering.Settings = FlickeringSettings.PresetSettings[0];
                }
                fallingShapes.ClearShapes(Content);
                gameStarted = false;
            }
     

            base.Update(gameTime);

            mouseState = Mouse.GetState();
            
            // Mouse down event
            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != ButtonState.Pressed && gameStarted)
            {
                //Console.WriteLine("Mouse clicked!");
                List<int> indexesOfShapesClicked = ShapeClicker.GetIndexesOfShapesClicked(fallingShapes.GetFallingShapes(), mouseState.X, mouseState.Y);
                foreach(int index in indexesOfShapesClicked)
                {
                    draggedShape = fallingShapes.GetFallingShapes()[index]; // This will ultimately result in the top piece being selected
                }
                if (draggedShape != null)
                {
                    fallingShapes.RemoveShape(draggedShape);
                }
            }
            else if ((mouseState.LeftButton != ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Pressed && gameStarted))
            {
                // Mouse up event
                bool aPieceWasPlaced = false;
                 // If this piece was placed
                if (draggedShape != null && gameBoard.SaturateIfNamePrefixMatch(mouseState.X, mouseState.Y, draggedShape.GetName(), THRESHOLD_FOR_PLACING_PIECES))
                {
                    // TODO: Remove this piece from the list of pieces that can fall
                    aPieceWasPlaced = true;
                    //increase counter of pieces placed
                    piecesPlaced++;
                }

                if (!aPieceWasPlaced)
                {
                    if(draggedShape != null)
                    {
                        fallingShapes.AddShape(draggedShape);
                    }
                }

                // Regardless, drop the shape
                draggedShape = null;
            }

            if(draggedShape != null)
            {
                draggedShape.SetPosition(new Vector2(mouseState.X - draggedShape.GetCenter().X, mouseState.Y - draggedShape.GetCenter().Y));
            }

            lastMouseState = mouseState;

            bloomSatPulse += bloomSatDir;
            if (bloomSatPulse > 2.5f) bloomSatDir = -0.04f;
            if (bloomSatPulse < 0.1f) bloomSatDir = 0.04f;
            flickering.Settings.BloomSaturation = bloomSatPulse;

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if(gameWon)
            {
                GraphicsDevice.SetRenderTarget(renderTarget1);
                GraphicsDevice.Clear(Color.TransparentBlack);
                spriteBatch.Begin();
                gameBoard.Draw(spriteBatch, gameWon);
                spriteBatch.End();
                flickering.Draw(renderTarget1, renderTarget2);
                GraphicsDevice.SetRenderTarget(null);
            }
            else
            {
                GraphicsDevice.SetRenderTarget(renderTarget1);
                GraphicsDevice.Clear(Color.TransparentBlack);
                flickering.Draw(renderTarget1, renderTarget2);
                GraphicsDevice.SetRenderTarget(null);
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            if (showBackground) {
                spriteBatch.Draw(background_Sprite, GraphicsDevice.Viewport.Bounds, Color.White);
            }
            else {
                spriteBatch.Draw(background_Sprite, GraphicsDevice.Viewport.Bounds, Color.White * 0.95f);
                showBackground = true;
            }

            if(!gameWon)
            {
                gameBoard.Draw(spriteBatch, gameWon);
            }

            if (gameStarted)
            {
                fallingShapes.Draw(spriteBatch);
                if (draggedShape != null)
                {
                    draggedShape.Draw(spriteBatch);
                }
            }
            else
                startButton.Draw(spriteBatch);

            Vector2 stringPos = timer < 10 ? new Vector2(WINDOW_WIDTH - 180 , 35 ) : new Vector2(WINDOW_WIDTH - 200, 35);
            spriteBatch.DrawString(spriteFont, Math.Ceiling(timer).ToString(), stringPos, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 0);

            spriteBatch.End();

            spriteBatch.Begin(0, BlendState.AlphaBlend);
            spriteBatch.Draw(renderTarget2, new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
