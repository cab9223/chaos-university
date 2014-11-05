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
using System.IO;

namespace Chaos_University
{
    // Stuff here is pretty self-explanitory, the imageFile thing is for future implementation of image handling stuff.
    class Player : MoveableGamePiece
    {
        private int tries;
        public int Tries
        {
            get
            {
                return tries;
            }
            set
            {
                tries = value;
            }
        }

        private int parCount;
        public int ParCount
        {
            get
            {
                return parCount;
            }
            set
            {
                parCount = value;
            }
        }

        private bool moving;
        public bool Moving
        {
            get
            {
                return moving;
            }
            set
            {
                moving = value;
            }
        }

        //Constructor
        public Player(int x, int y, int direction, List<Texture2D> textures)
            : base(x, y, direction, textures)
        {
            //PlrName = name;  //sets player name
            Console.WriteLine(PositionRect.ToString());
            moving = false;
            tries = 3;
            parCount = 0;
        }

        //Draws all player components.
        public override void Draw(SpriteBatch obj) //Draws player using base draw method
        {             
            StreamReader reader = new StreamReader(TitleContainer.OpenStream("Color.txt"));
            int readColor = Int32.Parse(reader.ReadLine());
            //Draws all parts of the character on top of each other.
            switch(readColor)
            {
                default:
                    //Draws Everything.
                    foreach (Texture2D texture in listTextures)
                    {
                        obj.Draw(
                            texture,
                            new Rectangle(
                                PositionRect.X + PositionRect.Width / 2,
                                PositionRect.Y + PositionRect.Height / 2,
                                PositionRect.Width,
                                PositionRect.Height),
                            null,
                            Color.White,
                            (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                            new Vector2(texture.Width / 2, texture.Height / 2),
                            SpriteEffects.None,
                            0.0f);
                    }
                    break;
            }
        }

        //Move Player
        public override void Move(int distance)
        {
            if (moving)
            {
                switch (Direction)
                {
                    case 0:
                        PositionRect = new Rectangle(
                            PositionRect.X,
                            PositionRect.Y - distance,
                            PositionRect.Width,
                            PositionRect.Height);
                        break;

                    case 1:
                        PositionRect = new Rectangle(
                            PositionRect.X + distance,
                            PositionRect.Y,
                            PositionRect.Width,
                            PositionRect.Height);
                        break;

                    case 2:
                        PositionRect = new Rectangle(
                            PositionRect.X,
                            PositionRect.Y + distance,
                            PositionRect.Width,
                            PositionRect.Height);
                        break;

                    case 3:
                        PositionRect = new Rectangle(
                            PositionRect.X - distance,
                            PositionRect.Y,
                            PositionRect.Width,
                            PositionRect.Height);
                        break;
                }
            }
        }

        //Turns Player to face a new direction based on standard direction numbers.
        public void turn(int newDirection)
        {
            switch(newDirection)
            {
                case 0:
                    Vector = new Vector2(0, -1);
                    Direction = 0;
                    break;
                case 1:
                    Vector = new Vector2(1, 0);
                    Direction = 1;
                    break;
                case 2:
                    Vector = new Vector2(0, 1);
                    Direction = 2;
                    break;
                case 3:
                    Vector = new Vector2(-1, 0);
                    Direction = 3;
                    break;
            }
        }
    }
}
