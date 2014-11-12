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
    class Indicator : GamePiece
    {
        public Indicator(int x, int y, List<Texture2D> textures)
            : base(x, y, textures)
        {
            PieceState = PieceState.Indicator;
        }
    }
}
