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

    // Floor tiles.
    class Tile : GamePiece
    {
        //Constructor.
        public Tile(int x, int y, List<Texture2D> textures)
            : base(x, y, textures)
        {
            PieceState = PieceState.Floor;
            DefaultState = PieceState.Floor;
        }

        //Constructor with prestablished direction.
        public Tile(int x, int y, List<Texture2D> textures, PieceState _pieceState)
            : base(x, y, textures)
        {
            PieceState = _pieceState;
            DefaultState = _pieceState;
            switch (PieceState)
            {
                case PieceState.Floor:
                    IndexTexture = 0;
                    break;
                case PieceState.North:
                    IndexTexture = 1;
                    break;
                case PieceState.East:
                    IndexTexture = 2;
                    break;
                case PieceState.South:
                    IndexTexture = 3;
                    break;
                case PieceState.West:
                    IndexTexture = 4;
                    break;
            }
        }

        //Increment floor type to a different type.
        public override bool IncrementType()
        {
            switch(PieceState)
            {
                case PieceState.Floor:
                    PieceState = PieceState.North;
                    IndexTexture++;
                    break;
                case PieceState.North:
                    PieceState = PieceState.East;
                    IndexTexture++;
                    break;
                case PieceState.East:
                    PieceState = PieceState.South;
                    IndexTexture++;
                    break;
                case PieceState.South:
                    PieceState = PieceState.West;
                    IndexTexture++;
                    break;
                case PieceState.West:
                    PieceState = PieceState.Floor;
                    IndexTexture = 0;
                    break;
            }
            return true;
        }

        //Decrement floor type to a different type.
        public override bool DecrementType()
        {
            switch (PieceState)
            {
                case PieceState.Floor:
                    PieceState = PieceState.West;
                    IndexTexture = 4;
                    break;
                case PieceState.North:
                    PieceState = PieceState.Floor;
                    IndexTexture = 0;
                    break;
                case PieceState.East:
                    PieceState = PieceState.North;
                    IndexTexture--;
                    break;
                case PieceState.South:
                    PieceState = PieceState.East;
                    IndexTexture--;
                    break;
                case PieceState.West:
                    PieceState = PieceState.South;
                    IndexTexture --;
                    break;
            }
            return true;
        }

        //Resets tile
        public override void ResetType()
        {
            PieceState = DefaultState;
            switch (DefaultState)
            {
                case PieceState.Floor:
                    IndexTexture = 0;
                    break;
                case PieceState.North:
                    IndexTexture = 1;
                    break;
                case PieceState.East:
                    IndexTexture = 2;
                    break;
                case PieceState.South:
                    IndexTexture = 3;
                    break;
                case PieceState.West:
                    IndexTexture = 4;
                    break;
            }
        }
    }
}
