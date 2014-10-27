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
    class Enemy : MoveableGamePiece
    {


        public Enemy(int x, int y, int dir, string imageFile) //Constructor
            : base(x, y, dir)
        {
            //Nothing yet
        }


        public void Attack() //Method for attacking player
        {

        }


        public void Patrol() //Method to move enemy around map
        {

        }


        public override void Draw(SpriteBatch obj) //Draws player using base draw
        {
            base.Draw(obj);
        }
    }
}
