using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rapid_Prototype_1
{
    class Shape
    {
        private Vector2 position;
        private float speed;
        private Vector2 center;
        private Texture2D sprite;
        

        public Shape(Vector2 pos, float speed, string spriteName, ContentManager content) { 
            position = pos;
            this.speed = speed;

            sprite = content.Load<Texture2D>(spriteName);

            center = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        private Vector2 Position {
            get => position - center;
        }

        public void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += speed * dt;
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(sprite, Position, Color.White);
        }
    }
}
