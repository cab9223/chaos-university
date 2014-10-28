using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaos_University
{
    class Goal : GamePiece
    {

        public Goal(int x, int y) //Constructor
            : base(x, y, "placeholder")
        {
            //Nothing yet
        }


        public bool CheckCollision(GamePiece obj)//Method Checks to see if player reached the goal
        {
            if (obj.positionRect.Intersects(this.positionRect))
            {
                return true;
            }
            return false;
        }


        public void LevelComplete() //Method to end level and transition to menu
        {
            //stub
        }

    }
}
