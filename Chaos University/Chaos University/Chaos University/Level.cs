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
    class Level
    {
        private GamePiece[,] grid;          //Grid of game pieces
        private int gamePieceSize;          //Height of width of a game piece
        private List<Money> monies;         //List of non-grid objects on map. (currently exclusively monies).

        private Rectangle startNinja;             //Location at which ninja starts.
        public Rectangle StartNinja
        {
            get { return startNinja; }
            set { startNinja = value; }
        }

        private Rectangle startRecon;             //Location at which recon starts.
        public Rectangle StartRecon
        {
            get { return startRecon; }
            set { startRecon = value; }
        }

        private Rectangle startAssault;            //Location at which- yeah, yeah, you get the idea.
        public Rectangle StartAssault
        {
            get { return startAssault; }
            set { startAssault = value; }
        }

        private Player ninja;
        public Player Ninja
        {
            get { return ninja; }
            set { ninja = value; }
        }

        private Player recon;
        public Player Recon
        {
            get { return recon; }
            set { recon = value; }
        }

        private Player assault;
        public Player Assault
        {
            get { return assault; }
            set { assault = value; }
        }

        public List<Money> Monies
        {
            get
            {
                return monies;
            }
            set
            {
                monies = value;
            }
        }

        //Width of map (measured in tiles) 6 = 6 tiles wide.
        private int width;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        //Height of map (measured in tiles) 6 = 6 tiles high.
        private int height;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        //Par of number of directional tiles.
        private int par;
        public int Par
        {
            get
            {
                return par;
            }
            set
            {
                par = value;
            }
        }

        //Constructor
        public Level(int _width, int _height, int _gamePieceSize, int _par)
        {
            width = _width;
            height = _height;
            gamePieceSize = _gamePieceSize;
            grid = new GamePiece[_width, _height];
            monies = new List<Money>();
            par = _par;
        }

        //Sets a specific tile in the grid.
        public void SetTile(int x_index, int y_index, GamePiece newGamePiece)
        {
            newGamePiece.PositionRect = new Rectangle(
                x_index * gamePieceSize,
                y_index * gamePieceSize,
                gamePieceSize,
                gamePieceSize);
            grid[x_index, y_index] = newGamePiece;
        }

        //Adds a money to the list of game monies.
        public void AddObject(Money newGamePiece)
        {
            monies.Add(newGamePiece);
        }

        //Draws each object in the grid.
        public void Draw(SpriteBatch obj, int offX, int offY)
        {
            for (int j = 0; j < Height; ++j)
            {
                for (int i = 0; i < Width; ++i)
                {
                    grid[i, j].Draw(obj, offX, offY);
                }
            }
            foreach (GamePiece gamePiece in monies)
            {
                gamePiece.Draw(obj, offX, offY);
            }
        }

        //Activate all monies
        public void ActivateMoney()
        {
            foreach (Money countedMoney in monies)
            {
                countedMoney.Active = true;
            }
        }

        //Returns number of active monies
        public int GetMoneyCount()
        {
            int count = 0;
            foreach(Money countedMoney in monies)
            {
                if (countedMoney.Active)
                    count++;
            }
            return count;
        }

        //Returns a specific piece in the grid.
        public GamePiece GetGamePiece(int x_index, int y_index)
        {
            //Try-Catch incase parameters are beyond the grid.
            try
            {
                return grid[x_index, y_index];
            }
            catch(IndexOutOfRangeException)
            {
                throw;
            }
        }
    }
}
