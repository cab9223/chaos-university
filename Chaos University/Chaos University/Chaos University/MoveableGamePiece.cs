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
            : base(x, y)
        {
            direction = dir;
        }

        // standard 4-direction setup.
        private int direction;
        public int Direction
        {
            get
            {
                return direction;
            }

            set
            {
                direction = value;
            }
        }


        public int X //Property easily sets Position Rectangles X value
        {
            get { return positionRect.X; }
            set { positionRect.X = value; }
        }

        public int Y //Property easily sets Position Rectangles Y value
        {
            get { return positionRect.Y; }
            set { positionRect.Y = value; }
        }





    }
}
