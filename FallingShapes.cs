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
        private List<Shape> allFallingShapes = new List<Shape>();

        const int SCREEN_HEIGHT = 1080;
        const int FALL_LENGTH = 816;
        const int MIN_SPEED = 300;
        const int MAX_SPEED = 500;
        const int MIN_X = 100;
        const int MAX_X = 1920 - 100;
        const float SCALE = 1.75f;

        const double MIN_SPAWN_INTERVAL = 500f;
        const double MAX_SPAWN_INTERVAL = 1000f;
        double nextSpawnTime;

        Random random = new Random();

        public FallingShapes(ContentManager content) {

            // Add one of each piece
            AddAllFallingShapes(content);


        }

        private float RandomFallSpeed() {
            return random.Next(MIN_SPEED, MAX_SPEED) + (float)random.NextDouble();
        }

        private int RandomXPos() {
            return random.Next(MIN_X, MAX_X);
        }

        private void SpawnPiece() {
            if (allFallingShapes.Count != 0) {
                fallingShapes.Add(allFallingShapes[0]);
                allFallingShapes.RemoveAt(0); 
            }
        }

        private double GetNextSpawnOffset() {
            return random.NextDouble() * (MAX_SPAWN_INTERVAL - MIN_SPAWN_INTERVAL) + MAX_SPAWN_INTERVAL;
        }

        public void Update(GameTime gameTime) {
            if (gameTime.TotalGameTime.TotalMilliseconds > nextSpawnTime) {
                SpawnPiece();
                nextSpawnTime = gameTime.TotalGameTime.TotalMilliseconds + GetNextSpawnOffset();
            }
            List<Shape> shapesToWrap = new List<Shape>();
            foreach (Shape shape in fallingShapes) {
                shape.Update(gameTime);
                float shapeTop = shape.GetPosition().Y - shape.GetCenter().Y;
                float shapeBottom = shape.GetPosition().Y + shape.GetCenter().Y; // Leaving this in for when we have pieces "shatter" on the bottom
                
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
                allFallingShapes.Add(shape); 
            }
            shapesToWrap.Clear();
        }

        public void Draw(SpriteBatch batch) {
            foreach (Shape shape in fallingShapes) {
                shape.Draw(batch);
            }
        }

        public void RemoveShape(Shape shape)
        {
            allFallingShapes.RemoveAll(item => shape.GetName() == item.GetName());
            fallingShapes.RemoveAll(item => shape.GetName() == item.GetName());
        }

        public void AddShape(Shape shape)
        {
            fallingShapes.Add(shape);
        }

        public void ClearShapes(ContentManager content)
        {
            fallingShapes.Clear();
            allFallingShapes.Clear();
            AddAllFallingShapes(content);
        }

        public void AddAllFallingShapes(ContentManager content)
        {
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_back_left_leg_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Eagle1", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_back_neck_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_back_right_leg_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_body_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Eagle3", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_chest_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_front_legs_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Eagle4", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_front_neck_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_horn_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Eagle5", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_nose_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Unicorn_tail_sat", content));
            allFallingShapes.Add(new Shape(RandomXPos(), RandomFallSpeed(), new Vector2(SCALE, SCALE), "Eagle10", content));
        }

        public List<Shape> GetFallingShapes()
        {
            return fallingShapes;
        }
    }
}
