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
        public bool Active { get; set; }//Is this money currently active on screen

        public int Amount { set; get; } //The value of the money object


        public Money(int x, int y, int amount) //Constructor
            : base(x, y)
        {
            Amount = amount;
        }


        public bool CheckCollision(GamePiece obj)//Method Checks to see if player got the money
        {
            if (Active == true)
            {
                if (obj.positionRect.Intersects(this.positionRect))
                {
                    Active = false;
                    return true;
                }
            }
            return false;
        }


        public override void Draw(SpriteBatch obj)//Draws to screen Money if it is active
        {
            if (Active == true)
            {
                base.Draw(obj);
            }
        }
    }
}
