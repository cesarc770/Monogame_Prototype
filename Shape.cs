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
        private Vector2 scale;
        private Texture2D sprite;
        private string assetName;

        private Vector2 initialPosition;

        public Shape(float x, float speed, Vector2 scale, string spriteName, ContentManager content) { 
            position = new Vector2(x, 0);
            initialPosition = new Vector2(x, 0);
            this.speed = speed;
            this.scale = scale;

            sprite = content.Load<Texture2D>(spriteName);
            assetName = spriteName;

            center = new Vector2(sprite.Width * scale.X / 2, sprite.Height * scale.Y / 2);
        }

        public void Update(GameTime gameTime) {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += speed * dt;
        }

        public void Draw(SpriteBatch batch) {
            batch.Draw(sprite, position, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void ResetPosition()
        {
            position = initialPosition;
        }
        public Vector2 GetCenter()
        {
            return center;
        }

        public string GetName()
        {
            return assetName;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector2 pos)
        {
            position = pos;
        }

        public Vector2 GetScale()
        {
            return scale;
        }

        public Texture2D GetSprite()
        {
            return sprite;
        }

    }
}
