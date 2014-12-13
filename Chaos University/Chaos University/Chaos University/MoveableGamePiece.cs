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
    abstract class MoveableGamePiece : GamePiece
    {
        // standard 4-direction setup.
        // 0 = Up, 1 = Right, 2 = Down, 3 = Left 
        //This is reversed for enemies
        private int direction;
        public int Direction
        {
            get
            {
                return direction;
            }

            set
            {
                //if ((value > -1) && (value < 4)) //Test to make sure its direction is from only 0 to 3
                //{
                    direction = value;
                //}
                //else
                //{
                //    direction = 0;
                //}
            }
        }


        private int initialDirection;
        public int InitialDirection
        {
            get
            {
                return initialDirection;
            }

            set
            {
                //if ((value > -1) && (value < 4)) //Test to make sure its direction is from only 0 to 3
                //{
                    initialDirection = value;
                //}
                //else
                //{
                //    initialDirection = 0;
                //}
            }
        }


        private bool moving;
        public bool Moving
        {
            get
            {
                return moving;
            }
            set
            {
                moving = value;
            }
        }


        //Vector drection To Draw Textures at.
        private Vector2 vector;
        public Vector2 Vector
        {
            get
            {
                return vector;
            }
            set
            {
                vector = value;
            }
        }

        //Constructor
        public MoveableGamePiece(int x, int y, int dir, List<Texture2D> textures)
            : base(x, y, textures)
        {
            direction = dir;
            vector = new Vector2(0, -1);
        }

        public virtual void Move(int value)
        {

        }
    }
}
