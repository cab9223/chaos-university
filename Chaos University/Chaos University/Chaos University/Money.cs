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
    class Money : GamePiece
    {
        //true if the money appears active in game.
        private bool active;
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        //Constructor
        public Money(int x, int y, List<Texture2D> textures)
            : base(x, y, textures)
        {
            PositionRect = new Rectangle(
                PositionRect.X + (PositionRect.Width - textures[0].Width) / 2,
                PositionRect.Y + (PositionRect.Height - textures[0].Height) / 2,
                textures[0].Width,
                textures[0].Height);
            PieceState = PieceState.Collect;
            active = true;
        }

        //Money is only drawn if active.
        public override void Draw(SpriteBatch obj, int offX, int offY)
        {
            if(active)
                base.Draw(obj, offX, offY);
        }
    }
}

