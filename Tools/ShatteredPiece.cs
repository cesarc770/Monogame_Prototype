using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapid_Prototype_1.Tools
{
    class ShatteredPiece
    {
        public bool isDead = false;

        private Shape shape;
        private Vector2 position;
        private double birthTime;
        private double lifeSpanInMS;
        private Texture2D asset;
        private Vector2 scale;
        private float alpha = 0f;

        public ShatteredPiece(Shape originalShape, Vector2 pos, double bTime, double lifeSpan, string assetName, Vector2 assetScale, ContentManager content)
        {
            shape = originalShape;
            position = pos;
            birthTime = bTime;
            lifeSpanInMS = lifeSpan;
            asset = content.Load<Texture2D>(assetName);
            SwapAssetToColor(Color.Red);
            scale = assetScale;
        }

        public void Update(GameTime gameTime)
        {
            if(gameTime.TotalGameTime.TotalMilliseconds >= birthTime + lifeSpanInMS)
            {
                isDead = true;
            }
            else
            {
                alpha = 1 - MathHelper.Clamp((float)((gameTime.TotalGameTime.TotalMilliseconds - birthTime) / lifeSpanInMS), 0f, 1f);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if(!isDead && asset != null)
            {
                batch.Draw(asset, position, null, Color.White*alpha, 0, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
        }

        public Shape GetShape()
        {
            return shape;
        }

        private void SwapAssetToColor(Color color)
        {
            Color[] colors = new Color[asset.Width * asset.Height];

            asset.GetData(colors);

            for(int i = 0; i < asset.Width*asset.Height; i++)
            {
                if(colors[i].A > 128)
                {
                    colors[i] = color;
                }
            }

            asset.SetData(colors);
        }
    }
}
