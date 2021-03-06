﻿using System;
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
    abstract class GamePiece
    {
        //List of textures the object uses.
        public List<Texture2D> listTextures { get; set; } //All GamePiece images

        //Index of the current texture of the object.
        private int indexTexture;
        public int IndexTexture
        {
            get
            {
                return indexTexture;
            }

            set
            {
                indexTexture = value;
            }
        }

        // Rectangle boundary of object.
        private Rectangle positionRect;
        public Rectangle PositionRect
        {
            get
            {
                return positionRect;
            }

            set
            {
                positionRect = value;
            }
        }

        //State of piece with multiple types.
        private PieceState pieceState;
        public PieceState PieceState
        {
            get
            {
                return pieceState;
            }
            set
            {
                pieceState = value;
            }
        }

        //Default of piece with multiple types.
        private PieceState defaultState;
        public PieceState DefaultState
        {
            get
            {
                return defaultState;
            }
            set
            {
                defaultState = value;
            }
        }

        //Initial value for X
        private int initialX;
        public int InitialX
        {
            get
            {
                return initialX;
            }
            set
            {
                initialX = value;
            }
        }
        
        //Initial value for Y
        private int initialY;
        public int InitialY
        {
            get
            {
                return initialY;
            }
            set
            {
                initialY = value;
            }
        }

        //Empty constructor for gear
        public GamePiece()
        {
        }

        // Constructor that establishes textures.
        public GamePiece(int x, int y, List<Texture2D> textures)
        {
            positionRect = new Rectangle(x, y, GlobalVar.TILESIZE, GlobalVar.TILESIZE);

            listTextures = textures;
            indexTexture = 0;

        }

        // Constructor that does not establish textures.
        public GamePiece(int x, int y)
        {
            positionRect = new Rectangle(x, y, GlobalVar.TILESIZE, GlobalVar.TILESIZE);

            listTextures = new List<Texture2D>();
            indexTexture = 0;

            InitialX = x;

            InitialY = y;

        }

        // for items bigger than global tile size
        public GamePiece(int x, int y, int width, int height, List<Texture2D> textures)
        {
            positionRect = new Rectangle(x, y, width, height);

            listTextures = textures;
            indexTexture = 0;

            InitialX = x;
            InitialY = y;
        }

        //Draws any gameobject to screen
        public virtual void Draw(SpriteBatch obj, int offX, int offY)
        {
            Rectangle offsetRect = new Rectangle(
                positionRect.X + offX,
                positionRect.Y + offY,
                positionRect.Width,
                positionRect.Height);
            obj.Draw(this.listTextures[indexTexture], offsetRect, Color.White);
        }

        //Increments type and texture for pieces with multiple types or textures.
        //Returns false by default. Objects that can be incremented like Tile return true.
        public virtual bool IncrementType()
        {
            return false;
        }

        //Decrements type and texture for pieces with multiple types or textures.
        //Returns false by default. Objects that can be incremented like Tile return true.
        public virtual bool DecrementType()
        {
            return false;
        }

        public virtual bool ReturnStartingDirection()
        {
            return false;
        }

        public virtual int GetTileType()
        {
            return 0;
        }

        public virtual void ResetType()
        {

        }

        //Checks for object collision.
        public bool CheckCollision(GamePiece obj)
        {
            if (obj.PositionRect.Intersects(this.PositionRect))
            {
                return true;
            }
            return false;
        }
    }
}
