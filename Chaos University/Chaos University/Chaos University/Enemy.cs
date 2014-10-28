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
        public int Difficulty { get; set; }


        public Enemy(int x, int y, int dir, string imageFile) //Constructor
            : base(x, y, dir)
        {
            
        }


        public void Attack() //Method for attacking player
        {
            //Stub
        }


        public void Patrol() //Method to move enemy around map
        {
            switch (Difficulty)
            {
                case 0:
                    //Simple back and forth partol, stops at cirtain distance or wall
                    break;
                case 1:
                    //Moves in any direction based on an algorithm, stops at walls, can hit traps
                    break;
                case 2:
                    //Moves in any direction based on an algorithm, stops at walls, avoids traps, maybe more
                    break;
            }
        }


        public bool CheckCollision(GamePiece obj)//Method Checks to see if enemy hits something
        {
            /*if (obj.positionRect.Intersects(this.positionRect))
            {
                return true;
            }
            return false;*/
            if ((this.PositionRect.X / GlobalVar.TILESIZE == obj.PositionRect.X / GlobalVar.TILESIZE) && (this.PositionRect.Y / GlobalVar.TILESIZE == obj.PositionRect.Y / GlobalVar.TILESIZE))
            {
                return true;
            }

            else
            {
                return false;
            }
        }


        public override void Draw(SpriteBatch obj) //Draws player using base draw
        {
            base.Draw(obj);
        }
    }
}
