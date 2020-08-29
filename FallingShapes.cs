using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rapid_Prototype_1
{
    class FallingShapes
    {
        private List<Shape> fallingShapes = new List<Shape>();
        private Queue<Shape> allFallingShapes = new Queue<Shape>();

        const int FALL_LENGTH = 816;
        const float MINUTES_PER_SECOND = 1 / 60f;
        const int PIECES_ON_SCREEN = 4;

        public FallingShapes(ContentManager content, float bpm) {

            float fallSpeed = (FALL_LENGTH * bpm * MINUTES_PER_SECOND) / PIECES_ON_SCREEN;
            const int NUM_SHAPES = 20;
            for (int i = 0; i < NUM_SHAPES; ++i) {
                allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_back_left_leg_sat", content));
            }

        }


        public void SpawnPiece() {
            if (allFallingShapes.Count != 0) {
                fallingShapes.Add(allFallingShapes.Dequeue());
            }
        }

        public void Update(GameTime gameTime) {
            foreach (Shape shape in fallingShapes) {
                shape.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch batch) {
            foreach (Shape shape in fallingShapes) {
                shape.Draw(batch);
            }
        }
    }
}
