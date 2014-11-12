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
        public enum Major
        {
            Ninja,
            Assault,
            Recon
        }

        public Major ThisMajor { get; set; }

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
        public Player(int x, int y, int direction, List<Texture2D> textures, Major myMajor)
            : base(x, y, direction, textures)
        {
            //PlrName = name;  //sets player name
            Console.WriteLine(PositionRect.ToString());
            moving = false;
            ThisMajor = myMajor;
        }

        //Draws all player components.
        public override void Draw(SpriteBatch obj) //Draws player using base draw method
        {
            //Draws all parts of the character on top of each other.
            obj.Draw(listTextures[0],
                new Rectangle(PositionRect.X + PositionRect.Width / 2, PositionRect.Y + PositionRect.Height / 2, PositionRect.Width, PositionRect.Height),
                null,
                Color.FromNonPremultiplied(GlobalVar.ColorsSplit[0],GlobalVar.ColorsSplit[1],GlobalVar.ColorsSplit[2], 255),
                (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                new Vector2(listTextures[0].Width / 2, listTextures[0].Height / 2),
                SpriteEffects.None,
                0.0f);

            if (ThisMajor == Major.Assault)
            {
                obj.Draw(listTextures[1],
                    new Rectangle(PositionRect.X + PositionRect.Width / 2, PositionRect.Y + PositionRect.Height / 2, PositionRect.Width, PositionRect.Height),
                    null,
                    Color.FromNonPremultiplied(GlobalVar.ColorsSplit[6], GlobalVar.ColorsSplit[7], GlobalVar.ColorsSplit[8], 255),
                    (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                    new Vector2(listTextures[1].Width / 2, listTextures[1].Height / 2),
                    SpriteEffects.None,
                    0.0f);
            }

            if (ThisMajor == Major.Recon)
            {
                obj.Draw(listTextures[2],
                    new Rectangle(PositionRect.X + PositionRect.Width / 2, PositionRect.Y + PositionRect.Height / 2, PositionRect.Width, PositionRect.Height),
                    null,
                    Color.FromNonPremultiplied(GlobalVar.ColorsSplit[6], GlobalVar.ColorsSplit[7], GlobalVar.ColorsSplit[8], 255),
                    (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                    new Vector2(listTextures[2].Width / 2, listTextures[2].Height / 2),
                    SpriteEffects.None,
                    0.0f);
            }

            obj.Draw(listTextures[3],
                new Rectangle(PositionRect.X + PositionRect.Width / 2, PositionRect.Y + PositionRect.Height / 2, PositionRect.Width, PositionRect.Height),
                null,
                    Color.FromNonPremultiplied(GlobalVar.ColorsSplit[3], GlobalVar.ColorsSplit[4], GlobalVar.ColorsSplit[5], 255),
                (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                new Vector2(listTextures[3].Width / 2, listTextures[3].Height / 2),
                SpriteEffects.None,
                0.0f);

            if (ThisMajor == Major.Ninja)
            {
                obj.Draw(listTextures[4],
                    new Rectangle(PositionRect.X + PositionRect.Width / 2, PositionRect.Y + PositionRect.Height / 2, PositionRect.Width, PositionRect.Height),
                    null,
                    Color.FromNonPremultiplied(GlobalVar.ColorsSplit[6], GlobalVar.ColorsSplit[7], GlobalVar.ColorsSplit[8], 255),
                    (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                    new Vector2(listTextures[4].Width / 2, listTextures[4].Height / 2),
                    SpriteEffects.None,
                    0.0f);
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
            switch (newDirection)
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