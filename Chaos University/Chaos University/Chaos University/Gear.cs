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
                PositionRect.X + offX,
                PositionRect.Y + offY,
                50,
                75);
            
            if (User.ThisMajor == Player.Major.Ninja)
            {                
                if ( rotDir <= 2 * Math.PI)
                {
                        obj.Draw(listTextures[0],
                    new Rectangle(offsetRect.X + offsetRect.Width / 2, offsetRect.Y + offsetRect.Height / 3, 50, 75),
                    null,
                    Color.White,
                    rotDir,
                    new Vector2(User.PositionRect.Width / 2, User.PositionRect.Height/ 2 + this.PositionRect.Height),
                    SpriteEffects.None,
                    0.0f);


                }

                if (rotDir > 2 * Math.PI)
                {
                    User.AbilityActive = false;
                }

            }

            if (User.ThisMajor == Player.Major.Assault)
            {
                if (User.AbilityActive == true)
                {
                    obj.Draw(listTextures[0],
                new Rectangle(offsetRect.X + offsetRect.Width / 2, offsetRect.Y + offsetRect.Height / 3, 50, 50),
                null,
                Color.White,
                rotDir,
                new Vector2(User.PositionRect.Width / 2, User.PositionRect.Height / 2 + 10),
                SpriteEffects.None,
                0.0f);

                User.AbilityActive = false;

                }

            }
        }

        public void Rotate(float increment)
        {
            rotDir += 200 * (float)Math.PI * increment;
        }
    }
}
