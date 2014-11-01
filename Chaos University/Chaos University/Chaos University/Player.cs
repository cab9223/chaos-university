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
        public string PlrName { get; set; } //Players name
        
        //public Majors PlrClass { get; set; } sets major, only for actual use if we implement a major system.

        public Player(int x, int y, int direction, List<Texture2D> textures) //Constructor
            : base(x, y, direction, textures)
        {
            //PlrName = name;  //sets player name
            Console.WriteLine(PositionRect.ToString());
        }

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
            Console.WriteLine(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2);
        }
    }
}
