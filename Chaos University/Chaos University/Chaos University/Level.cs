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
        GamePiece[,] grid;              //Grid of game pieces
        private int gamePieceSize;      //Height of width of a game piece
        private int width;              //Width of map (measured in tiles) 6 = 6 tiles wide.
        public int Width { get { return width; } set { width = value; } }
        private int height;             //Height of map (measured in tiles) 6 = 6 tiles high.
        public int Height { get { return height; } set { height = value; } }

        public Level(int _width, int _height, int _gamePieceSize)
        {
            width = _width;
            height = _height;
            gamePieceSize = _gamePieceSize;
            grid = new GamePiece[_height, _width];

            for (int i = 0; i < _width; i++)
            {
                for (int c = 0; c < _height; c++)
                {
                    grid[i, c] = new Tile(i * GlobalVar.TILESIZE, c * GlobalVar.TILESIZE);
                }
            }
        }

        public void SetTile(int y_index, int x_index, GamePiece newGamePiece)
        {
            newGamePiece.PositionRect = new Rectangle(
                x_index * gamePieceSize,
                y_index * gamePieceSize,
                gamePieceSize,
                gamePieceSize);
            grid[y_index, x_index] = newGamePiece;
        }

        public void Draw(SpriteBatch obj)
        {
            for (int j = 0; j < grid.GetLength(0); ++j)
            {
                for (int i = 0; i < grid.GetLength(1); ++i)
                {
                    grid[j, i].Draw(obj);
                }
            }
        }

        public GamePiece GetGamePiece(int y_index, int x_index)
        {
            return grid[x_index, y_index];
        }
    }
}
