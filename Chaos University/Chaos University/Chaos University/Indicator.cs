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
        private bool active;
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Indicator(int x, int y, List<Texture2D> textures)
            : base(x, y, textures)
        {
            PieceState = PieceState.Indicator;
            active = false;
        }

        //Indicator is only drawn if active.
        public override void Draw(SpriteBatch obj, int offX, int offY)
        {
            if (active)
                base.Draw(obj, offX, offY);
        }
    }
}
