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

        Vector2 menuPos;            //Position of the menu header
        SpriteFont menuFont;
        SpriteFont headerFont;

        Level level;

        //Textures
        Texture2D gridNorth;
        Texture2D gridEast;
        Texture2D gridSouth;
        Texture2D gridWest;
        Texture2D gridWall;
        Texture2D gridFloor;
        List<Texture2D> tileTextures;
        List<Texture2D> wallTextures;
        List<Texture2D> playerTextures;

        //Base game methods below...
        int playerMove;
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
            current = GameState.Title;      // Establish starting game state.
            this.IsMouseVisible = true;     // Make mouse visible on screen.

            base.Initialize();
        }

        // Loads a level. Requires integer for specific file name.
        private void LoadLevel(int newLevel)
        {
            //concatanates "LevelMap" and passed int to find map file.
            string file = "LevelMap" + newLevel + ".txt";
            
            //makes new stream reader
            StreamReader input;
            try
            {
                //mapfiles will be one line long to avoid dealing with newlines. Also, assuming mapfile resides in bin.
                input = new StreamReader(file);
                string line = "";

                int tempWidth = int.Parse(input.ReadLine());
                int tempHeight = int.Parse(input.ReadLine());
                int columnNumber;
                int lineNumber = 0;

                level = new Level(tempWidth, tempHeight, GlobalVar.TILESIZE);

                while((line = input.ReadLine()) != null)
                {
                    columnNumber = 0;
                    foreach(char block in line)
                    {
                        switch(block)
                        {
                            default:
                                level.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                break;
                            case '1':
                                level.SetTile(columnNumber, lineNumber, new Wall(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    wallTextures));
                                break;
                            case 'X':
                                level.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                playerChar = new Player(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    0,
                                    playerTextures);
                                break;
                        }
                        columnNumber++;
                    }
                    lineNumber++;
                }
            }
            //Close if File Not Found. (Change to send an error message?)
            catch(FileNotFoundException)
            {
                this.Exit();
            }
            //Close if any other odd Exception.
            catch(Exception)
            {
                this.Exit();
            }
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

            menuFont = this.Content.Load<SpriteFont>("MenuFont");
            headerFont = this.Content.Load<SpriteFont>("MenuHeaderFont");

            menuPos = headerFont.MeasureString("CHAOS UNIVERSITY");

            gridNorth = this.Content.Load<Texture2D>("Default_Up");
            gridEast = this.Content.Load<Texture2D>("Default_Right");
            gridSouth = this.Content.Load<Texture2D>("Default_Down");
            gridWest = this.Content.Load<Texture2D>("Default_Left");
            gridWall = this.Content.Load<Texture2D>("Default_Tile");
            gridFloor = this.Content.Load<Texture2D>("Default_Wall");

            // Order player textures from lowest to highest. (Alex wishes this was a robot.)
            playerTextures = new List<Texture2D>();
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Body"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Vest"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Backpack"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Head"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Bandana"));

            tileTextures = new List<Texture2D>();
            tileTextures.Add(gridFloor);
            tileTextures.Add(gridNorth);
            tileTextures.Add(gridEast);
            tileTextures.Add(gridSouth);
            tileTextures.Add(gridWest);

            wallTextures = new List<Texture2D>();
            wallTextures.Add(gridWall);

            this.LoadLevel(1);
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
                //TITLE SCREEN
                case GameState.Title:
                    if (keyboard.IsKeyDown(Keys.Enter))
                    {
                        current = GameState.PlacingTiles;
                    }
                    break;

                //MENU SCREEN
                case GameState.Menus:
                    break;

                //PLACING TILES
                case GameState.PlacingTiles:
                    if (mouse.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
                    {
                        //Place or turn direction tile at gameGrid[j / gamePieceSize, i / gamePieceSize]
                        Console.WriteLine(mouse.X + " " + mouse.Y);
                        try
                        {
                            level.GetGamePiece(
                                (int)(mouse.X / GlobalVar.TILESIZE),
                                (int)(mouse.Y / GlobalVar.TILESIZE)).IncrementType();
                        }
                        catch (IndexOutOfRangeException) { }
                    }
                    if(keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter))
                    {
                        current = GameState.Playing;
                    }
                    break;

                //RUNNING SEQUENCE
                case GameState.Playing:
                    playerMove = (int)(50 * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    switch (playerChar.Direction)
                    {
                        case 0:
                            playerChar.PositionRect = new Rectangle(
                                playerChar.PositionRect.X,
                                playerChar.PositionRect.Y - 1,
                                playerChar.PositionRect.Width,
                                playerChar.PositionRect.Height);
                            break;

                        case 1:
                            playerChar.PositionRect = new Rectangle(
                                playerChar.PositionRect.X + 1,
                                playerChar.PositionRect.Y,
                                playerChar.PositionRect.Width,
                                playerChar.PositionRect.Height);
                            break;

                        case 2:
                            playerChar.PositionRect = new Rectangle(
                                playerChar.PositionRect.X,
                                playerChar.PositionRect.Y + 1,
                                playerChar.PositionRect.Width,
                                playerChar.PositionRect.Height);
                            break;

                        case 3:
                            playerChar.PositionRect = new Rectangle(
                                playerChar.PositionRect.X - 1,
                                playerChar.PositionRect.Y,
                                playerChar.PositionRect.Width,
                                playerChar.PositionRect.Height);
                            break;
                    }

                    /*
                    int tempX = (int)(playerChar.PositionRect.X / GlobalVar.TILESIZE);
                    int tempY = (int)(playerChar.PositionRect.Y / GlobalVar.TILESIZE);

                    GamePiece subPiece = level.GetGamePiece(tempX, tempY)
                     */

                    for (int j = 0; j < level.Height; ++j)
                    {
                        for (int i = 0; i < level.Height; ++i)
                        {
                            if(level.GetGamePiece(i,j).PositionRect.Center == playerChar.PositionRect.Center)
                            {
                                GamePiece subPiece = level.GetGamePiece(i, j);

                                switch (subPiece.TileState)
                                {
                                    case TileState.Floor:
                                        break;
                                    case TileState.North:
                                        playerChar.turn(0);
                                        break;
                                    case TileState.East:
                                        playerChar.turn(1);
                                        break;
                                    case TileState.South:
                                        playerChar.turn(2);
                                        break;
                                    case TileState.West:
                                        playerChar.turn(3);
                                        break;
                                }
                            }
                        }
                    }

                    break;

                //GAME OVER
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
                //TITLE SCREEN
                case GameState.Title:
                    spriteBatch.DrawString(headerFont,  //Draw Title
                        "CHAOS UNIVERSITY",
                        new Vector2(GraphicsDevice.Viewport.Width / 2, 0),
                        Color.White,
                        0.0f,
                        new Vector2(menuPos.X / 2, 0),
                        new Vector2(1.2f, 1.0f),
                        SpriteEffects.None,
                        0);
                    spriteBatch.DrawString(menuFont,    //Draw Press Enter Prompt
                        "Press enter to continue.",
                        new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                        Color.White);
                    break;

                //MENU SCREEN
                case GameState.Menus:
                    break;

                //PLACING TILES
                case GameState.PlacingTiles:
                    level.Draw(spriteBatch);
                    playerChar.Draw(spriteBatch);
                    break;

                //RUNNING SEQUENCE
                case GameState.Playing:
                    level.Draw(spriteBatch);
                    playerChar.Draw(spriteBatch);
                    break;

                //GAME OVER
                case GameState.GameOver:
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
