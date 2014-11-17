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

        //LEVEL PLAYERS
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


        //LEVEL PLAYERS START POSITION
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

        //LEVEL PLAYERS START ROTATION
        private int rotNinja;               //Initial direction the Ninja faces.
        public int RotNinja
        {
            get { return rotNinja; }
            set { rotNinja = value; }
        }

        private int rotRecon;               //Initial direction the Recon faces.
        public int RotRecon
        {
            get { return rotRecon; }
            set { rotRecon = value; }
        }

        private int rotAssault;               //Initial direction the Assault faces.
        public int RotAssault
        {
            get { return rotAssault; }
            set { rotAssault = value; }
        }

        //LEVEL PLAYERS EXISITENCE BOOLEAN.
        private bool isNinja;
        public bool IsNinja
        {
            get { return isNinja; }
            set { isNinja = value; }
        }

        private bool isRecon;
        public bool IsRecon
        {
            get { return isRecon; }
            set { isRecon = value; }
        }

        private bool isAssault;
        public bool IsAssault
        {
            get { return isAssault; }
            set { isAssault = value; }
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

        public int CheckCollisions()
        {
            //Condition:    0 = do nothing.
            //              1 = Succeed.
            //              2 = Fail.
            int condition = 0;
            for (int j = 0; j < this.Height; ++j)
            {
                for (int i = 0; i < this.Width; ++i)
                {
                    //If Ninja on direction tile.
                    if (this.IsNinja)
                    {
                        if (this.GetGamePiece(i, j).PositionRect.Center == this.Ninja.PositionRect.Center)
                        {
                            //Turn player based on tile direction.
                            switch (this.GetGamePiece(i, j).PieceState)
                            {
                                case PieceState.Floor:
                                    break;
                                case PieceState.North:
                                    this.Ninja.turn(0);
                                    break;
                                case PieceState.East:
                                    this.Ninja.turn(1);
                                    break;
                                case PieceState.South:
                                    this.Ninja.turn(2);
                                    break;
                                case PieceState.West:
                                    this.Ninja.turn(3);
                                    break;
                                case PieceState.Goal:
                                    condition = 1;
                                    break;
                            }
                        }
                    }

                    //If Recon on direction tile.
                    if (this.IsRecon)
                    {
                        if (this.GetGamePiece(i, j).PositionRect.Center == this.Recon.PositionRect.Center)
                        {
                            //Turn player based on tile direction.
                            switch (this.GetGamePiece(i, j).PieceState)
                            {
                                case PieceState.Floor:
                                    break;
                                case PieceState.North:
                                    this.Recon.turn(0);
                                    break;
                                case PieceState.East:
                                    this.Recon.turn(1);
                                    break;
                                case PieceState.South:
                                    this.Recon.turn(2);
                                    break;
                                case PieceState.West:
                                    this.Recon.turn(3);
                                    break;
                                case PieceState.Goal:
                                    condition = 1;
                                    break;
                            }
                        }
                    }

                    //If Assault on direction tile.
                    if (this.IsAssault)
                    {
                        if (this.GetGamePiece(i, j).PositionRect.Center == this.Assault.PositionRect.Center)
                        {
                            //Turn player based on tile direction.
                            switch (this.GetGamePiece(i, j).PieceState)
                            {
                                case PieceState.Floor:
                                    break;
                                case PieceState.North:
                                    this.Assault.turn(0);
                                    break;
                                case PieceState.East:
                                    this.Assault.turn(1);
                                    break;
                                case PieceState.South:
                                    this.Assault.turn(2);
                                    break;
                                case PieceState.West:
                                    this.Assault.turn(3);
                                    break;
                                case PieceState.Goal:
                                    condition = 1;
                                    break;
                            }
                        }
                    }

                    //If game piece is a wall.
                    if (this.GetGamePiece(i, j).PieceState == PieceState.Wall)
                    {
                        //Check if player collided with it.
                        if (this.GetGamePiece(i, j).CheckCollision(this.Ninja))
                        {
                            condition = 2;
                        }
                    }
                }

                //For all Game Pieces in level object list, check for collision.
                foreach (Money gamePiece in this.Monies)
                {
                    if (gamePiece.CheckCollision(this.Ninja))
                    {
                        gamePiece.Active = false;
                    }
                }
            }

            return condition;
        }
    }
}
