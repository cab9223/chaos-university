using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaos_University
{
    public static class GlobalVar
    {
        // All the variables that we'll need

        // These two let us change the width and height of the game at-will, without messing up
        // our other equations. 
        public static const int GAMEWIDTH = 800;
        public static const int GAMEHEIGHT = 600;

        // Allows us to edit the size of a single tile- also useful for a bunch of other things that have been startedo n.
        public static const int TILESIZE = 50;
    }
}
