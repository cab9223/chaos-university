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


        public Enemy(int x, int y, int dir, List<Texture2D> textures) //Constructor
            : base(x, y, dir, textures)
        {
            Difficulty = 0;
        }


        public bool Attack(Player obj) //Method for attacking player
        {
            if (this.CheckCollision(obj) == true)
            {
                //Maybe add an effect or sound to say the guard caught the player
                return true;
            }
            else
            {
                return false;
            }
        }


        public void Patrol() //Method to move enemy around map
        {
            switch (Difficulty)
            {
                case 0:
                    //Simple back and forth partol, stops at cirtain distance or wall


                    //Move(distance);
                    ////Commented this out for now because im not sure how we are using the wall pieces for detection!

                    //if (this.CheckCollision("Wall") == false)
                    //{
                    //    switch (Direction)
                    //    {
                    //        case 0:
                    //            Direction = 2;
                    //            Move();
                    //            break;

                    //        case 1:
                    //            Direction = 3;
                    //            Move();
                    //            break;

                    //        case 2:
                    //            Direction = 0;
                    //            Move();
                    //            break;

                    //        case 3:
                    //            Direction = 1;
                    //            Move();
                    //            break;
                    //    } 
                    //}
                    break;
                case 1:
                    //Moves in any direction based on an algorithm, stops at walls, can hit traps
                    break;
                case 2:
                    //Moves in any direction based on an algorithm, stops at walls, avoids traps, maybe more
                    break;
            }
        }


        //public bool CheckCollision(GamePiece obj)//Method Checks to see if enemy hits something
        //{
            
        //    if ((this.PositionRect.X / GlobalVar.TILESIZE == obj.PositionRect.X / GlobalVar.TILESIZE) && (this.PositionRect.Y / GlobalVar.TILESIZE == obj.PositionRect.Y / GlobalVar.TILESIZE))
        //    {
        //        return true;
        //    }

        //    else
        //    {
        //        return false;
        //    }
        //}


        public override void Draw(SpriteBatch obj) //Draws Enemy
        {
            obj.Draw(this.listTextures[Direction], PositionRect, Color.White);
        }


        public override void Move(int distance)
        {
            Rectangle temp = PositionRect;

            switch (Direction)
            {
                case 0:
                    temp.Y += distance;
                    break;

                case 1:
                    temp.X -= distance;
                    break;

                case 2:
                    temp.Y -= distance;
                    break;

                case 3:
                    temp.X += distance;
                    break;
            }

            PositionRect = temp;
        }
    }
}
