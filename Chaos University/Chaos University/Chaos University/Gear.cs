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
        float rotDir;

        public Gear(int x, int y, List<Texture2D> textures, Player owner)
        {
            rotDir = 0;

            User = owner;

            listTextures = textures;

            IndexTexture = 0;

            PositionRect = new Rectangle(x - (GlobalVar.TILESIZE * 3) / 2, y - (GlobalVar.TILESIZE * 3) / 2, GlobalVar.TILESIZE * 3, GlobalVar.TILESIZE * 3);

        }

        public Player User { get; set; }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch obj, int offX, int offY)
        {
            Rectangle offsetRect = new Rectangle(
                PositionRect.X + offX,
                PositionRect.Y + offY,
                PositionRect.Width,
                PositionRect.Height);
            
            if (User.ThisMajor == Player.Major.Ninja)
            {                
                if ( rotDir <= 2 * Math.PI)
                {
                        obj.Draw(listTextures[0],
                    new Rectangle(offsetRect.X + offsetRect.Width / 6 + PositionRect.Width / 2, offsetRect.Y + GlobalVar.TILESIZE / 2 + PositionRect.Width / 2, listTextures[0].Width, listTextures[0].Height),
                    null,
                    Color.White,
                    rotDir,
                    new Vector2(GlobalVar.TILESIZE / 2, listTextures[0].Height),
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
                new Rectangle((offsetRect.X + offsetRect.Width / 2) + 20, (offsetRect.Y + offsetRect.Height / 2) + 20, 50, 50),
                null,
                Color.White,
                rotDir,
                new Vector2(User.PositionRect.Width / 2, User.PositionRect.Height / 2),
                SpriteEffects.None,
                0.0f);

                User.AbilityActive = false;

                }
            }

            if (User.ThisMajor == Player.Major.Recon)
            {
                switch (User.Direction)
                {
                    case 0:
                        if (User.AbilityActive == true)
                        {
                            obj.Draw(listTextures[0],
                             new Rectangle(offsetRect.X + offsetRect.Width / 2 + 25, offsetRect.Y + offsetRect.Height / 2 - 40, 25, 75),
                             null,
                             Color.White,
                             (float)(Math.Atan2(-1, 0) + Math.PI / 2),
                             new Vector2(User.PositionRect.Width / 2, User.PositionRect.Height / 2),
                             SpriteEffects.None,
                             0.0f);
                        }
                        break;
                    case 1:
                        if (User.AbilityActive == true)
                        {
                            obj.Draw(listTextures[0],
                            new Rectangle(offsetRect.X + offsetRect.Width / 2 + 90, offsetRect.Y + offsetRect.Height / 2 + 25, 25, 75),
                            null,
                            Color.White,
                            (float)(Math.Atan2(0, 1) + Math.PI / 2),
                            new Vector2(User.PositionRect.Width / 2, User.PositionRect.Height / 2),
                            SpriteEffects.None,
                            0.0f);
                        }
                        break;
                    case 2:
                        if (User.AbilityActive == true)
                        {
                            obj.Draw(listTextures[0],
                            new Rectangle(offsetRect.X + offsetRect.Width / 2 + 25, offsetRect.Y + offsetRect.Height / 2 + 90, 25, 75),
                            null,
                            Color.White,
                            (float)(Math.Atan2(1, 0) + Math.PI / 2),
                            new Vector2(User.PositionRect.Width / 2, User.PositionRect.Height / 2),
                            SpriteEffects.None,
                            0.0f);
                        }
                        break;
                    case 3:
                        if (User.AbilityActive == true)
                        {
                            obj.Draw(listTextures[0],
                            new Rectangle(offsetRect.X + offsetRect.Width / 2 - 40, offsetRect.Y + offsetRect.Height / 2 + 25, 25, 75),
                            null,
                            Color.White,
                            (float)(Math.Atan2(0, -1) + Math.PI / 2),
                            new Vector2(User.PositionRect.Width / 2, User.PositionRect.Height / 2),
                            SpriteEffects.None,
                            0.0f);
                        }
                        break;
                }

            }

        }

        public void Rotate(float increment)
        {
            rotDir += 300 * (float)Math.PI * increment;
        }

    }
}
