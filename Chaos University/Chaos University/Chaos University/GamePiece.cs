using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaos_University
{
    abstract class GamePiece
    {
        // Tentative direction things. Two rectangles- one for position, one for spot on grid has been discussed.

        int xpos;
        public int XPos
        {
            get 
            {
                return xpos;
            }

            set
            {
                xpos = value;
            }
        }

        int ypos;
        public int YPos
        {
            get
            {
                return ypos;
            }

            set
            {
                ypos = value;
            }
        }
    }
}
