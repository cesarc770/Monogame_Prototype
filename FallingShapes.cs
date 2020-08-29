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

        private List<Shape> shapesInBeatBar = new List<Shape>(); // This will almost always havev 0 or 1 entries, more if we add more lanes

        const int SCREEN_HEIGHT = 1080;
        const int FALL_LENGTH = 816;
        const int BEAT_BAR_SIZE = 114;
        const float MINUTES_PER_SECOND = 1 / 60f;
        const int PIECES_ON_SCREEN = 4;

        public FallingShapes(ContentManager content, float bpm) {

            float fallSpeed = (FALL_LENGTH * bpm * MINUTES_PER_SECOND) / PIECES_ON_SCREEN;
            // Add one of each piece
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_back_left_leg_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_back_neck_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_back_right_leg_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_body_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_chest_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_front_legs_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_front_neck_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_horn_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_nose_sat", content));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), fallSpeed, "Unicorn_tail_sat", content));
        }


        public void SpawnPiece() {
            if (allFallingShapes.Count != 0) {
                fallingShapes.Add(allFallingShapes.Dequeue());
            }
        }

        public void Update(GameTime gameTime) {
            List<Shape> shapesToWrap = new List<Shape>();
            foreach (Shape shape in fallingShapes) {
                shape.Update(gameTime);
                float shapeTop = shape.GetPosition().Y - shape.GetCenter().Y;
                float shapeBottom = shape.GetPosition().Y + shape.GetCenter().Y;
                // If any part of the shape is in the shape bar
                if (shapeTop <= FALL_LENGTH + BEAT_BAR_SIZE/2 &&
                    shapeBottom >= FALL_LENGTH -BEAT_BAR_SIZE/2)
                {
                    // If this shape is not already counted as in the beat bar
                    if(!shapesInBeatBar.Contains(shape))
                    {
                        // Add it to the list of shapes in the beat bar
                        shapesInBeatBar.Add(shape);
                    }
                }
                // If no part of the shape is in the beat bar and it is still counted as in the beat bar
                if(shapeTop >= FALL_LENGTH + BEAT_BAR_SIZE/2 && shapesInBeatBar.Contains(shape))
                {
                    shapesInBeatBar.Remove(shape);
                }
                
                // If a piece is all the way off screen
                if (shapeTop >= SCREEN_HEIGHT)
                {
                    shapesToWrap.Add(shape);
                }
            }
            // This will almost always have 0 or 1 entry; if we add multiple lanes later, this could have more
            foreach (Shape shape in shapesToWrap)
            {
                fallingShapes.Remove(shape);
                // Comment these to stop the pieces from wrapping
                shape.ResetPosition();
                allFallingShapes.Enqueue(shape); 
            }
            shapesToWrap.Clear();
        }

        public void Draw(SpriteBatch batch) {
            foreach (Shape shape in fallingShapes) {
                shape.Draw(batch);
            }
        }

        public List<Shape> GetShapesInBeatBar()
        {
            return shapesInBeatBar;
        }
    }
}
