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

    // Stuff here is pretty self-explanitory, the imageFile thing is for future implementation of image handling stuff.
    class Player : MoveableGamePiece
    {
        public string PlrName { get; set; } //Players name

        public int Money { get; set; } //Players cash

        public int Score { get; set; } //Perhaps points accumulated, maybe
        
        //public Majors PlrClass { get; set; } sets major, only for actual use if we implement a major system.

        public Player(int x, int y, int direction, string name, string imageFile) //Constructor
            : base(x, y, direction)
        {
            PlrName = name;  //sets player name
        }


        // Checks position with a single object.
        public bool CheckPosition(GamePiece thing)
        {
            if ((this.PositionRect.X / GlobalVar.TILESIZE == thing.PositionRect.X / GlobalVar.TILESIZE) && (this.PositionRect.Y / GlobalVar.TILESIZE == thing.PositionRect.Y / GlobalVar.TILESIZE))
            {
                return true;
            }

            else
            {
                return false;
            }
        }


        public bool ReduceMoney(int costs) //Reduces money after stage or purchase, if money is negative then returns true for a gameover state
        {
            Money = Money - costs;

            if (Money <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public override void Draw(SpriteBatch obj) //Draws player using base draw method
        {
            base.Draw(obj);
        }

        // Movement. Switch by direction. Will want to execute one update per seconde, or adjust rate of movement.
        public override void Move()
        {
            Rectangle temp = PositionRect;

            switch (Direction)
            {                    
                case 0:
                    temp.Y += GlobalVar.TILESIZE;
                    break;

                case 1:
                    temp.X -= GlobalVar.TILESIZE;
                    break;

                case 2:
                    temp.Y -= GlobalVar.TILESIZE;
                    break;

                case 3:
                    temp.X += GlobalVar.TILESIZE;
                    break;
            }

            PositionRect = temp;
        }

    }
}
