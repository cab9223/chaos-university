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
            AnimTime = 0;
            direction = 3.14159f;
        }

        public int AnimTime { get; set; }

        private float direction;

        public Player User { get; set; }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch obj, int offX, int offY)
        {
            if (User.ThisMajor == Player.Major.Ninja)
            {
                if ( AnimTime <= 50)
                {
                    obj.Draw(listTextures[1],
                            new Rectangle(User.PositionRect.X,
                                User.PositionRect.Y,
                                listTextures[1].Width,
                                listTextures[1].Height),
                            null,
                            Color.FromNonPremultiplied(GlobalVar.ColorsSplit[6], GlobalVar.ColorsSplit[7], GlobalVar.ColorsSplit[8], 255),
                            direction,
                            new Vector2(User.PositionRect.X, User.PositionRect.Y),
                            SpriteEffects.None,
                            0.0f);

                    AnimTime++;

                }

                if (AnimTime >= 50)
                {
                    User.AbilityActive = false;
                }
            }
        }

        public void Rotate(float increment)
        {
            direction += AnimTime * increment;
        }
    }
}
