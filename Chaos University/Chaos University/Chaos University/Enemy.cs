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
    class Enemy : MoveableGamePiece
    {
        public int Difficulty { get; set; }

        public bool Detected { get; set; }

        public bool Confused { get; set; }

        public bool Taunted { get; set; }

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

            Moving = true;

            Detected = false;

            Confused = false;

            Taunted = false;

            InitialX = x;

            InitialY = y;

            InitialDirection = dir;

            if (Direction == 0 || Direction == 2 || Direction == 4 || Direction == 6)
            {
                DetectRect = new Rectangle(x + 20, y + 20, (GlobalVar.TILESIZE / 5), (GlobalVar.TILESIZE / 2));
                EmotionRect = new Rectangle(x + 13, y - 11, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 2));
            }
            else if (Direction == 1 || Direction == 3 || Direction == 5 || Direction == 7)
            {
                DetectRect = new Rectangle(x + 4, y + 20, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 5));
                EmotionRect = new Rectangle(x + 11, y - 13, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 2));
            }

            Turn(Direction);
        }


        public Enemy(int x, int y, int dir, List<Texture2D> textures, int diff, int initDir, int initX, int initY) //Constructor 2
            : base(x, y, dir, textures)
        {
            Difficulty = diff;

            isAlive = true;

            Moving = true;

            Detected = false;

            Confused = false;

            InitialX = initX;

            InitialY = initY;

            InitialDirection = initDir;

            if (Direction == 0 || Direction == 2 || Direction == 4 || Direction == 6)
            {
                DetectRect = new Rectangle(x + 20, y + 20, (GlobalVar.TILESIZE / 5), (GlobalVar.TILESIZE / 2));
                EmotionRect = new Rectangle(x + 13, y - 11, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 2));
            }
            else if (Direction == 1 || Direction == 3 || Direction == 5 || Direction == 7)
            {
                DetectRect = new Rectangle(x + 4, y + 20, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 5));
                //DetectRect = new Rectangle(x + 12, y + 20, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 5));
                EmotionRect = new Rectangle(x + 11, y - 13, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 2));
            }

            Turn(Direction);
        }


        public bool Attack(Player obj) //Method for attacking player
        {
            if (isAlive == true)
            {
                if (obj.PositionRect.Intersects(this.DetectRect) == true)
                {
                    Detected = true;
                    Moving = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        public void Reset()
        {
            isAlive = true;

            Moving = true;

            Detected = false;

            Confused = false;

            Taunted = false;

            Direction = InitialDirection;

            PositionRect = new Rectangle(InitialX, InitialY, GlobalVar.TILESIZE, GlobalVar.TILESIZE);

            if (Direction == 0 || Direction == 2 || Direction == 4 || Direction == 6)
            {
                DetectRect = new Rectangle(InitialX + 20, InitialY + 20, (GlobalVar.TILESIZE / 5), (GlobalVar.TILESIZE / 2));
                EmotionRect = new Rectangle(InitialX + 13, InitialY - 11, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 2));
            }
            else if (Direction == 1 || Direction == 3 || Direction == 5 || Direction == 7)
            {
                DetectRect = new Rectangle(InitialX + 4, InitialY + 20, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 5));
                EmotionRect = new Rectangle(InitialX + 11, InitialY - 13, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 2));
            }

            Turn(Direction);
        }


        public void Patrol(int type) //Method to move enemy around map
        {
            //if (challenge < 3 && challenge > -1)
            //{
            //    Difficulty = challenge;
            //}

            switch (type)
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
                    //Moves back and forth, at walls, can hit traps

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
                            DetectRect = new Rectangle(DetectRect.X, DetectRect.Y, (GlobalVar.TILESIZE / 5), (GlobalVar.TILESIZE / 2));
                            //Direction = 0;
                            break;

                        case 3:
                            Turn(1);
                            DetectRect = new Rectangle(DetectRect.X, DetectRect.Y, (GlobalVar.TILESIZE / 2), (GlobalVar.TILESIZE / 5));
                            //Direction = 1;
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
                switch (Difficulty)
                {
                    case 0:
                        obj.Draw(listTextures[0],
                            new Rectangle((PositionRect.X + PositionRect.Width / 2) + offX, (PositionRect.Y + PositionRect.Height / 2) + offY, PositionRect.Width, PositionRect.Height),
                            null,
                            Color.White,
                            (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                            new Vector2(listTextures[0].Width / 2, listTextures[0].Height / 2),
                            SpriteEffects.None,
                            0.0f);
                        break;

                    case 1:
                        obj.Draw(listTextures[1],
                            new Rectangle((PositionRect.X + PositionRect.Width / 2) + offX, (PositionRect.Y + PositionRect.Height / 2) + offY, PositionRect.Width, PositionRect.Height),
                            null,
                            Color.White,
                            (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
                            new Vector2(listTextures[0].Width / 2, listTextures[0].Height / 2),
                            SpriteEffects.None,
                            0.0f);
                        break;
                }

            }

            //For checking Detection -- Testing purposes only
            //obj.Draw(listTextures[0],
            //        new Rectangle((DetectRect.X + DetectRect.Width / 2) + offX, (DetectRect.Y + DetectRect.Height / 2) + offY, DetectRect.Width, DetectRect.Height),
            //        null,
            //        Color.White,
            //        (float)(Math.Atan2(Vector.Y, Vector.X) + Math.PI / 2),
            //        new Vector2(listTextures[0].Width / 2, listTextures[0].Height / 2),
            //        SpriteEffects.None,
            //        0.0f);

            if (this.Confused == true) //For Emotion -- ?
            {
                obj.Draw(listTextures[3],
                    new Rectangle((EmotionRect.X + EmotionRect.Width / 2) + offX, (EmotionRect.Y + EmotionRect.Height / 2) + offY, EmotionRect.Width, EmotionRect.Height),
                    null,
                    Color.White,
                    (float)(Math.Atan2(-1, 0) + Math.PI / 2),
                    new Vector2(listTextures[2].Width / 2, listTextures[2].Height / 2),
                    SpriteEffects.None,
                    0.0f);
            }

            if (this.Detected == true) //For Emotion -- !
            {
                Confused = false;

                obj.Draw(listTextures[2],
                    new Rectangle((EmotionRect.X + EmotionRect.Width / 2) + offX, (EmotionRect.Y + EmotionRect.Height / 2) + offY, EmotionRect.Width, EmotionRect.Height),
                    null,
                    Color.White,
                    (float)(Math.Atan2(-1, 0) + Math.PI / 2),
                    new Vector2(listTextures[2].Width / 2, listTextures[2].Height / 2),
                    SpriteEffects.None,
                    0.0f);
            }


        }


        public override void Move(int distance)
        {
            if (Moving == true)
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
                    case 4: //Stopped equivilent to Direction 0.
                        Stop();
                        Turn(0);
                        break;
                    case 5: //Stopped equivilent to Direction 1.
                        Stop();
                        Turn(1);
                        break;
                    case 6: //Stopped equivilent to Direction 2.
                        Stop();
                        Turn(2);
                        break;
                    case 7: //Stopped equivilent to Direction 3.
                        Stop();
                        Turn(3);
                        break;
                }
                PositionRect = temp;
                DetectRect = temp2;
                EmotionRect = temp3;
            }
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


        public void Stop() //Stop the enemy
        {
            Moving = false;
        }


        public void StartMove() //Start enemy movement after a stop
        {
            switch (Direction)
            {
                case 4:
                    Direction = 0;
                    break;
                case 5:
                    Direction = 1;
                    break;
                case 6:
                    Direction = 2;
                    break;
                case 7:
                    Direction = 3;
                    break;
            }   
            Moving = true;
        }


        public void Dead() //removes guard
        {
            Moving = false;
            isAlive = false;
        }
    }
}
