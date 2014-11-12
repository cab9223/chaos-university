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
    public static class GlobalVar
    {
        // All the variables that we'll need

        // These two let us change the width and height of the game at-will, without messing up
        // our other equations. 
        public static int GAMEWIDTH = 800;
        public static int GAMEHEIGHT = 600;

        // Allows us to edit the size of a single tile- also useful for a bunch of other things that have been startedo n.
        public static int TILESIZE = 50;

        //Colors that CaracterCreator edits, set to white(no tint) by default
        public static Int16[] ColorsSplit = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static int ParCount = 0;
        public static int Tries = 3;

    }
}
