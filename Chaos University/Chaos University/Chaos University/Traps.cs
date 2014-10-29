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
    class Traps : GamePiece
    {
        public bool Active { get; set; }

        public string TrapType { get; set; }

        public int Direction { get; set; }

        public Traps(int x, int y, string type) //Constructor
            : base(x, y)
        {
            Active = true;

            TrapType = type;

            if (type.ToLower().CompareTo("movement") == 0)
            {
                TrapType = "MvtTrap";
            }
        }

        public Traps(int x, int y, string type, int dir) //Constructor
            : base(x, y)
        {
            Active = true;

            TrapType = type;

            if (type.ToLower().CompareTo("movement") == 0)
            {
                TrapType = "MvtTrap";

                Direction = dir;
            }
        }



        public void HitTrap(MoveableGamePiece thing)
        {
            if (Active)
            {
                //Code for player hitting a trap, depending on what the trapType is
                if (TrapType.ToLower().CompareTo("movement") == 0)
                {
                    thing.Direction = this.Direction;
                }
            }
        }


        public void DisableTrap(bool safely) //Checks to see if trap was disabled and if done safely
        {
            if (Active == true && safely == true)
            {
                Active = false;
            }
            else if (Active == true && safely == false)
            {
                Active = false;
                //Add some negative effect depending on the trapType
            }
        }


        public override void Draw(SpriteBatch obj)//Draws to screen Trap if it is active
        {
            if (Active ==  true)
            {
                if (TrapType == "MvtTrap")
                {
                    obj.Draw(this.CurrentTexture[Direction], positionRect, Color.White);
                }
                
            }
        }

    }
}
