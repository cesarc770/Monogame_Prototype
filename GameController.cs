using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rapid_Prototype_1
{
    class GameController
    {
        public double timer = 2.0;
        public List<Shape> fallingShapes = new List<Shape>();

        //list to hold all possible falling shapes in the game for reference
        public Queue<Shape> allFallingShapes = new Queue<Shape>();

        public GameController()
        {
            //instantiate all shapes needed for list
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), 100, 40, "Unicorn_back_left_leg_sat"));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), 100, 40, "Unicorn_back_left_leg_sat"));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), 100, 40, "Unicorn_back_left_leg_sat"));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), 100, 40, "Unicorn_back_left_leg_sat"));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), 100, 40, "Unicorn_back_left_leg_sat"));
            allFallingShapes.Enqueue(new Shape(new Vector2(200, 0), 100, 40, "Unicorn_back_left_leg_sat"));
        }


        public void ControllerUpate(GameTime gameTime)
        {
            //logic nees to be updated to make sure the hit the line in time
            //this is just t test they can fall - every two seconds they get created and start falling
            timer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0 && allFallingShapes.Count > 0) 
            {
                fallingShapes.Add(allFallingShapes.Dequeue());
                timer = 2.0;
            }

        }
    }
}
