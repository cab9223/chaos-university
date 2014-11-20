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
    class SpecialTile : Tile
    {
        private int tileType;
        public int TileType
        {
            get
            {
                return tileType;
            }

            set
            {
                tileType = value;
            }
        }

        //Constructor.
        public SpecialTile(int x, int y, List<Texture2D> textures, int tileType, int direction) //Constructor
            : base(x, y, textures)
        {
            if (direction == 0)
            {
                PieceState = PieceState.SpecialNorth;
                IndexTexture = direction;
            }
            if (direction == 1)
            {
                PieceState = PieceState.SpecialEast;
                IndexTexture = direction;
            }
            if (direction == 2)
            {
                PieceState = PieceState.SpecialSouth;
                IndexTexture = direction;
            }
            if (direction == 3)
            {
                PieceState = PieceState.SpecialWest;
                IndexTexture = direction;
            }
            TileType = tileType;
        }

        //Increment floor type to a different type.
        public override bool IncrementType()
        {
           
                    if (TileType == 1)
                    {
                        switch (PieceState)
                        {
                            case PieceState.SpecialNorth:
                            PieceState = PieceState.SpecialEast;
                            IndexTexture++;
                            break;
                            case PieceState.SpecialEast:
                            PieceState = PieceState.SpecialSouth;
                            IndexTexture++;
                            break;
                            case PieceState.SpecialSouth:
                            PieceState = PieceState.SpecialWest;
                            IndexTexture++;
                            break;
                            case PieceState.SpecialWest:
                            PieceState = PieceState.SpecialNorth;
                            IndexTexture = 0;
                            break;
                        }
                    return true;
                    }
            return true;
        }
        
        
    }
}
