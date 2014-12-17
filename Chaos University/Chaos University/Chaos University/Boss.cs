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
    class Boss : MoveableGamePiece //Boss class
    {
        private bool bossactive;
        private int initialWidth;
        private int initialHeight;

        public bool BossActive
        {
            get { return bossactive; }
            set { bossactive = value; }
        }

        public int InitialWidth
        {
            get { return initialWidth; }
            set { initialWidth = value; }
        }

        public int InitialHeight
        {
            get { return initialHeight; }
            set { initialHeight = value; }
        }

        public Boss(int x, int y, int width, int height, int dir, List<Texture2D> textures)
            : base(x, y, width, height, dir, textures)
        {
            BossActive = false;
            InitialWidth = width;
            InitialHeight = height;
        }

        public override void Move(int distance)
        {                                
            PositionRect = new Rectangle(
            PositionRect.X,
            PositionRect.Y + distance,
            PositionRect.Width,
            PositionRect.Height);
             
        }

        public override void Draw(SpriteBatch obj, int offX, int offY) //Draws boss
        {

            obj.Draw(listTextures[0],
                new Rectangle((PositionRect.X + PositionRect.Width / 2) + offX, (PositionRect.Y + PositionRect.Height / 2) + offY, PositionRect.Width, PositionRect.Height),
                null,
                Color.White,
                (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                new Vector2(listTextures[0].Width / 2, listTextures[0].Height / 2),
                SpriteEffects.None,
                0.0f);
        }

        public void ResetBoss()
        {
            this.PositionRect = new Rectangle(InitialX, InitialY, InitialWidth, InitialHeight);
        }

        
            
        
    }
}
