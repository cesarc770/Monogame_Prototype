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

    class ShatterManager
    {

        public const float SHATTER_HEIGHT = 990.0f;

        private const float LIFETIME_IN_MS = 1000.0f;

        private List<ShatteredPiece> shatteredPieces = new List<ShatteredPiece>();
        private List<ShatteredPiece> deadPieces = new List<ShatteredPiece>();
        private List<Shape> shapesToResurrect = new List<Shape>();
        

        public void AddPiece(Shape shape, GameTime gameTime, ContentManager content)
        {
            shatteredPieces.Add(new ShatteredPiece(shape, 
                shape.GetPosition(),
                gameTime.TotalGameTime.TotalMilliseconds,
                LIFETIME_IN_MS,
                shape.GetName() + "_shattered",
                shape.GetScale(),
                content
                ));
        }

        public void Update(GameTime gameTime)
        {
            foreach(ShatteredPiece piece in shatteredPieces)
            {
                piece.Update(gameTime);
                if(piece.isDead)
                {
                    deadPieces.Add(piece);
                }
            }

            foreach(ShatteredPiece piece in deadPieces)
            {
                shatteredPieces.Remove(piece);
                if(!shapesToResurrect.Contains(piece.GetShape()))
                {
                    shapesToResurrect.Add(piece.GetShape());
                }
            }
            deadPieces.Clear();
        }

        public List<Shape> GetShapesToResurrect()
        {
            return shapesToResurrect;
        }

        public void ClearShapesToResurect()
        {
            shapesToResurrect.Clear();
        }

        public void Draw(SpriteBatch batch)
        {
            foreach(ShatteredPiece piece in shatteredPieces)
            {
                piece.Draw(batch);
            }
        }

    }
}
