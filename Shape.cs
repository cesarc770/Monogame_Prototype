using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rapid_Prototype_1
{
    class Shape
    {
        private Vector2 position;
        private int speed = 220;
        private int center = 144;
        private Texture2D sprite;
        private string spriteName;
        

        public Shape(Vector2 pos, int s, int c)
        {
            position = pos;
            speed = s;
            center = c;
        }

        public Shape(Vector2 pos, int s, int c, string spName)
        {
            position = pos;
            speed = s;
            center = c;
            spriteName = spName;
        }

        public void ShapeUpdate(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y += speed * dt;
        }

        public void SetPosition(Vector2 p)
        {
            position = p;
        } 
        
        public void SetSpeed(int s)
        {
            speed = s;
        } 
        
        public void SetCenter(int c)
        {
            center = c;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public int GetSpeed()
        {
            return speed;
        }

        public int GetCenter()
        {
            return center;
        }

        public Texture2D getSprite()
        {
            return sprite;
        }

        public void setSprite(Texture2D s)
        {
            sprite = s;
        }

        public string getSpriteName()
        {
            return spriteName;
        }

        public void setSpriteName(string s)
        {
            spriteName = s;
        }
    }
}
