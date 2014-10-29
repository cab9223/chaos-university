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
            StreamReader reader = new StreamReader(TitleContainer.OpenStream("Color.txt"));
            int readColor = Int32.Parse(reader.ReadLine());
            Rectangle temp;
            //Draws the five parts of the character on top of each other.
            switch(readColor)
            {
                default:
                    // Draws body.
                    obj.Draw(CurrentTexture[0], PositionRect, Color.White);
                    
                    // Draws vest.
                    temp = new Rectangle(
                        (PositionRect.X + PositionRect.Right)/2 + CurrentTexture[1].Width,
                        (PositionRect.Y + PositionRect.Bottom)/2 + CurrentTexture[1].Height,
                        CurrentTexture[1].Width,
                        CurrentTexture[1].Height);

                    obj.Draw(CurrentTexture[1], temp, Color.White);

                    // Draws head.
                    temp = new Rectangle(
                        (PositionRect.X + PositionRect.Right)/2 + CurrentTexture[2].Width,
                        (PositionRect.Y + PositionRect.Bottom)/2 + CurrentTexture[2].Height,
                        CurrentTexture[2].Width,
                        CurrentTexture[2].Height);

                    obj.Draw(CurrentTexture[2], temp, Color.White);

                    // Draws bandana.
                    temp = new Rectangle(
                        (PositionRect.X + PositionRect.Right)/2 + CurrentTexture[3].Width,
                        (PositionRect.Y + PositionRect.Bottom)/2 + CurrentTexture[3].Height,
                        CurrentTexture[3].Width,
                        CurrentTexture[3].Height);

                    obj.Draw(CurrentTexture[3], temp, Color.White);

                    //Draws backpack.
                    temp = new Rectangle(
                        (PositionRect.X + PositionRect.Right)/2 + CurrentTexture[4].Width,
                        PositionRect.Bottom + CurrentTexture[4].Height,
                        CurrentTexture[4].Width,
                        CurrentTexture[4].Height);
                    obj.Draw(CurrentTexture[4], temp, Color.White);
                    break;
            }
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
