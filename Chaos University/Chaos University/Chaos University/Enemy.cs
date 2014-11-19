﻿using System;
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
    class Enemy : MoveableGamePiece
    {
        public int Difficulty { get; set; }

        private bool isAlive; //For guard death
        public bool IsAlive//Read only
        {
            get
            {
                return isAlive;
            }
        }

        private Rectangle detectRect;
        public Rectangle DetectRect
        {
            get
            {
                return detectRect;
            }

            set
            {
                detectRect = value;
            }
        }

        private Rectangle emotionRect;
        public Rectangle EmotionRect
        {
            get
            {
                return emotionRect;
            }

            set
            {
                emotionRect = value;
            }
        }


        public Enemy(int x, int y, int dir, List<Texture2D> textures) //Constructor
            : base(x, y, dir, textures)
        {
            Difficulty = 0;

            isAlive = true;

            EmotionRect = new Rectangle(x + 10, y - 13, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 2));

            if (dir == 0 || dir == 2)
            {
                DetectRect = new Rectangle(x + 20, y + 20, (GlobalVar.TILESIZE / 5), (GlobalVar.TILESIZE / 2));
            }
            else if (dir == 1 || dir == 3)
            {
                DetectRect = new Rectangle(x + 20, y + 20, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 5));
            }
        }


        public bool Attack(Player obj) //Method for attacking player
        {
            if (obj.PositionRect.Intersects(this.DetectRect) == true)
            {
                //Maybe add an effect or sound to say the guard caught the player
                return true;
            }
            else
            {
                return false;
            }
        }


        public void Patrol(int challenge) //Method to move enemy around map
        {
            if (challenge < 3 && challenge > -1)
            {
                Difficulty = challenge;
            }

            switch (Difficulty)
            {
                case 0:  //Simple back and forth partol, stops at cirtain distance or wall

                    switch (Direction)
                    {
                        case 0:
                            Turn(2);
                            DetectRect = new Rectangle(DetectRect.X, DetectRect.Y - 18, (GlobalVar.TILESIZE / 5), (GlobalVar.TILESIZE / 2));
                            //Direction = 2;
                            break;

                        case 1:
                            Turn(3);
                            DetectRect = new Rectangle(DetectRect.X + 18, DetectRect.Y, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 5));
                            //Direction = 3;
                            break;

                        case 2:
                            Turn(0);
                            DetectRect = new Rectangle(DetectRect.X, DetectRect.Y + 18, (GlobalVar.TILESIZE / 5), (GlobalVar.TILESIZE / 2));
                            //Direction = 0;
                            break;

                        case 3:
                            Turn(1);
                            DetectRect = new Rectangle(DetectRect.X - 18, DetectRect.Y, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 5));
                            //Direction = 1;
                            break;
                    } 
                    break;
                case 1:
                    //Moves in any direction based on an algorithm, stops at walls, can hit traps

                    switch (Direction)
                    {
                        case 0:
                            Direction = 2;
                            break;

                        case 1:
                            Direction = 3;
                            break;

                        case 2:
                            Direction = 0;
                            break;

                        case 3:
                            Direction = 1;
                            break;
                    }
                    break;
                case 2:
                    //Moves in any direction based on an algorithm, stops at walls, avoids traps, maybe more
                    break;
            }
        }


        public override void Draw(SpriteBatch obj, int offX, int offY) //Draws Enemy
        {
            if (this.IsAlive == true)
            {
                obj.Draw(listTextures[0],
                    new Rectangle((PositionRect.X + PositionRect.Width / 2) + offX, (PositionRect.Y + PositionRect.Height / 2) + offY, PositionRect.Width, PositionRect.Height),
                    null,
                    Color.White,
                    (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                    new Vector2(listTextures[0].Width / 2, listTextures[0].Height / 2),
                    SpriteEffects.None,
                    0.0f);
            }

            //For checking Detection
            //obj.Draw(listTextures[0],
            //        new Rectangle(DetectRect.X + DetectRect.Width / 2, DetectRect.Y + DetectRect.Height / 2, DetectRect.Width, DetectRect.Height),
            //        null,
            //        Color.White,
            //        (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
            //        new Vector2(listTextures[0].Width / 2, listTextures[0].Height / 2),
            //        SpriteEffects.None,
            //        0.0f);

            //For checking Emotion
            //obj.Draw(listTextures[1],
            //        new Rectangle((EmotionRect.X + EmotionRect.Width / 2) + offX, (EmotionRect.Y + EmotionRect.Height / 2) + offY, EmotionRect.Width, EmotionRect.Height),
            //        null,
            //        Color.White,
            //        (float)(Math.Atan2(-1, 0) + Math.PI / 2),
            //        new Vector2(listTextures[1].Width / 2, listTextures[1].Height / 2),
            //        SpriteEffects.None,
            //        0.0f);
        }


        public override void Move(int distance)
        {
            Rectangle temp = PositionRect;
            Rectangle temp2 = DetectRect;
            Rectangle temp3 = EmotionRect;

            switch (Direction)
            {
                case 0:
                    temp.Y += distance;
                    temp2.Y += distance;
                    temp3.Y += distance;
                    break;

                case 1:
                    temp.X -= distance;
                    temp2.X -= distance;
                    temp3.X -= distance;
                    break;

                case 2:
                    temp.Y -= distance;
                    temp2.Y -= distance;
                    temp3.Y -= distance;
                    break;

                case 3:
                    temp.X += distance;
                    temp2.X += distance;
                    temp3.X += distance;
                    break;
            }
            PositionRect = temp;
            DetectRect = temp2;
            EmotionRect = temp3;
        }


        //Turns Enemy to face a new direction based on standard direction numbers.
        public void Turn(int newDirection)
        {
            switch (newDirection)
            {
                case 0:
                    Vector = new Vector2(0, 1);
                    Direction = 0;
                    break;
                case 1:
                    Vector = new Vector2(-1, 0);
                    Direction = 1;
                    break;
                case 2:
                    Vector = new Vector2(0, -1);
                    Direction = 2;

                    break;
                case 3:
                    Vector = new Vector2(1, 0);
                    Direction = 3;
                    break;
            }
        }

        public void Dead() //removes guard
        {
            isAlive = false;
        }
    }
}
