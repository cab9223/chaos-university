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

        //Gamestate + Level stuff.
        GameState current;          //State of the game (title, menu, playing, gameover, etc.)
        int indexLevel;             //Current level index.
        Level level;                //Current level of the game.
        List<Level> levels;         //List of levels.
        
        //Input stuff.
        MouseState mouse;           //Current mouse state
        MouseState mousePrev;       //Previous mouse state
        KeyboardState keyboard;     //Keyboard state
        KeyboardState keyboardPrev; //Keyboard state previous
        int clickPrevX;             //X index of previous tile changed.
        int clickPrevY;             //Y index of previous tile changed.

        //Guards stuff.
        Enemy guard;                //Guard temp
        List<Enemy> guards;         //List of Guards
        List<Enemy> activeGuards;   //List of Active Guards
        bool isGuard;               //Active Guards
        int guardIndex;             //Tells which guard to make
        int guardCount;             //Tells amount of guards
        Queue<int> guardAmount;     //Also tells amount of guards
        bool attacked;              //Guard attacked player
        Indicator indicator;        //Indicator of currently selected tile.
        float timer;                //Short pause
        bool alerted;               //Alert went off

        //Other things.
        Vector2 menuPos;            //Position of the menu header
        Vector2 gameOverPos;        //Position of the game over screen.
        Vector2 levelCompPos;       //Position of the level complete screen.
        SpriteFont menuFont;        //Font of menu text
        SpriteFont headerFont;      //Font of header text
        int camX;                   //X offset of view.
        int camY;                   //Y offset of view.
        int camXCenter;             //Current X center position of view.
        int camYCenter;             //Current Y center position of view.
        string title;
        bool fastActive;

        //Textures
        List<Texture2D> tileTextures;
        List<Texture2D> wallTextures;
        List<Texture2D> goalTextures;
        List<Texture2D> playerTextures;
        List<Texture2D> guardTextures;
        List<Texture2D> moneyTextures;
        List<Texture2D> specialTileTextures;
        //List<Texture2D> abilityTextures;

        //Songs
        List<Song> music;
        List<Song> effects;

        //Videos
        Video introVideo;
        VideoPlayer videoPlayer;

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
            current = GameState.Intro;      // Establish starting game state.
            this.IsMouseVisible = true;     // Make mouse visible on screen.
            videoPlayer = new VideoPlayer();
            levels = new List<Level>();
            guards = new List<Enemy>();
            activeGuards = new List<Enemy>();
            guardAmount = new Queue<int>();
            title = "PLACE HOLDER";


            indexLevel = -1;
            clickPrevX = -1;                //Start clickPrev at a nonexistent index.
            clickPrevY = -1;
            camX = 0;
            camY = 0;
            camXCenter = 0;
            camYCenter = 0;
            isGuard = false;
            guardIndex = 0;
            guardCount = 0;
            attacked = false;
            timer = 0;
            alerted = false;
            fastActive = false;

            base.Initialize();
        }

        // Loads a new level and return it. Requires integer for specific file name.
        private Level LoadLevel(int levelNum)
        {
            //concatanates "LevelMap" and passed int to find map file.
            string file = "LevelMap" + levelNum + ".txt";
            Level newLevel = new Level(0, 0, 0, 0); //New Level with default values to satisfy compiler.
            
            
            //makes new stream reader
            StreamReader input;
            try
            {
                //mapfiles will be one line long to avoid dealing with newlines. Also, assuming mapfile resides in bin.
                input = new StreamReader(file);
                string line = "";

                //Read map height, width, and par. Establish newLevel.
                int tempWidth = int.Parse(input.ReadLine());    //Width of map from file.
                int tempHeight = int.Parse(input.ReadLine());   //Height of map from file.
                int newLevelPar = int.Parse(input.ReadLine());  //Par of newLevel from file.
                newLevel = new Level(tempWidth, tempHeight, GlobalVar.TILESIZE, newLevelPar);     //Establish newLevel.

                //Read character directions.
                Queue<int> charDirs = new Queue<int>();
                foreach(char dir in input.ReadLine())
                {
                    charDirs.Enqueue(int.Parse(dir.ToString()));
                }

                //Read enemy directions.
                Queue<int> guardDirs = new Queue<int>();
                foreach (char dir in input.ReadLine())
                {
                    guardDirs.Enqueue(int.Parse(dir.ToString()));
                }

                //Counters for reading map blocks.
                int columnNumber;       //Counter for each column being written.
                int lineNumber = 0;     //Counter for each row/line being written.

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
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                break;
                            //1 = Wall.
                            case '1':
                                newLevel.SetTile(columnNumber, lineNumber, new Wall(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    wallTextures));
                                break;
                            //M = Moneh
                            case 'M':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                newLevel.AddObject(new Money(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    moneyTextures));
                                break;
                            //F = Goal
                            case 'F':
                                newLevel.SetTile(columnNumber, lineNumber, new Goal(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    goalTextures));
                                break;
                            //N = Ninja start and tile.
                            case 'N':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                newLevel.Ninja = new Player(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    newLevel.RotNinja = charDirs.Dequeue(),
                                    playerTextures,
                                    Player.Major.Ninja);
                                newLevel.StartNinja = newLevel.Ninja.PositionRect;
                                newLevel.IsNinja = true;
                                break;
                            //R = Recon
                            case 'R':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                newLevel.Recon = new Player(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    newLevel.RotRecon = charDirs.Dequeue(),
                                    playerTextures,
                                    Player.Major.Recon);
                                newLevel.StartRecon = newLevel.Recon.PositionRect;
                                newLevel.IsRecon = true;
                                break;
                            //T = assaulT
                            case 'T':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                newLevel.Assault = new Player(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    newLevel.RotAssault = charDirs.Dequeue(),
                                    playerTextures,
                                    Player.Major.Assault);
                                newLevel.StartAssault = newLevel.Assault.PositionRect;
                                newLevel.IsAssault = true;
                                break;
                            //X = Guard
                            case 'X':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                guard = new Enemy(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    guardDirs.Dequeue(),
                                    guardTextures);
                                guards.Add(guard);
                                guardCount = guardCount + 1;
                                break;
                            //W = Rotate_Clockwise_North
                            case 'W':
                                newLevel.SetTile(columnNumber, lineNumber, new SpecialTile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    specialTileTextures, 1, 0));
                                break;
                            //D = Rotate_Clockwise_East
                            case 'D':
                                newLevel.SetTile(columnNumber, lineNumber, new SpecialTile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    specialTileTextures, 1, 1));
                                break;
                            //S = Rotate_Clockwise_South
                            case 'S':
                                newLevel.SetTile(columnNumber, lineNumber, new SpecialTile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    specialTileTextures, 1, 2));
                                break;
                            //A = Rotate_Clockwise_West
                            case 'A':
                                newLevel.SetTile(columnNumber, lineNumber, new SpecialTile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    specialTileTextures, 1, 3));
                                break;
                            //I = Rotate_Counter_Clockwise_North
                            case 'I':
                                newLevel.SetTile(columnNumber, lineNumber, new SpecialTile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    specialTileTextures, 2, 0));
                                break;
                            //L = Rotate_Counter_Clockwise_East
                            case 'L':
                                newLevel.SetTile(columnNumber, lineNumber, new SpecialTile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    specialTileTextures, 2, 3));
                                break;
                            //K = Rotate_Counter_Clockwise_South
                            case 'K':
                                newLevel.SetTile(columnNumber, lineNumber, new SpecialTile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    specialTileTextures, 2, 2));
                                break;
                            //J = Rotate_Counter_Clockwise_West
                            case 'J':
                                newLevel.SetTile(columnNumber, lineNumber, new SpecialTile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    specialTileTextures, 2, 1));
                                break;
                        }
                        columnNumber++;
                    }
                    lineNumber++;
                }

                guardAmount.Enqueue(int.Parse(guardCount.ToString())); //Sets a guard count for each stage
                guardCount = 0;
                
                input.Close();
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

            return newLevel;
        }

        // Failure state.
        private void Fail()
        {
            //Ninja
            if(level.IsNinja)
            {
                level.Ninja.PositionRect = level.StartNinja;    //Reset Player Location.
                level.Ninja.turn(level.RotNinja);               //Reset Player Direction.
                level.Ninja.Moving = false;                     //Halt player.
                for (int i = 0; i < guardCount; i++)
                {
                    activeGuards[i].Reset();
                }
            }

            //Recon
            if (level.IsRecon)
            {
                level.Recon.PositionRect = level.StartRecon;    //Reset Player Location.
                level.Recon.turn(level.RotRecon);               //Reset Player Direction.
                level.Recon.Moving = false;                     //Halt player.
                for (int i = 0; i < guardCount; i++)
                {
                    activeGuards[i].Reset();
                }
            }

            //Assault
            if (level.IsAssault)
            {
                level.Assault.PositionRect = level.StartAssault;    //Reset Player Location.
                level.Assault.turn(level.RotAssault);               //Reset Player Direction.
                level.Assault.Moving = false;                       //Halt player.
                for (int i = 0; i < guardCount; i++)
                {
                    activeGuards[i].Reset();
                }
            }


            GlobalVar.ParCount = 0;                             //Reset par for player.
            level.ActivateMoney();                              //Reset monies.
            indicator.Active = false;                           //Reset indicator.
            clickPrevX = -1;                                    //Reset clickPrev at a nonexistent index.
            clickPrevY = -1;                                    //Reset clickPrev at a nonexistent index.
            this.CenterCamera();
            GlobalVar.SpeedLevel = 50;
            fastActive = false;

            //Reset all direction tiles.
            for (int b = 0; b < level.Height; ++b)
            {
                for (int a = 0; a < level.Width; ++a)
                {
                    if (level.GetGamePiece(a, b).PieceState == PieceState.North ||
                        level.GetGamePiece(a, b).PieceState == PieceState.East  ||
                        level.GetGamePiece(a, b).PieceState == PieceState.South ||
                        level.GetGamePiece(a, b).PieceState == PieceState.West)
                    {
                        level.GetGamePiece(a, b).PieceState = PieceState.Floor;
                        level.GetGamePiece(a, b).IndexTexture = 0;
                    }
                    if (level.GetGamePiece(a, b).PieceState == PieceState.SpecialNorth||
                        level.GetGamePiece(a, b).PieceState == PieceState.SpecialEast  ||
                        level.GetGamePiece(a, b).PieceState == PieceState.SpecialSouth ||
                        level.GetGamePiece(a, b).PieceState == PieceState.SpecialWest)
                    {
                        level.GetGamePiece(a, b).ReturnStartingDirection();
                    }
                }
            }
        }

        //Succeed state.
        private void Success()
        {
            GlobalVar.ParCount = 0;                             //Reset par.
            indicator.Active = false;                           //Reset indicator.
            clickPrevX = -1;                                    //Reset clickPrev at a nonexistent index.
            clickPrevY = -1;                                    //Reset clickPrev at a nonexistent index.
        }


        private void FastForward() //Speeds up gameplay or returns to normal
        {
            if (fastActive == false)
            {
                GlobalVar.SpeedLevel = 40;
                fastActive = true;
            }
            else if (fastActive == true)
            {
                GlobalVar.SpeedLevel = 50;
                fastActive = false;
            }
        }


        //Checks the state of the mouse and performs appropriate actions.
        private void CheckGameMouse()
        {
            //LEFT MOUSE CLICK. Mouse click to change tiles. Click fails beyond par.
            if (mouse.LeftButton == ButtonState.Pressed && mousePrev.LeftButton == ButtonState.Released)
            {
                try
                {
                    int clickNowX = (mouse.X - camX) / GlobalVar.TILESIZE;
                    int clickNowY = (mouse.Y - camY) / GlobalVar.TILESIZE;
                    //Change tile at mouse location. True if Increment could have occured.
                    //Check to see if tile is of special type, won't Increment if it is.
                    if (level.GetGamePiece(clickNowX, clickNowY).PieceState != PieceState.SpecialNorth &&
                        level.GetGamePiece(clickNowX, clickNowY).PieceState != PieceState.SpecialEast &&
                        level.GetGamePiece(clickNowX, clickNowY).PieceState != PieceState.SpecialSouth &&
                        level.GetGamePiece(clickNowX, clickNowY).PieceState != PieceState.SpecialWest)
                    {
                        if (level.GetGamePiece(clickNowX, clickNowY).IncrementType())
                        {
                            //Increment parCount if tile changed is a new tile.
                            if (clickNowX != clickPrevX || clickNowY != clickPrevY)
                            {
                                indicator.Active = true;                    //Activate indicator at changed tile.
                                indicator.PositionRect = new Rectangle(     //Move indicator to changed tile.
                                    clickNowX * GlobalVar.TILESIZE,
                                    clickNowY * GlobalVar.TILESIZE,
                                    GlobalVar.TILESIZE,
                                    GlobalVar.TILESIZE);
                                GlobalVar.ParCount++;
                                clickPrevX = clickNowX;
                                clickPrevY = clickNowY;
                            }
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }    //Catch and ignore exception that mouse is beyond map.
            }

            //RIGHT MOUSE CLICK. Mouse click to change tiles. Click fails beyond par.
            if (mouse.RightButton == ButtonState.Pressed && mousePrev.RightButton == ButtonState.Released)
            {
                try
                {
                    int clickNowX = (mouse.X - camX) / GlobalVar.TILESIZE;
                    int clickNowY = (mouse.Y - camY) / GlobalVar.TILESIZE;
                    //Change tile at mouse location. True if Increment could have occured.
                    if (level.GetGamePiece(clickNowX, clickNowY).DecrementType())
                    {
                        //Increment parCount if tile changed is a new tile.
                        if (clickNowX != clickPrevX || clickNowY != clickPrevY)
                        {
                            indicator.Active = true;                    //Activate indicator at changed tile.
                            indicator.PositionRect = new Rectangle(     //Move indicator to changed tile.
                                clickNowX * GlobalVar.TILESIZE,
                                clickNowY * GlobalVar.TILESIZE,
                                GlobalVar.TILESIZE,
                                GlobalVar.TILESIZE);
                            GlobalVar.ParCount++;
                            clickPrevX = clickNowX;
                            clickPrevY = clickNowY;
                        }
                    }
                }
                catch (IndexOutOfRangeException) { }    //Catch and ignore exception that mouse is beyond map.
            }
        }

        //Checks the state of the keyboard and performs appropriate actions.
        private void CheckGameKeyboard(double elapsedTime)
        {
            //Keyboard buttons.
            //Pressing enter makes the player start move.
            if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter)) //&& !level.Ninja.Moving)
            {
                if(level.IsNinja)
                    level.Ninja.Moving = true;
                if(level.IsRecon)
                    level.Recon.Moving = true;
                if(level.IsAssault)
                    level.Assault.Moving = true;
            }

            //Reset
            if (keyboard.IsKeyDown(Keys.R) && keyboardPrev.IsKeyUp(Keys.R))
            {
                this.Fail();
            }

            //Move view Up.
            if (keyboard.IsKeyDown(Keys.W) || keyboard.IsKeyDown(Keys.Up))
            {
                camY += (int)(150 * (float)elapsedTime);
            }

            //Move view Right.
            if (keyboard.IsKeyDown(Keys.D) || keyboard.IsKeyDown(Keys.Right))
            {
                camX -= (int)(150 * (float)elapsedTime);
            }

            //Move view Down.
            if (keyboard.IsKeyDown(Keys.S) || keyboard.IsKeyDown(Keys.Down))
            {
                camY -= (int)(150 * (float)elapsedTime);
            }

            //Move view Left.
            if (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Left))
            {
                camX += (int)(150 * (float)elapsedTime);
            }

            //Fast Forward with Space bar
            if (keyboard.IsKeyDown(Keys.Space) && keyboardPrev.IsKeyUp(Keys.Space))
            {
                FastForward();
            }

            //Next Level cheat, for our personal testing purposes only!
            if (keyboard.IsKeyDown(Keys.N) && keyboardPrev.IsKeyUp(Keys.N))
            {
                current = GameState.LevelComp;
            }
        }

        //Increments the current level.
        private void IncrementLevel()
        {
            indexLevel++;
            isGuard = false;
            activeGuards.Clear();
            guardCount = guardAmount.Dequeue();

            try
            {
                level = levels[indexLevel];
                camXCenter = (GraphicsDevice.Viewport.Width - level.Width * GlobalVar.TILESIZE) / 2;
                camYCenter = (GraphicsDevice.Viewport.Height - level.Height * GlobalVar.TILESIZE) / 2;
                this.CenterCamera();

                if (guardCount > 0)  //Checks for guards in this level
                {
                    isGuard = true;  //Sets the guards to active

                    for (int i = 0; i < guardCount; i++)
                    {
                        activeGuards.Add(guards[guardIndex]);
                        activeGuards[i].Turn(activeGuards[i].Direction);
                        guardIndex = guardIndex + 1;
                    }
                }
            }
            catch(Exception)
            {
                current = GameState.Title;
            }
        }

        //Centers the camera. Is called by Fail and Increment Level.
        private void CenterCamera()
        {
            camX = camXCenter;  //Reset view.
            camY = camYCenter;  //Reset view.
        }


        public void GuardFail() //When the player failed due to a guard
        {
            if (alerted == false)  //For alert sound effect
            {
                MediaPlayer.Play(effects[0]);
                alerted = true;
            }


            if (level.IsNinja)  //if active Ninja
            {
                level.Ninja.Moving = false;
            }

            if (level.IsAssault) //if active Assault
            {
                level.Assault.Moving = false;
            }

            if (level.IsRecon) //if active Recon
            {
                level.Recon.Moving = false;
            }


            if (timer >= 2.0f) //2 second pause
            {
                for (int i = 0; i < guardCount; i++) //Resets all guards
                {
                    activeGuards[i].Reset();
                }
                this.Fail();
                attacked = false;
                alerted = false;
                timer = 0;
            }
        }


        //Updates all guards.
        private void UpdateGuards(double gameTime)
        {
            //Important Guard Updates
            if (isGuard == true)
            {
                for (int i = 0; i < guardCount; i++)
                {
                    activeGuards[i].Move((int)(100 * GlobalVar.TILESIZE * (float)gameTime / GlobalVar.SpeedLevel));
                }


                for (int i = 0; i < guardCount; i++)
                {
                    if (level.IsNinja) //if active Ninja is attacked
                    {
                        if (activeGuards[i].Attack(level.Ninja) == true)
                        {
                            attacked = true;
                        }
                    }

                    if (level.IsAssault) //if active Assault is attacked
                    {
                        if (activeGuards[i].Attack(level.Assault) == true)
                        {
                            attacked = true;
                        }
                    }

                    if (level.IsRecon) //if active Recon is attacked
                    {
                        if (activeGuards[i].Attack(level.Recon) == true)
                        {
                            attacked = true;
                        }
                    }
                }


                //Guards Wall detection
                for (int j = 0; j < level.Height; ++j)
                {
                    for (int i = 0; i < level.Width; ++i)
                    {
                        //Check if enemy collided with it.
                        for (int x = 0; x < guardCount; x++)
                        {
                            if (level.GetGamePiece(i, j).CheckCollision(activeGuards[x]))
                            {
                                //activeGuards[x].Patrol();

                                switch (level.GetGamePiece(i, j).PieceState)
                                {
                                    case PieceState.Floor:
                                        break;
                                    case PieceState.Wall:
                                        activeGuards[x].Patrol();
                                        break;
                                    case PieceState.Goal:
                                        activeGuards[x].Patrol();
                                        break;
                                }
                                if (level.GetGamePiece(i, j).PositionRect.Center == activeGuards[x].PositionRect.Center
                                    && activeGuards[x].Difficulty == 1)
                                {
                                    switch (level.GetGamePiece(i, j).PieceState)
                                    {
                                        case PieceState.North:
                                            activeGuards[x].Rotate(2);
                                            break;
                                        case PieceState.East:
                                            activeGuards[x].Rotate(3);
                                            break;
                                        case PieceState.South:
                                            activeGuards[x].Rotate(0);
                                            break;
                                        case PieceState.West:
                                            activeGuards[x].Rotate(1);
                                            break;
                                        case PieceState.SpecialNorth:
                                            activeGuards[x].Turn(2);
                                            level.GetGamePiece(i, j).IncrementType();
                                            break;
                                        case PieceState.SpecialEast:
                                            activeGuards[x].Turn(3);
                                            level.GetGamePiece(i, j).IncrementType();
                                            break;
                                        case PieceState.SpecialSouth:
                                            activeGuards[x].Turn(0);
                                            level.GetGamePiece(i, j).IncrementType();
                                            break;
                                        case PieceState.SpecialWest:
                                            activeGuards[x].Turn(1);
                                            level.GetGamePiece(i, j).IncrementType();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
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

            menuPos = headerFont.MeasureString(title);
            gameOverPos = headerFont.MeasureString("GAME OVER");
            levelCompPos = headerFont.MeasureString("LEVEL COMPLETE");

            music = new List<Song>();
            music.Add(this.Content.Load<Song>("ThemeV2"));

            effects = new List<Song>();
            effects.Add(this.Content.Load<Song>("Alert"));

            introVideo = this.Content.Load<Video>("Intro");

            //Single indicator.
            List<Texture2D> indicatorTextureTemp = new List<Texture2D>();
            indicatorTextureTemp.Add(this.Content.Load<Texture2D>("Indicator"));
            indicator = new Indicator(0, 0, indicatorTextureTemp);

            // Order tile textures in order that they appear when clicked.
            tileTextures = new List<Texture2D>();
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Tile_1"));
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Up"));
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Right"));
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Down"));
            tileTextures.Add(this.Content.Load<Texture2D>("Default_Left"));

            //For rotate tiles
            specialTileTextures = new List<Texture2D>();            
            specialTileTextures.Add(this.Content.Load<Texture2D>("Rotate_Clockwise_Up_1"));
            specialTileTextures.Add(this.Content.Load<Texture2D>("Rotate_Clockwise_Right_1"));
            specialTileTextures.Add(this.Content.Load<Texture2D>("Rotate_Clockwise_Down_1"));
            specialTileTextures.Add(this.Content.Load<Texture2D>("Rotate_Clockwise_Left_1"));
            specialTileTextures.Add(this.Content.Load<Texture2D>("Rotate_Counter_Clockwise_Up_1"));
            specialTileTextures.Add(this.Content.Load<Texture2D>("Rotate_Counter_Clockwise_Left_1"));
            specialTileTextures.Add(this.Content.Load<Texture2D>("Rotate_Counter_Clockwise_Down_1"));
            specialTileTextures.Add(this.Content.Load<Texture2D>("Rotate_Counter_Clockwise_Right_1"));

            //Single Wall texture.
            wallTextures = new List<Texture2D>();
            wallTextures.Add(this.Content.Load<Texture2D>("Default_Wall_1"));

            //Single goal texture.
            goalTextures = new List<Texture2D>();
            goalTextures.Add(this.Content.Load<Texture2D>("Default_Open_Ladder"));

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

            guardTextures = new List<Texture2D>();
            guardTextures.Add(this.Content.Load<Texture2D>("Default_Guard"));
            guardTextures.Add(this.Content.Load<Texture2D>("!"));

            for (int i = 1; i <= GlobalVar.LevelCount; ++i)
            {
                levels.Add(this.LoadLevel(i));
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
               //Intro Animation
                case GameState.Intro:
                    //Waits total lenght of video (in this case, 6 sec)
                    if (gameTime.TotalGameTime.Seconds < 6)
                    {
                        videoPlayer.Play(introVideo);
                        videoPlayer.IsLooped = false;
                    }
                    else
                    {
                        videoPlayer.Stop();
                        current = GameState.Title;
                    }

                    // Can skip Intro Animation
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter)) //Press enter to play.
                    {
                        videoPlayer.IsLooped = false;
                        videoPlayer.Stop();
                        current = GameState.Title;
                    }
                    break;
                
                
                //TITLE SCREEN
                case GameState.Title:
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter)) //Press enter to play.
                    {
                        //Play sound. Do this only to type change.
                        //MediaPlayer.Play(music[0]);

                        string path = Directory.GetCurrentDirectory() + "/../../../../../../Colors.txt";

                        //read the file
                        StreamReader reader = new StreamReader(path);

                        string[] stringColors = reader.ReadLine().Split(',');

                        GlobalVar.ColorsSplit = new Int16[stringColors.Length];

                        //match the values
                        for (int i = 0; i < stringColors.Length; i++)
                        {
                            GlobalVar.ColorsSplit[i] = Int16.Parse(stringColors[i]);
                        }

                        reader.Close();

                        //First level.
                        this.IncrementLevel();

                        current = GameState.Playing;
                    }
                    break;

                //PLAYING GAME
                case GameState.Playing:
                    this.CheckGameMouse();
                    this.CheckGameKeyboard(gameTime.ElapsedGameTime.TotalSeconds);

                    //Move Player
                    if (level.IsNinja)
                    {
                        level.Ninja.Move((int)(
                            100 * 
                            GlobalVar.TILESIZE *
                            (float)gameTime.ElapsedGameTime.TotalSeconds / 
                            GlobalVar.SpeedLevel));
                    }

                    //Use Ninja Ability
                    /*if ((level.IsNinja && keyboard.IsKeyDown(Keys.NumPad1) && keyboardPrev.IsKeyUp(Keys.NumPad1)) || level.Ninja.AbilityActive)
                    {
                        level.Ninja.Ability();
                    }*/

                    //Rotate Ninja sword if the Ability is active
                    /*if (level.Ninja.AbilityActive)
                    {
                        level.Ninja.ThisGear.Rotate((float)(gameTime.ElapsedGameTime.TotalSeconds / GlobalVar.SpeedLevel));
                    }*/
                     

                    //Move Recon
                    if (level.IsRecon)
                    {
                        level.Recon.Move((int)(
                            100 *
                            GlobalVar.TILESIZE *
                            (float)gameTime.ElapsedGameTime.TotalSeconds /
                            GlobalVar.SpeedLevel));
                    }

                    //Move Assault
                    if (level.IsAssault)
                    {
                        //Console.WriteLine("LOL!");
                        level.Assault.Move((int)(
                            100 * 
                            GlobalVar.TILESIZE *
                            (float)gameTime.ElapsedGameTime.TotalSeconds /
                            GlobalVar.SpeedLevel));
                    }

                    this.UpdateGuards(gameTime.ElapsedGameTime.TotalSeconds);  //Updates and checks guards

                    if (attacked == true) //Attacked by a guard
                    {
                        timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        this.GuardFail();
                    }
                    
                    //Gonna have to redo this for multiple classes                    
                    //For all gamepieces in level grid, check for direcion changes or collisions.
                    switch(level.CheckCollisions())
                    {
                        case CollisionState.Goal:
                            if (level.GetMoneyCount() == 0 || GlobalVar.ParCount <= level.Par)
                            {
                                current = GameState.LevelComp;
                            }
                            else
                            {
                                current = GameState.LevelFail;
                            }
                            break;
                        case CollisionState.Fail:
                            for (int i = 0; i < guardCount; i++)
                            {
                                activeGuards[i].Reset();
                            }
                            this.Fail();
                            break;
                    }
                    break;

                //LEVEL COMPLETE
                case GameState.LevelComp:
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter))
                    {
                        //Increment level
                        this.Success();
                        this.IncrementLevel();

                        current = GameState.Playing;
                    }
                    break;

                //LEVEL INCOMPLETE
                case GameState.LevelFail:
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter))
                    {
                        //Increment level
                        this.Success();

                        current = GameState.Playing;
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
                //Intro Animation
                case GameState.Intro:
                    spriteBatch.Draw(videoPlayer.GetTexture(), new Rectangle(0, 0, introVideo.Width, introVideo.Height), Color.White);
                    break;
                               
                //TITLE SCREEN
                case GameState.Title:
                    //Draw Title
                    spriteBatch.DrawString(headerFont,
                        title,
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

                //PLAYING
                case GameState.Playing:
                    //Draw Par UI Element.
                    if (GlobalVar.ParCount < level.Par || GlobalVar.ParCount < 1)
                    {
                        spriteBatch.DrawString(menuFont,
                            String.Format("Par: {0} of {1}", GlobalVar.ParCount, level.Par),
                            new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                            Color.White);
                    }
                    //Draw maxed out Par UI Element. Ternary expression stops display from going above par.
                    else
                    {
                        spriteBatch.DrawString(menuFont,
                            String.Format("Par: {0} of {1} (PAR REACHED)", GlobalVar.ParCount, level.Par),
                            new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                            Color.White);
                    }
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
                    level.Draw(spriteBatch, camX, camY);
                    if(level.IsNinja)
                        level.Ninja.Draw(spriteBatch, camX, camY);
                    if (level.IsRecon)
                        level.Recon.Draw(spriteBatch, camX, camY);
                    if(level.IsAssault)
                        level.Assault.Draw(spriteBatch, camX, camY);
                    /*if (level.Ninja.AbilityActive)
                        level.Ninja.ThisGear.Draw(spriteBatch, camX, camY);*/
                    indicator.Draw(spriteBatch, camX, camY);

                    if (isGuard == true) //Guard in level?
                    {
                        for (int i = 0; i < guardCount; i++)  //Draw all guards
                        {
                            activeGuards[i].Draw(spriteBatch, camX, camY);
                        }
                    }
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
                    //Draw Par
                    spriteBatch.DrawString(menuFont,
                        String.Format("Par: {0} of {1}", GlobalVar.ParCount, level.Par),
                        new Vector2(25, 125),
                        Color.White);
                    //Draw Press Enter Prompt
                    spriteBatch.DrawString(menuFont,
                        "Press enter to continue.",
                        new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                        Color.White);
                    break;

                //Level Complete feedback screen.
                case GameState.LevelFail:
                    //Draw LEVEL COMPLETE
                    spriteBatch.DrawString(headerFont,
                        "LEVEL INCOMPLETE",
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
                    //Draw Par
                    spriteBatch.DrawString(menuFont,
                        String.Format("Par: {0} of {1}", GlobalVar.ParCount, level.Par),
                        new Vector2(25, 125),
                        Color.White);
                    //Draw Press Enter Prompt
                    spriteBatch.DrawString(menuFont,
                        "Press enter to replay level.",
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
