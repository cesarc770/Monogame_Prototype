using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Prototype_1
{
    class Button
    {
        private MouseState currentState;
        private MouseState previousState;
        private SpriteFont font;
        private bool isHovering;
        private Texture2D texture;

        public event EventHandler Click;
        public bool Clicked { get; private set; }
        public Vector2 Position { get; set; }
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
            }
        }

        public string Text { get; set; }


        public Button(string spriteName, ContentManager content)
        {
            texture = content.Load<Texture2D>(spriteName);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var color = Color.White;

            if (isHovering)
                color = Color.Gray;

            spriteBatch.Draw(texture, Rectangle, color);
            
        }

        public void Draw(SpriteBatch batch)
        {
            var color = Color.White;

            if (isHovering)
                color = Color.Gray;

            batch.Draw(texture, Rectangle, color);
        }

        public void Update(GameTime gameTime)
        {
            previousState = currentState;
            currentState = Mouse.GetState();

            var mouseRectangle = new Rectangle(currentState.X, currentState.Y, 1, 1);

            isHovering = false;
            if (mouseRectangle.Intersects(Rectangle))
            {
                isHovering = true;

                if (currentState.LeftButton == ButtonState.Released && previousState.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
           
        }
    }
}
