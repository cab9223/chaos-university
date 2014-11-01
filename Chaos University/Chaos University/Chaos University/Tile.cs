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

    // Pretty sure we will need a tile class to represent stationary things that take up a spot.
    class Tile : GamePiece
    {
        public Tile(int x, int y, List<Texture2D> textures) //Constructor
            : base(x, y, textures)
        {
            TileState = TileState.Floor;
        }

        public override void Draw(SpriteBatch obj)
        {
            base.Draw(obj);
        }

        public override void IncrementType()
        {
            switch(TileState)
            {
                case TileState.Floor:
                    TileState = TileState.North;
                    IndexTexture++;
                    break;
                case TileState.North:
                    TileState = TileState.East;
                    IndexTexture++;
                    break;
                case TileState.East:
                    TileState = TileState.South;
                    IndexTexture++;
                    break;
                case TileState.South:
                    TileState = TileState.West;
                    IndexTexture++;
                    break;
                case TileState.West:
                    TileState = TileState.Floor;
                    IndexTexture = 0;
                    break;
            }
        }
    }
}
