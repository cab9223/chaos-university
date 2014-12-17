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

        //Number of level types
        public static int levelsOpening = 12;   //12
        public static int levelsNinja = 10;     //10
        public static int levelsRecon = 7;
        public static int levelsAssault = 1;
        public static int levelsBoss = 1;

        // Allows us to edit the size of a single tile- also useful for a bunch of other things that have been startedo n.
        public static int TILESIZE = 50;

        //Colors that CaracterCreator edits, set to white(no tint) by default
        public static Int16[] ColorsSplit = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        public static int ParCount = 0; //For initial par count

        public static int SpeedLevel = 50; //50 is normal, 40 is max
    }
}
