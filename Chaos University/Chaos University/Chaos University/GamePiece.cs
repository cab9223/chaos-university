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
        }

        //Draws any gameobject to screen
        public virtual void Draw(SpriteBatch obj)
        {
            obj.Draw(this.listTextures[indexTexture], positionRect, Color.White);
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
