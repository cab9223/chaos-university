using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Chaos_University
{
    class Wall : GamePiece
    {

        public Wall(int x, int y) //Constructor
            : base(x, y, "placeholder") 
        {
            //Nothing yet
        }


        public bool CheckCollision(GamePiece obj)//Method Checks to see if player hit a wall
        {
            if (obj.positionRect.Intersects(this.positionRect))
            {
                return true;
            }
            return false;
        }


    }
}
