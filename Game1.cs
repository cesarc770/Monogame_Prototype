﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Rapid_Prototype_1.Tools;
using System.Globalization;
using System.Collections.Generic;

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
        MouseState lastMouseState;

        Button startButton;

        //shapes temporary
        FallingShapes fallingShapes;

        //**************//

        private bool gameStarted = false;

        // TODO: This needs to be the names of the assets used to create the pieces that are in the beat bar
        // TODO: For instance, "Unicorn_back_left_leg_sat"
        List<string> namesOfPiecesInBeatBar = new List<string>(); 
        GameBoard gameBoard;

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
            background_Sprite = Content.Load<Texture2D>("1080 ui no start");

            startButton = new Button("start", Content)
            {
                Position = new Vector2(WINDOW_WIDTH - 300 , WINDOW_HEIGHT - 80),
            };

            startButton.Click += StartButton_Click;

            fallingShapes = new FallingShapes(Content);

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
            if (gameStarted)
            {
                fallingShapes.Update(gameTime);
            }
     

            base.Update(gameTime);

            mouseState = Mouse.GetState();
            

            if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton != ButtonState.Pressed && gameStarted)
            {
                bool aPieceWasPlaced = false;
                List<Shape> shapesInBeatBar = fallingShapes.GetShapesInBeatBar();
                foreach(Shape shape in shapesInBeatBar)
                {
                    // If this piece was placed
                    if (gameBoard.SaturateIfNamePrefixMatch(mouseState.X, mouseState.Y, shape.GetName()))
                    {
                        // TODO: Remove this piece from the list of pieces that can fall
                        aPieceWasPlaced = true;
                        fallingShapes.RemoveShape(shape);
                    }
                }

                if(!aPieceWasPlaced)
                {
                    //TODO: Deduct points for a missed click
                }
            }

            lastMouseState = mouseState;

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


            gameBoard.Draw(spriteBatch);

            if (gameStarted)
                fallingShapes.Draw(spriteBatch);
            else
                startButton.Draw(spriteBatch);

            spriteBatch.End();




            base.Draw(gameTime);
        }
    }
}
