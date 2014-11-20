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

        private int ogDirection;
        public int OGDirection
        {
            get
            {
                return ogDirection;
            }

            set
            {
                ogDirection = value;
            }
        }

        private int Direction;
        public int direction
        {
            get
            {
                return Direction;
            }

            set
            {
                Direction = value;
            }
        }
        //Constructor.
        public SpecialTile(int x, int y, List<Texture2D> textures, int tileType, int dir) //Constructor
            : base(x, y, textures)
        {
            direction = dir;
            TileType = tileType;

            if (TileType == 1)
            {
                if (direction == 0)
                {
                    PieceState = PieceState.SpecialNorth;
                    IndexTexture = direction;
                    OGDirection = direction;
                }
                if (direction == 1)
                {
                    PieceState = PieceState.SpecialEast;
                    IndexTexture = direction;
                    OGDirection = direction;
                }
                if (direction == 2)
                {
                    PieceState = PieceState.SpecialSouth;
                    IndexTexture = direction;
                    OGDirection = direction;
                }
                if (direction == 3)
                {
                    PieceState = PieceState.SpecialWest;
                    IndexTexture = direction;
                    OGDirection = direction;
                }
            }
            if (TileType == 2)
            {
                if (direction == 0)
                {
                    PieceState = PieceState.SpecialNorth;
                    IndexTexture = direction + 4;
                    OGDirection = direction;
                }
                if (direction == 1)
                {
                    PieceState = PieceState.SpecialWest;
                    IndexTexture = direction + 4;
                    OGDirection = direction;
                }
                if (direction == 2)
                {
                    PieceState = PieceState.SpecialSouth;
                    IndexTexture = direction + 4;
                    OGDirection = direction;
                }
                if (direction == 3)
                {
                    PieceState = PieceState.SpecialEast;
                    IndexTexture = direction + 4;
                    OGDirection = direction;
                }
            }
            
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
                    if (TileType == 2)
                    {
                        switch (PieceState)
                        {
                            case PieceState.SpecialNorth:
                                PieceState = PieceState.SpecialWest;
                                IndexTexture++;
                                break;
                            case PieceState.SpecialWest:
                                PieceState = PieceState.SpecialSouth;
                                IndexTexture++;
                                break;
                            case PieceState.SpecialSouth:
                                PieceState = PieceState.SpecialEast;
                                IndexTexture++;
                                break;
                            case PieceState.SpecialEast:
                                PieceState = PieceState.SpecialNorth;
                                IndexTexture = 4;
                                break;
                        }
                        return true;
                    }
                    return true;
        }


        public override int GetTileType()
        {
            return TileType;
        }
        //sets special tiles back to ogrinal direction
        public override bool ReturnStartingDirection()
        {
            direction = OGDirection;
            IndexTexture = OGDirection;
            
            if (direction == 0)
            {
                PieceState = PieceState.SpecialNorth;
                if (TileType == 2 && direction == 0)
                {
                    IndexTexture = 4;
                }
            }
           
            if (direction == 1)
            {
                PieceState = PieceState.SpecialEast;
               
            }
            if (TileType == 2 && direction == 1)
            {
                PieceState = PieceState.SpecialWest;
                IndexTexture = 5;
            }
            if (direction == 2)
            {
                PieceState = PieceState.SpecialSouth;
                if (TileType == 2 && direction == 2)
                {
                    IndexTexture = 6;
                }
                
            }
            if (direction == 3)
            {
                PieceState = PieceState.SpecialWest;
               
            }
            if (TileType == 2 && direction == 3)
            {
                PieceState = PieceState.SpecialEast;
                IndexTexture = 7;
            }
            return true;
        }
        
        
    }
}
