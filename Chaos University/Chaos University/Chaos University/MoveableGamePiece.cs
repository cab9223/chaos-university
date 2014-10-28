using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Chaos_University
{
    class MoveableGamePiece : GamePiece
    {
        public MoveableGamePiece(int x, int y, int dir) //Constructor
            : base(x, y, "placeholder")
        {
            direction = dir;
        }

        // standard 4-direction setup.
        // 0 = Up, 1 = Right, 2 = Down, 3 = Left
        private int direction;
        public int Direction
        {
            get
            {
                return direction;
            }

            set
            {
                if ((value > -1) && (value < 4)) //Test to make sure its direction is from only 0 to 3
                {
                    direction = value;
                }
                else
                {
                    direction = 0;
                }
            }
        }

        //Don't think this is needed. Am now using stuff that was gone over in class 10/26 to change position.
        /*public int X //Property easily sets Position Rectangles X value
        {
            get { return positionRect.X; }
            set { positionRect.X = value; }
        }

        public int Y //Property easily sets Position Rectangles Y value
        {
            get { return positionRect.Y; }
            set { positionRect.Y = value; }
        }*/


        public abstract void Move();
        
    }
}
