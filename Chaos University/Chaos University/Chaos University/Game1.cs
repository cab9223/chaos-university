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
        KeyboardState keyboard;     //Keyboard state
        KeyboardState keyboardPrev; //Keyboard state previous

        SpriteFont menuFont;
        SpriteFont headerFont;

        Level level;

        public enum GameState
        {
            Title,
            Menus,
            PlacingTiles,
            Playing,
            GameOver
        }

        //Textures
        Texture2D gridNorth;
        Texture2D gridEast;
        Texture2D gridSouth;
        Texture2D gridWest;
        Texture2D gridWall;
        Texture2D gridFloor;

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
            current = GameState.Title;
            this.IsMouseVisible = true;

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

                string[] indexs = line.Split(' ');

                int tempWidth = Int32.Parse(indexs[0]);
                int tempHeight = Int32.Parse(indexs[1]);

                level = new Level(tempWidth, tempHeight, GlobalVar.TILESIZE);

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

                    if (className == "player")
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);
                        int Direction = Int32.Parse(classCord[3]);

                        Player playerChar = new Player(TempX, TempY, Direction);
                    }

                    if (className == "wall")
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);

                        level.SetTile(TempX, TempY, new Wall(TempX * GlobalVar.TILESIZE, TempY * GlobalVar.TILESIZE));
                        // will most likly add this to a list or something like that

                    }

                    if (className == "money") //Money now takes 3 arguments
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);
                        //int amount = Int32.Parse(classCord[3]);

                        //level.SetTile(TempX, TempY, new Money(TempX * GlobalVar.TILESIZE, TempY * GlobalVar.TILESIZE, amount));
                        //This now takes an amount int, if any adjustments need to be made
                    }

                    if (className == "goal")
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);

                        level.SetTile(TempX, TempY, new Goal(TempX * GlobalVar.TILESIZE, TempY * GlobalVar.TILESIZE));
                    }

                    if (className == "trap") //Trap now takes 3 arguments
                    {
                        int TempX = Int32.Parse(classCord[1]);
                        int TempY = Int32.Parse(classCord[2]);
                        string type = classCord[3];

                        level.SetTile(TempX, TempY, new Traps(TempX * GlobalVar.TILESIZE, TempY * GlobalVar.TILESIZE, "MvtTrap"));
                        //This now takes trap type string, if you need to make any adjustments
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

            this.LoadLevel(1);
            playerChar = new Player(0, 0, 0);

            // TODO: use this.Content to load your game content here
            // Order player textures from lowest to highest. (Alex wishes this was a robot.)
            playerChar.CurrentTexture.Add(this.Content.Load<Texture2D>("Default_Body"));
            playerChar.CurrentTexture.Add(this.Content.Load<Texture2D>("Default_Vest"));
            playerChar.CurrentTexture.Add(this.Content.Load<Texture2D>("Default_Backpack"));
            playerChar.CurrentTexture.Add(this.Content.Load<Texture2D>("Default_Head"));
            playerChar.CurrentTexture.Add(this.Content.Load<Texture2D>("Default_Bandana"));

            menuFont = this.Content.Load<SpriteFont>("MenuFont");
            headerFont = this.Content.Load<SpriteFont>("MenuHeaderFont");

            gridNorth = this.Content.Load<Texture2D>("Default_Up");
            gridEast = this.Content.Load<Texture2D>("Default_Right");
            gridSouth = this.Content.Load<Texture2D>("Default_Down");
            gridWest = this.Content.Load<Texture2D>("Default_Left");
            gridWall = this.Content.Load<Texture2D>("Default_Tile");
            gridFloor = this.Content.Load<Texture2D>("Default_Wall");

            for (int j = 0; j < level.Width; j++)
            {
                for (int i = 0; i < level.Height; i++)
                {
                    //If trap.
                    if (level.GetGamePiece(i, j).Type == "MvtTrap")
                    {
                        level.GetGamePiece(i, j).CurrentTexture.Add(gridFloor);
                    }
                    //If tile.
                    else if (level.GetGamePiece(i, j).Type == "Tile")
                    {
                        level.GetGamePiece(i, j).CurrentTexture.Add(gridFloor);
                        level.GetGamePiece(i, j).CurrentTexture.Add(gridNorth);
                        level.GetGamePiece(i, j).CurrentTexture.Add(gridEast);
                        level.GetGamePiece(i, j).CurrentTexture.Add(gridSouth);
                        level.GetGamePiece(i, j).CurrentTexture.Add(gridWest);
                    }
                    //If wall.
                    else if (level.GetGamePiece(i, j).Type == "Wall")
                    {
                        level.GetGamePiece(i, j).CurrentTexture.Add(gridWall);
                    }
                    //If unhandled object type, make it a tile.
                    else
                    {
                        level.SetTile(i, j, new Tile(i * GlobalVar.TILESIZE, j * GlobalVar.TILESIZE));
                        level.GetGamePiece(i, j).CurrentTexture.Add(gridFloor);
                    }
                }
            }
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
            keyboard = Keyboard.GetState();

            switch (current)
            {
                case GameState.Title:
                    current = GameState.PlacingTiles;
                    break;
                case GameState.Menus:
                    break;
                case GameState.PlacingTiles:
                    if (mouse.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        //Place or turn direction tile at gameGrid[j / gamePieceSize, i / gamePieceSize]
                        Console.WriteLine(mouse.X + " " + mouse.Y);
                        level.GetGamePiece(
                            (int)(mouse.X / GlobalVar.TILESIZE),
                            (int)(mouse.Y / GlobalVar.TILESIZE)).IncrementType();
                    }
                    if(keyboard.IsKeyDown(Keys.Enter))
                    {
                        current = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    // Collision detection, for playing the game.
                    if (current == GameState.Playing)
                    {
                        // We probably want this to hold for a second then move the player by GlobalVar.TileSize pixels.
                        // Holding for a second so that it's actually visible what happens- and it's not like we have a reason to
                        // code this thing to take real-time input.
                        /*for (int j = 0; j < (GlobalVar.GAMEWIDTH / GlobalVar.TILESIZE); j++)
                        {
                            for (int i = 0; i < (GlobalVar.GAMEHEIGHT / GlobalVar.TILESIZE); i++)
                            {
                                if (playerChar.CheckPosition(level.GetGamePiece(i, j)))
                                {
                                    //tileGrid[i, j].ThingIn.HitTrap(playerChar);
                                }
                            }
                        }*/

                        // Don't do anything else for another second.
                        System.Threading.Thread.Sleep(1000);
                    }
                    break;
                case GameState.GameOver:
                    break;
            }

            mousePrev = Mouse.GetState();
            keyboardPrev = Keyboard.GetState();
            
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

            switch (current)
            {
                case GameState.Title:
                    spriteBatch.DrawString(menuFont,        //Draw Press Enter Prompt
                        "Press enter to continue.",
                        new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                        Color.White);
                    break;
                case GameState.Menus:
                    break;
                case GameState.PlacingTiles:
                    level.Draw(spriteBatch);
                    playerChar.Draw(spriteBatch);
                    break;
                case GameState.Playing:
                    level.Draw(spriteBatch);
                    playerChar.Draw(spriteBatch);
                    break;
                case GameState.GameOver:
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
