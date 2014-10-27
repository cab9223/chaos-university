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
        private string plrName { get; set; } //Players name

        private int money { get; set; } //Players cash

        private int score { get; set; } //Perhaps points accumulated, maybe


        public Player(int x, int y, int direction, string name, string imageFile) //Constructor
            : base(x, y, direction)
        {
            plrName = name;  //sets player name
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        // Checks position with a single object.
        public bool CheckPosition(GamePiece thing)
        {
            if ((this.PositionRect.X / GlobalVar.TileSize == thing.PositionRect.X / GlobalVar.TileSize) && (this.PositionRect.Y / GlobalVar.TileSize == thing.PositionRect.Y / GlobalVar.TileSize))
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
            money = money - costs;

            if (money < 0)
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
    }
}
