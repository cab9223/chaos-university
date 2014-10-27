using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaos_University
{
    class GlobalVar
    {
        // All the variables that we'll need

        // These two let us change the width and height of the game at-will, without messing up
        // our other equations. 
        public const int GameWidth = 800;
        public const int GameHeight = 600;

        // Allows us to edit the size of a single tile- also useful for a bunch of other things that have been startedo n.
        public const int TileSize = 50;
    }
}
