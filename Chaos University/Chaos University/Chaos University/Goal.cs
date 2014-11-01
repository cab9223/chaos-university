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
    class Goal : GamePiece
    {
        //Constructor
        public Goal(int x, int y, List<Texture2D> textures)
            : base(x, y, textures)
        {
            
        }
    }
}
