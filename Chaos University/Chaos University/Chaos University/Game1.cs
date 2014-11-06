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

        int clickPrevX;             //X index of previous tile changed.
        int clickPrevY;             //Y index of previous tile changed.
        Level level;                //Current level of the game.
        Player ninjaChar;          //Player
        Rectangle ninjaStart;      //Player starting position.
        Enemy guard;                //Guard
        Rectangle guardStart;       //Guard starting position.
        GameState current;          //State of the game (title, menu, playing, gameover, etc.)

        Vector2 menuPos;            //Position of the menu header
        Vector2 gameOverPos;        //Position of the game over screen.
        Vector2 levelCompPos;       //Position of the level complete screen.
        SpriteFont menuFont;        //Font of menu text
        SpriteFont headerFont;      //Font of header text

        //Textures
        List<Texture2D> tileTextures;
        List<Texture2D> wallTextures;
        List<Texture2D> goalTextures;
        List<Texture2D> playerTextures;
        List<Texture2D> moneyTextures;

        //Songs
        List<Song> music;

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

            clickPrevX = -1;                //Start clickPrev at a nonexistent index.
            clickPrevY = -1;

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

                int tempWidth = int.Parse(input.ReadLine());    //Width of map from file.
                int tempHeight = int.Parse(input.ReadLine());   //Height of map from file.
                int levelPar = int.Parse(input.ReadLine());     //Par of level from file.
                int columnNumber;                               //Counter for each column being written.
                int lineNumber = 0;                             //Counter for each row/line being written.

                level = new Level(tempWidth, tempHeight, GlobalVar.TILESIZE, levelPar);     //Establish level.

                //While a line is to be read.
                while((line = input.ReadLine()) != null)
                {
                    columnNumber = 0;
                    foreach(char block in line)
                    {
                        switch(block)
                        {
                            //Any unknown piece becomes a tile.
                            default:
                                level.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                break;
                            //1 = Wall.
                            case '1':
                                level.SetTile(columnNumber, lineNumber, new Wall(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    wallTextures));
                                break;
                            //M = Moneh
                            case 'M':
                                level.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                level.AddObject(new Money(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    moneyTextures));
                                break;
                            //F = Goal
                            case 'F':
                                level.SetTile(columnNumber, lineNumber, new Goal(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    goalTextures));
                                break;
                            //X = Player and guard start and tile.
                            case 'X':
                                level.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));

                                ninjaChar = new Player(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    0,
                                    playerTextures);
                                ninjaStart = ninjaChar.PositionRect;

                                guard = new Enemy(
                                    2 * GlobalVar.TILESIZE,
                                    1 * GlobalVar.TILESIZE,
                                    2,
                                    playerTextures);
                                guardStart = guard.PositionRect;
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

        // Failure state.
        private void Fail()
        {
            ninjaChar.PositionRect = ninjaStart;  //Reset Player Location.
            ninjaChar.turn(0);                     //Reset Player Direction.
            ninjaChar.Tries--;                     //Reduce number of tries player has.
            ninjaChar.ParCount = 0;                //Reset par for player.
            ninjaChar.Moving = false;              //Halt player.
            level.ActivateMoney();                  //Reset monies.

            //Reset all direction tiles.
            for (int b = 0; b < level.Height; ++b)
            {
                for (int a = 0; a < level.Width; ++a)
                {
                    if (level.GetGamePiece(a, b).PieceState == PieceState.North ||
                        level.GetGamePiece(a, b).PieceState == PieceState.East ||
                        level.GetGamePiece(a, b).PieceState == PieceState.South ||
                        level.GetGamePiece(a, b).PieceState == PieceState.West)
                    {
                        level.GetGamePiece(a, b).PieceState = PieceState.Floor;
                        level.GetGamePiece(a, b).IndexTexture = 0;
                    }
                }
            }

            //Induce Game over if player tries = 0;
            if (ninjaChar.Tries == 0)
            {
                current = GameState.GameOver;
            }
        }
        
        // will create a new instance of the character creator and display it.
        public void LoadCharacterCreator()
        {
            CharacterCreator characterCreator = new CharacterCreator();
            characterCreator.Show();
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
            gameOverPos = headerFont.MeasureString("GAME OVER");
            levelCompPos = headerFont.MeasureString("LEVEL COMPLETE");

            music = new List<Song>();
            music.Add(this.Content.Load<Song>("MainTheme"));

            // Order tile textures in order that they appear when clicked.
            tileTextures = new List<Texture2D>();
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Tile"));
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Up"));
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Right"));
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Down"));
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Left"));

            //Single Wall texture.
            wallTextures = new List<Texture2D>();
            wallTextures.Add(this.Content.Load<Texture2D>("Default_Wall"));

            //Single goal texture.
            goalTextures = new List<Texture2D>();
            goalTextures.Add(this.Content.Load<Texture2D>("Default_Goal"));

            //Single Money texture.
            moneyTextures = new List<Texture2D>();
            moneyTextures.Add(this.Content.Load<Texture2D>("Default_Collect"));

            // Order player textures from lowest to highest. (Alex wishes this was a robot.)
            playerTextures = new List<Texture2D>();
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Body"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Vest"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Backpack"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Head"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Bandana"));

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
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter)) //Press enter to play.
                    {
                        current = GameState.Playing;

                        //Play sound. Do this only to type change.
                        MediaPlayer.Play(music[0]);

                    }
                    break;

                //MENU SCREEN
                case GameState.Menus:
                    break;

                //PLACING TILES
                case GameState.Playing:
                    //Mouse click to change tiles. Click fails beyond par.
                    if (mouse.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released && ninjaChar.ParCount <= level.Par)
                    {
                        try
                        {
                            int clickNowX = mouse.X / GlobalVar.TILESIZE;
                            int clickNowY = mouse.Y / GlobalVar.TILESIZE;
                            //Change tile at mouse location. True if Increment could have occured.
                            if (level.GetGamePiece(clickNowX, clickNowY).IncrementType())
                            {
                                //Increment parCount if tile changed is a new tile.
                                if (clickNowX != clickPrevX || clickNowY != clickPrevY)
                                {
                                    ninjaChar.ParCount++;
                                    clickPrevX = clickNowX;
                                    clickPrevY = clickNowY;
                                    //Reverses any click beyond par.
                                    if (ninjaChar.ParCount == level.Par + 1)
                                    {
                                        level.GetGamePiece(clickPrevX, clickPrevY).DecrementType();
                                    }
                                }
                            }
                        }
                        catch (IndexOutOfRangeException) { }    //Catch and ignore exception that mouse is beyond map.
                    }

                    //Keyboard buttons.
                    //Pressing enter makes the player start move.
                    if(keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter) && !ninjaChar.Moving)
                    {
                        ninjaChar.Moving = true;
                    }
                    if(keyboard.IsKeyDown(Keys.R) && keyboardPrev.IsKeyUp(Keys.R))
                    {
                        this.Fail();
                    }

                    //Move Player
                    ninjaChar.Move((int)(100 * (float)gameTime.ElapsedGameTime.TotalSeconds));

                    //Move Guard
                    guard.Move((int)(100 * (float)gameTime.ElapsedGameTime.TotalSeconds));

                    //Guard attack player, failed attempt
                    //if (guard.Attack(playerChar) == true)
                    //{
                    //    this.Fail();
                    //}

                    //For all gamepieces in level grid, check for direcion changes or collisions.
                    for (int j = 0; j < level.Height; ++j)
                    {
                        for (int i = 0; i < level.Width; ++i)
                        {
                            //If player on direction tile.
                            if(level.GetGamePiece(i,j).PositionRect.Center == ninjaChar.PositionRect.Center)
                            {
                                //Turn player based on tile direction.
                                switch (level.GetGamePiece(i, j).PieceState)
                                {
                                    case PieceState.Floor:
                                        break;
                                    case PieceState.North:
                                        ninjaChar.turn(0);
                                        break;
                                    case PieceState.East:
                                        ninjaChar.turn(1);
                                        break;
                                    case PieceState.South:
                                        ninjaChar.turn(2);
                                        break;
                                    case PieceState.West:
                                        ninjaChar.turn(3);
                                        break;
                                    case PieceState.Goal:
                                        current = GameState.LevelComp;
                                        break;
                                }
                            }

                            //If game piece is a wall.
                            if(level.GetGamePiece(i,j).PieceState == PieceState.Wall)
                            {
                                //Check if player collided with it.
                                if(level.GetGamePiece(i,j).CheckCollision(ninjaChar))
                                {
                                    this.Fail();
                                }
                            }

                            //If game piece is a wall.
                            if (level.GetGamePiece(i, j).PieceState == PieceState.Wall)
                            {
                                //Check if enemy collided with it.
                                if (level.GetGamePiece(i, j).CheckCollision(guard))
                                {
                                    guard.Patrol(0);
                                }
                            }

                        }

                        //For all Game Pieces in level object list, check for collision.
                        foreach (Money gamePiece in level.Monies)
                        {
                            if(gamePiece.CheckCollision(ninjaChar))
                            {
                                gamePiece.Active = false;
                            }
                        }
                    }
                    break;
                //LEVEL COMPLETE
                case GameState.LevelComp:
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter))
                    {
                        current = GameState.Title;
                    }
                    break;
                //GAME OVER
                case GameState.GameOver:
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter))
                    {
                        current = GameState.Title;
                    }
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
                    //Draw Title
                    spriteBatch.DrawString(headerFont,
                        "CHAOS UNIVERSITY",
                        new Vector2(GraphicsDevice.Viewport.Width / 2, 0),
                        Color.White,
                        0.0f,
                        new Vector2(menuPos.X / 2, 0),
                        new Vector2(1.2f, 1.0f),
                        SpriteEffects.None,
                        0);
                    //Draw Directions.
                    spriteBatch.DrawString(menuFont,
                        "Use the mouse to select tiles and change them to direction tiles.\n" +
                        "You can place tiles before or during player movement.\n" +
                        "You have a limited number of tries to place tiles.",
                        new Vector2(25, 100),
                        Color.White);
                    //Draw Press Enter Prompt
                    spriteBatch.DrawString(menuFont,
                        "Press enter to continue.",
                        new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                        Color.White);
                    break;

                //MENU SCREEN
                case GameState.Menus:
                    break;

                //PLAYING
                case GameState.Playing:
                    //Draw Par UI Element.
                    if (ninjaChar.ParCount < level.Par)
                    {
                        spriteBatch.DrawString(menuFont,
                            String.Format("Par:   {0} of {1}", ninjaChar.ParCount, level.Par),
                            new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                            Color.White);
                    }
                    //Draw maxed out Par UI Element. Ternary expression stops display from going above par.
                    else
                    {
                        spriteBatch.DrawString(menuFont,    
                            String.Format("Par:   {0} of {1} (PAR REACHED)", ninjaChar.ParCount <= level.Par ? ninjaChar.ParCount : level.Par, level.Par),
                            new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                            Color.White);
                    }
                    //Draw Tries Counter.
                    spriteBatch.DrawString(menuFont,
                        String.Format("Tries: {0}", ninjaChar.Tries),
                        new Vector2(25, GraphicsDevice.Viewport.Height - 50),
                        Color.White);
                    //Draw Enter Directions.
                    spriteBatch.DrawString(menuFont,
                        "Press Enter to start movement.",
                        new Vector2(420, GraphicsDevice.Viewport.Height - 26),
                        Color.White);
                    //Draw Reset Directions.
                    spriteBatch.DrawString(menuFont,
                        "Press R to reset.",
                        new Vector2(420, GraphicsDevice.Viewport.Height - 50),
                        Color.White);
                    level.Draw(spriteBatch);
                    ninjaChar.Draw(spriteBatch);
                    //guard.Draw(spriteBatch);
                    break;

                //Level Complete feedback screen.
                case GameState.LevelComp:
                    //Draw LEVEL COMPLETE
                    spriteBatch.DrawString(headerFont,
                        "LEVEL COMPLETE",
                        new Vector2(GraphicsDevice.Viewport.Width / 2, 0),
                        Color.White,
                        0.0f,
                        new Vector2(levelCompPos.X / 2, 0),
                        new Vector2(1.2f, 1.0f),
                        SpriteEffects.None,
                        0);
                    //Draw Items collected.
                    spriteBatch.DrawString(menuFont,
                        String.Format("Items Collected: {0} of {1}", level.Monies.Count - level.GetMoneyCount(), level.Monies.Count),
                        new Vector2(25, 100),
                        Color.White);
                    //Draw Press Enter Prompt
                    spriteBatch.DrawString(menuFont,
                        "Press enter to continue.",
                        new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                        Color.White);
                    break;

                //GAME OVER
                case GameState.GameOver:
                    //Draw GAME OVER
                    spriteBatch.DrawString(headerFont,
                        "GAME OVER",
                        new Vector2(GraphicsDevice.Viewport.Width / 2, 0),
                        Color.White,
                        0.0f,
                        new Vector2(gameOverPos.X / 2, 0),
                        new Vector2(1.2f, 1.0f),
                        SpriteEffects.None,
                        0);
                    //Draw Press Enter Prompt
                    spriteBatch.DrawString(menuFont,
                        "Press enter to continue.",
                        new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                        Color.White);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
