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
using System.IO;

namespace Chaos_University
{
    class Gear : GamePiece
    {
        public Gear(int x, int y, List<Texture2D> textures, Player owner) : base(x, y, textures)
        {
            User = owner;

        }

        public Player User { get; set; }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch obj, int offX, int offY)
        {
            Rectangle offsetRect = new Rectangle(
                User.PositionRect.X + offX,
                User.PositionRect.Y + offY,
                50,
                90);
            
            if (User.ThisMajor == Player.Major.Ninja)
            {                
                if ( rotDir <= 2 * Math.PI)
                {
                        obj.Draw(listTextures[0],
                    new Rectangle(offsetRect.X + offsetRect.Width / 2, offsetRect.Y + offsetRect.Height / 2, offsetRect.Width, offsetRect.Height),
                    null,
                    Color.White,
                    rotDir,
                    new Vector2(0, listTextures[0].Height),
                    SpriteEffects.None,
                    0.0f);

                }

                if (rotDir > 2 * Math.PI)
                {
                    User.AbilityActive = false;
                }
            }
        }

        public void Rotate(float increment)
        {
            rotDir += 100 * (float)Math.PI * increment;
        }
    }
}
