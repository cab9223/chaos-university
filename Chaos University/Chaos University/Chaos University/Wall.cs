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
    class Wall : GamePiece
    {

        public Wall(int x, int y, List<Texture2D> textures) //Constructor
            : base(x, y, textures)
        {
            TileState = TileState.Wall;
        }

        public bool CheckCollision(GamePiece obj)//Method Checks to see if player hit a wall
        {
            if (obj.PositionRect.Intersects(this.PositionRect))
            {
                return true;
            }
            return false;
        }

        public override void Draw(SpriteBatch obj)
        {
            base.Draw(obj);
        }
    }
}
