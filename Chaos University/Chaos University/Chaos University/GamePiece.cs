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
    abstract class GamePiece
    {
        public List<Texture2D> CurrentTexture { get; set; } //All GamePiece images


        // Tentative direction things. Two rectangles- one for position, one for spot on grid has been discussed.
        // Am going with a single rectangle for position, will divide by the size of a tile and ignore the remainder for spot on grid.
        public Rectangle positionRect;
        public Rectangle PositionRect
        {
            get
            {
                return positionRect;
            }

            set
            {
                positionRect = value;
            }
        }
        
        // Base constructor that everything else will draw from.
        public GamePiece(int x, int y)
        {
            //will need to do a try-catch block for loading the image file for this object.
            
            //Old constructor will give a runtime error(PositionRect initialized to nothing). Remove note once we have character setup working.
            positionRect = new Rectangle(x, y, CurrentTexture[0].Width, CurrentTexture[0].Height);
        }

        public virtual void Draw(SpriteBatch obj) //Draws any gameobject to screen
        {
            obj.Draw(this.CurrentTexture[0], positionRect, Color.White);
        }

        public abstract bool Collide();
    }
}
