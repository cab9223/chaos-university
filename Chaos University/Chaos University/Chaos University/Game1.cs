using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Chaos_University
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState mouse;           //Current mouse state
        MouseState mousePrev;       //Previous mouse state
        Tile[,] tileGrid;           //Grid of tiles
        int gamePieceSize;          //Height of width of a game piece

        public enum GameState
        {
            TitleScreen,
            Menus,
            PlacingTiles,
            Playing,
            GameOver
        }

        //Base game methods below...
        Player playerChar;
        GameState current;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            current = GameState.TitleScreen;



            base.Initialize();
        }

        // will load a level based on level int passes into
        private void LoadLevel(int newLevel)
        {
            //concatanates "LevelMap" and passed int to find map file.
            string levelNumber = newLevel.ToString();
            string file = "LevelMap" + levelNumber + ".txt";
            
            //makes new stream reader
            StreamReader input = null;
            
                //mapfiles will be one line long to avoid dealing with newlines. Also, assuming mapfile resides in bin.
                input = new StreamReader(file);
                string line = "";
                line = input.ReadLine();


                //makes an array based of split on [space]. format is (classType,Xcord,Ycord)
                string[] fullMap = line.Split(' ');

                //gets total number of objects from mapfile
                int mapSize = fullMap.Length;

                int loopCount = 0;

                //will loop through all objects in map file and create new class objects based on fullMap[] string
                while (loopCount != mapSize)
                {
                    //classCord now holds info as such: [0] class name [1] Xcord [2] Ycord | Possible [3] for direction
                    string[] classCord = fullMap[loopCount].Split(',');

                    string className = classCord[0];

                    //if array has a 4th element for direction, will do special things. Not fully useable till .pngs are made
                    /* int checkDir = classCord.Length;

                     if (checkDir == 4)
                     {
                         if (className == "player")
                         {
                             int TempX = Int32.Parse(classCord[1]);
                             int TempY = Int32.Parse(classCord[2]);
                             int Direction = Int32.Parse(classCord[3]);

                             Player player = new Player(TempX, TempY, Direction, name, something.png);
                         }
                      
                         if (calssName == "guard")
                         {
                             int TempX = Int32.Parse(classCord[1]);
                             int TempY = Int32.Parse(classCord[2]);
                             int Direction = Int32.Parse(classCord[3]);
                      
                             Enemy guard = new Enemy(TempX, TempY, Direction, name, something.png);
                     }
                     
                     */

                    //creates new class objects depending on classname at classCord[0]


                    if (className == "wall")
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);

                        Wall wall = new Wall(TempX, TempY);
                        // will most likly add this to a list or something like that

                    }

                    if (className == "money") //Money now takes 3 arguments
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);
                        int amount = Int32.Parse(classCord[3]);

                        Money money = new Money(TempX, TempY, amount); //This now takes an amount int, if any adjustments need to be made
                    }

                    if (className == "goal")
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);

                        Goal goal = new Goal(TempX, TempY);

                    }

                    if (className == "trap") //Trap now takes 3 arguments
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);
                        string type = classCord[3];

                        Traps trap = new Traps(TempX, TempY, type);  //This now takes trap type string, if you need to make any adjustments
                    }

                    loopCount++;
                }
            //closes reader
                input.Close();       
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            mouse = Mouse.GetState();

            if(mouse.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
            {
                for (int j = 0; j < tileGrid.GetLength(0); ++j)
                {
                    for (int i = 0; i < tileGrid.GetLength(1); ++i)
                    {
                        //Place or turn direction tile at gameGrid[i / gamePieceSize, j / gamePieceSize]
                    }
                }
            }

            // Collision detection, for playing the game.
            if (current == GameState.Playing)
            {
                // We probably want this to hold for a second then move the player by GlobalVar.TileSize pixels.
                // Holding for a second so that it's actually visible what happens- and it's not like we have a reason to
                // code this thing to take real-time input.
                for (int i = 0; i < (GlobalVar.GAMEWIDTH / GlobalVar.TILESIZE); i++)
                {
                    for (int c = 0; c < (GlobalVar.GAMEHEIGHT / GlobalVar.TILESIZE); c++)
                    {
                        if (playerChar.CheckPosition(tileGrid[i, c]))
                        {                           
                        }
                    }
                }
            }

            mousePrev = Mouse.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (int j = 0; j < tileGrid.GetLength(0); ++j)
            {
                for (int i = 0; i < tileGrid.GetLength(1); ++i)
                {
                    //Draw at X = gamePieceSize * i, Y = gamePieceSize * j
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
