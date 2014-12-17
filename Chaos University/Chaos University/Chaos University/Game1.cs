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
        Level titleLevel;           //Title level.
        List<Level> levels;         //List of levels.
        Tutorial tutorial;          //Tutorial Messages.
        
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
        List<int> guardAmount;     //Also tells amount of guards
        bool attacked;              //Guard attacked player
        Indicator indicator;        //Indicator of currently selected tile.
        float timer;                //Short pause
        float timer2;               //Long pause

        //Boss stuff.
        Boss bulldozer;

        //Variables for preventing stuck
        int currX;
        int currY;


        //Special Ability Stuff
        bool usedNinja;
        bool usedAssault;
        bool usedRecon;

        //Sound effect stuff
        bool taunt;
        bool alerted;               //Alert went off
        bool stun;

        //Other things.
        Vector2 menuPos;            //Position of the menu header
        Vector2 gameOverPos;        //Position of the game over screen.
        Vector2 levelCompPos;       //Position of the level complete screen.
        SpriteFont menuFont;        //Font of menu text
        SpriteFont headerFont;      //Font of header text
        SpriteFont hudFont;
        int camX;                   //X offset of view.
        int camY;                   //Y offset of view.
        int camXCenter;             //Current X center position of view.
        int camYCenter;             //Current Y center position of view.
        string title;
        bool fastActive;
        PieceState temp;
        PieceState temp2;
        int tauntDir;
        bool stunned;
        bool flip;
        
        //Textures
        List<Texture2D> tileTextures;
        List<Texture2D> wallTextures;
        List<Texture2D> goalTextures;
        List<Texture2D> playerTextures;
        List<Texture2D> guardTextures;
        List<Texture2D> moneyTextures;
        List<Texture2D> specialTileTextures;
        List<Texture2D> abilityTextures;
        List<Texture2D> bossTextures;
        Texture2D hud;

        //Songs
        List<Song> music;
        List<SoundEffect> effects;

        //Videos
        Video introVideo;
        VideoPlayer videoPlayer;

        Video creditsVideo;
        int startTime;

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
            guardAmount = new List<int>();
            title = "Team Tyro";

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
            flip = false;
            timer = 0;
            timer2 = 0;
            alerted = false;
            fastActive = false;
            stunned = false;
            stun = false;
            tauntDir = 0;

            usedNinja = false;
            usedAssault = false;
            usedRecon = false;

            taunt = false;
            temp = PieceState.Floor;
            temp2 = PieceState.Floor;

            startTime = 0;

            this.LoadColors();

            base.Initialize();
        }

        // Loads a new level and return it. Requires integer for specific file name.
        private Level LoadLevel(int levelNum, string levelType)
        {
            //concatanates "LevelMap" and passed int to find map file.
            string file = "LevelMap" + levelType + levelNum + ".txt";
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
                while((line = input.ReadLine()) != null) //LEVEL BUILDER
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
                            //2 = Floor North.
                            case '2':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures,
                                    PieceState.North));
                                break;
                            //3 = Floor East.
                            case '3':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures,
                                    PieceState.East));
                                break;
                            //4 = Floor South.
                            case '4':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures,
                                    PieceState.South));
                                break;
                            //5 = Floor West.
                            case '5':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures,
                                    PieceState.West));
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
                                newLevel.Ninja.GearTextures.Add(abilityTextures[0]);
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
                                newLevel.Recon.GearTextures.Add(abilityTextures[2]);
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
                                newLevel.Assault.GearTextures.Add(abilityTextures[1]);
                                break;
                            //X = Guard Difficulty 0  -- Patrol and ignore
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
                            //Y = Guard Difficulty 1 -- Patrol and intercept
                            case 'Y':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                guard = new Enemy(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    guardDirs.Dequeue(),
                                    guardTextures);
                                guard.Difficulty = 1;
                                guards.Add(guard);
                                guardCount = guardCount + 1;
                                break;
                            //Z = Boss -- reserved for final boss
                            case 'Z':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures));
                                bulldozer = new Boss(columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE, 400, 200, 2, bossTextures);
                                newLevel.IsBoss = true;
                                //newLevel.StartBoss = newLevel.Boss.PositionRect;
                                
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

                guardAmount.Add(guardCount); //Sets a guard count for each stage
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

        // Loads a new level and returns it. Does not affect instance variables of Game1.
        private Level LoadUnobtrusiveLevel(int levelNum, string levelType)
        {
            //concatanates "LevelMap" and passed int to find map file.
            string file = "LevelMap" + levelType + levelNum + ".txt";
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
                foreach (char dir in input.ReadLine())
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
                while ((line = input.ReadLine()) != null) //LEVEL BUILDER
                {
                    columnNumber = 0;
                    foreach (char block in line)
                    {
                        switch (block)
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
                            //2 = Floor North.
                            case '2':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures,
                                    PieceState.North));
                                break;
                            //3 = Floor East.
                            case '3':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures,
                                    PieceState.East));
                                break;
                            //4 = Floor South.
                            case '4':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures,
                                    PieceState.South));
                                break;
                            //5 = Floor West.
                            case '5':
                                newLevel.SetTile(columnNumber, lineNumber, new Tile(
                                    columnNumber * GlobalVar.TILESIZE,
                                    lineNumber * GlobalVar.TILESIZE,
                                    tileTextures,
                                    PieceState.West));
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
                                newLevel.Ninja.GearTextures.Add(abilityTextures[0]);
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
                                newLevel.Recon.GearTextures.Add(abilityTextures[2]);
                                break;
                        }
                        columnNumber++;
                    }
                    lineNumber++;
                }

                input.Close();
            }
            //Close if File Not Found. (Change to send an error message?)
            catch (FileNotFoundException)
            {
                this.Exit();
            }
            //Close if any other odd Exception.
            catch (Exception)
            {
                this.Exit();
            }

            return newLevel;
        }

        // Loads character colors
        private void LoadColors()
        {
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
                level.Ninja.Active = true;
            }

            //Recon
            if (level.IsRecon)
            {
                level.Recon.PositionRect = level.StartRecon;    //Reset Player Location.
                level.Recon.turn(level.RotRecon);               //Reset Player Direction.
                level.Recon.Moving = false;                     //Halt player.
                level.Recon.Active = true;
            }

            //Assault
            if (level.IsAssault)
            {
                level.Assault.PositionRect = level.StartAssault;    //Reset Player Location.
                level.Assault.turn(level.RotAssault);               //Reset Player Direction.
                level.Assault.Moving = false;                       //Halt player.
                level.Assault.Active = true;
            }

            for (int i = 0; i < guardCount; i++)                //Reset all guards
            {
                activeGuards[i].Reset();
            }
            

            usedNinja = false;
            usedAssault = false;
            usedRecon = false;
            stunned = false;
            timer2 = 0;
            GlobalVar.ParCount = 0;                             //Reset par for player.
            level.ActivateMoney();                              //Reset monies.
            indicator.Active = false;                           //Reset indicator.
            clickPrevX = -1;                                    //Reset clickPrev at a nonexistent index.
            clickPrevY = -1;                                    //Reset clickPrev at a nonexistent index.
            this.CenterCamera();
            GlobalVar.SpeedLevel = 50;
            fastActive = false;
            level.ResetGate();                                  //Reset ladder sprite to close
            if (level.IsBoss)
            {
                bulldozer.Moving = false;
                bulldozer.ResetBoss();                              //Reset boss POS.
            }

            //Reset all direction tiles.
            Type type = typeof(Tile);
            for (int b = 0; b < level.Height; ++b)
            {
                for (int a = 0; a < level.Width; ++a)
                {
                    if (level.GetGamePiece(a, b).PieceState == PieceState.SpecialNorth ||
                        level.GetGamePiece(a, b).PieceState == PieceState.SpecialEast  ||
                        level.GetGamePiece(a, b).PieceState == PieceState.SpecialSouth ||
                        level.GetGamePiece(a, b).PieceState == PieceState.SpecialWest)
                    {
                        level.GetGamePiece(a, b).ReturnStartingDirection();
                    }
                    else if (level.GetGamePiece(a, b).GetType() == type)
                    {
                        level.GetGamePiece(a, b).ResetType();
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

        //Speeds up gameplay or returns to normal --  must be activated before movement begins, but ended any time
        private void FastForward()
        {
            if (level.IsNinja)
            {
                if (fastActive == false && level.Ninja.Moving == false)
                {
                    GlobalVar.SpeedLevel = 30;
                    fastActive = true;
                }
                else if (fastActive == true)
                {
                    GlobalVar.SpeedLevel = 50;
                    fastActive = false;
                }
            }

            if (level.IsAssault)
            {
                if (fastActive == false && level.Assault.Moving == false)
                {
                    GlobalVar.SpeedLevel = 30;
                    fastActive = true;
                }
                else if (fastActive == true)
                {
                    GlobalVar.SpeedLevel = 50;
                    fastActive = false;
                }
            }

            if (level.IsRecon)
            {
                if (fastActive == false && level.Recon.Moving == false)
                {
                    GlobalVar.SpeedLevel = 30;
                    fastActive = true;
                }
                else if (fastActive == true)
                {
                    GlobalVar.SpeedLevel = 50;
                    fastActive = false;
                }
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
                if (level.IsBoss)
                    bulldozer.Moving = true;
            }

            //Use Ninja Ability -- Sword
            if (level.IsNinja)
            {
                if ((level.IsNinja && keyboard.IsKeyDown(Keys.D1) && keyboardPrev.IsKeyUp(Keys.D1)) && usedNinja == false || level.Ninja.AbilityActive)
                {
                    level.Ninja.Ability();
                }
            }

            //Use Assault Ability -- Taunt
            if (level.IsAssault)
            {
                if ((level.IsAssault && keyboard.IsKeyDown(Keys.D2) && keyboardPrev.IsKeyUp(Keys.D2)) || level.Assault.AbilityActive)
                {
                    AssaultTaunt();
                }
            }

            //Use Recon Ability -- Tazer
            if (level.IsRecon)
            {
                if ((level.IsRecon && keyboard.IsKeyDown(Keys.D3) && keyboardPrev.IsKeyUp(Keys.D3)))
                {
                    ReconStun();
                }
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
        }

        //Increments the current level.
        private void IncrementLevel()
        {
            usedNinja = false;
            usedAssault = false;
            usedRecon = false;

            indexLevel++;
            isGuard = false;            
            activeGuards.Clear();
            if (indexLevel < levels.Count)
            {
                guardCount = guardAmount[indexLevel];
            }

            level = levels[indexLevel];
            camXCenter = (GraphicsDevice.Viewport.Width - level.Width * GlobalVar.TILESIZE) / 2;
            camYCenter = (GraphicsDevice.Viewport.Height - level.Height * GlobalVar.TILESIZE) / 2;

            //Temporary camera fix for level 4.
            if (indexLevel == 3)
            {
                camYCenter = 125;
            }

            this.CenterCamera();

            if (guardCount > 0)  //Checks for guards in this level
            {
                isGuard = true;  //Sets the guards to active

                for (int i = 0; i < guardCount; i++)
                {
                    activeGuards.Add(guards[guardIndex]);
                    activeGuards[i].Turn(activeGuards[i].Direction);
                    guardIndex++;
                }
            }
        }

        //Initializes the title screen. Reads colors for player characters, sets up title level.
        private void InitializeTitleScreen()
        {
            camXCenter = (GraphicsDevice.Viewport.Width - titleLevel.Width * GlobalVar.TILESIZE) / 2;
            camYCenter = (GraphicsDevice.Viewport.Height - titleLevel.Height * GlobalVar.TILESIZE) / 2;
            this.CenterCamera();

            titleLevel.Ninja.Moving = true;
            titleLevel.Recon.Moving = true;
            current = GameState.Title;
        }

        //Centers the camera. Is called by Fail and Increment Level.
        private void CenterCamera()
        {
            camX = camXCenter;  //Reset view.
            camY = camYCenter;  //Reset view.
        }

        public void GuardStunned() //After a successful ReconStun
        {
            usedRecon = true;

            if (stun == false) //For stun sound effect
            {
                effects[2].Play();
                stun = true;
            }


            if (timer2 < 1.0f) //1 second pause
            {
                level.Recon.Ability();
                flip = false;
                level.Recon.Moving = false;
                guard.Stop();
            }

            if (timer2 >= 1.0f && flip == false) //After 1 second pause
            {
                level.Recon.EndAbility();

                switch (level.Recon.Direction)
                {
                    case 0:
                        level.Recon.turn(2);
                        break;
                    case 1:
                        level.Recon.turn(3);
                        break;
                    case 2:
                        level.Recon.turn(0);
                        break;
                    case 3:
                        level.Recon.turn(1);
                        break;
                }

                flip = true;
                level.Recon.Moving = true;
            }

            if (timer2 >= 5.0f) //5 second pause
            {
                guard.Detected = false;
                guard.StartMove(); //Stationary guard will start moving
                stunned = false;
                timer2 = 0;
            }
        }

        public void ReconStun() //Recon special move, Tazer -- initial code
        {
            if (level.IsRecon == true && usedRecon == false) //if active Recon
            {
                stun = false;

                for (int x = 0; x < guardCount; x++)
                {
                    switch (level.Recon.Direction) //Checks direction for stun effect to activate
                    {
                        case 0:
                            if (activeGuards[x].PositionRect.X > level.Recon.PositionRect.X - (GlobalVar.TILESIZE /2)
                                && (activeGuards[x].PositionRect.X < level.Recon.PositionRect.X + (GlobalVar.TILESIZE /2))
                                && ((activeGuards[x].PositionRect.Y < level.Recon.PositionRect.Y))
                                && (activeGuards[x].PositionRect.Y > level.Recon.PositionRect.Y - (GlobalVar.TILESIZE * 2)))
                            {
                                activeGuards[x].Confused = true;
                                guard = activeGuards[x];
                                stunned = true;
                            }
                            break;
                        case 1:
                            if ((activeGuards[x].PositionRect.Y > level.Recon.PositionRect.Y - (GlobalVar.TILESIZE / 2))
                                && (activeGuards[x].PositionRect.Y < level.Recon.PositionRect.Y + (GlobalVar.TILESIZE / 2))
                                && ((activeGuards[x].PositionRect.X > level.Recon.PositionRect.X))
                                && (activeGuards[x].PositionRect.X < level.Recon.PositionRect.X + (GlobalVar.TILESIZE * 2)))
                            {
                                activeGuards[x].Confused = true;
                                guard = activeGuards[x];
                                stunned = true;
                            }
                            break;
                        case 2:
                            if (activeGuards[x].PositionRect.X > level.Recon.PositionRect.X - (GlobalVar.TILESIZE / 2)
                                && (activeGuards[x].PositionRect.X < level.Recon.PositionRect.X + (GlobalVar.TILESIZE / 2))
                                && ((activeGuards[x].PositionRect.Y > level.Recon.PositionRect.Y))
                                && (activeGuards[x].PositionRect.Y < level.Recon.PositionRect.Y + (GlobalVar.TILESIZE * 2)))
                            {
                                activeGuards[x].Confused = true;
                                guard = activeGuards[x];
                                stunned = true;
                            }
                            break;
                        case 3:
                            if ((activeGuards[x].PositionRect.Y > level.Recon.PositionRect.Y - (GlobalVar.TILESIZE / 2))
                                && (activeGuards[x].PositionRect.Y < level.Recon.PositionRect.Y + (GlobalVar.TILESIZE / 2))
                                && ((activeGuards[x].PositionRect.X < level.Recon.PositionRect.X))
                                && (activeGuards[x].PositionRect.X > level.Recon.PositionRect.X - (GlobalVar.TILESIZE * 2)))
                            {
                                activeGuards[x].Confused = true;
                                guard = activeGuards[x];
                                stunned = true;
                            }
                            break;
                    }
                }
            }
        }

        public void AssaultTaunt() //Assault special move, Taunt -- Working, but actually isnt in all cases
        {
            if (level.IsAssault == true && usedAssault == false) //if active Assault
            {
                if (taunt == false) //For taunt sound effect
                {
                    effects[1].Play();
                    taunt = true;
                }

                taunt = false;
                level.Assault.Ability();

                for (int x = 0; x < guardCount; x++)
                {
                    //for (int j = 0; j < level.Height; ++j)
                    //{
                    //    for (int i = 0; i < level.Width; ++i)
                    //    {
                            if ((activeGuards[x].PositionRect.X < level.Assault.PositionRect.X) //Guards to the left of Assault
                                && ((activeGuards[x].PositionRect.Y < level.Assault.PositionRect.Y + (GlobalVar.TILESIZE))
                                && (activeGuards[x].PositionRect.Y > level.Assault.PositionRect.Y - (GlobalVar.TILESIZE))))
                            {
                                currX = (activeGuards[x].PositionRect.X - camX) / GlobalVar.TILESIZE;
                                currY = (activeGuards[x].PositionRect.Y - camY) / GlobalVar.TILESIZE;

                                activeGuards[x].Taunted = true;
                                activeGuards[x].Confused = true;
                                usedAssault = true;
                                activeGuards[x].StartMove();

                                switch (activeGuards[x].Direction)
                                {
                                    case 0:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 3, currY + 1).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.East;
                                        if (level.GetGamePiece(currX + 3, currY + 1).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 3, currY + 1).PieceState = PieceState.OffEast;
                                        }
                                        break;
                                    case 1:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 2, currY).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.East;
                                        if (level.GetGamePiece(currX + 2, currY).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 2, currY).PieceState = PieceState.OffEast;
                                        }
                                        break;
                                    case 2:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 3, currY - 1).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.East;
                                        if (level.GetGamePiece(currX + 3, currY - 1).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 3, currY - 1).PieceState = PieceState.OffEast;
                                        }
                                        break;
                                    case 3:
                                        activeGuards[x].Taunted = false;
                                        activeGuards[x].Confused = false;
                                        break;
                                }
 
                            }
                            else if ((activeGuards[x].PositionRect.X > level.Assault.PositionRect.X) //Guards to the right of Assault
                                && ((activeGuards[x].PositionRect.Y < level.Assault.PositionRect.Y + (GlobalVar.TILESIZE))
                                && (activeGuards[x].PositionRect.Y > level.Assault.PositionRect.Y - (GlobalVar.TILESIZE))))
                            {
                                currX = (activeGuards[x].PositionRect.X - camX) / GlobalVar.TILESIZE;
                                currY = (activeGuards[x].PositionRect.Y - camY) / GlobalVar.TILESIZE;

                                activeGuards[x].Taunted = true;
                                activeGuards[x].Confused = true;
                                usedAssault = true;
                                activeGuards[x].StartMove();

                                switch (activeGuards[x].Direction)
                                {
                                    case 0:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 3, currY + 1).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.West;
                                        if (level.GetGamePiece(currX + 3, currY + 1).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 3, currY + 1).PieceState = PieceState.OffWest;
                                        }
                                        break;
                                    case 1:
                                        activeGuards[x].Taunted = false;
                                        activeGuards[x].Confused = false;
                                        break;
                                    case 2:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 3, currY - 1).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.West;
                                        if (level.GetGamePiece(currX + 3, currY - 1).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 3, currY - 1).PieceState = PieceState.OffWest;
                                        }
                                        break;
                                    case 3:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 4, currY).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.West;
                                        if (level.GetGamePiece(currX + 4, currY).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 4, currY).PieceState = PieceState.OffWest;
                                        }
                                        break;
                                }

                            }
                            else if ((activeGuards[x].PositionRect.Y > level.Assault.PositionRect.Y) //Guards below Assault
                                && ((activeGuards[x].PositionRect.X < level.Assault.PositionRect.X + (GlobalVar.TILESIZE))
                                && (activeGuards[x].PositionRect.X > level.Assault.PositionRect.X - (GlobalVar.TILESIZE))))
                            {
                                currX = (activeGuards[x].PositionRect.X - camX) / GlobalVar.TILESIZE;
                                currY = (activeGuards[x].PositionRect.Y - camY) / GlobalVar.TILESIZE;

                                activeGuards[x].Taunted = true;
                                activeGuards[x].Confused = true;
                                usedAssault = true;
                                activeGuards[x].StartMove();

                                switch (activeGuards[x].Direction)
                                {
                                    case 0:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 3, currY + 1).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.North;
                                        if (level.GetGamePiece(currX + 3, currY + 1).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 3, currY + 1).PieceState = PieceState.OffNorth;
                                        }
                                        break;
                                    case 1:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 2, currY).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.North;
                                        if (level.GetGamePiece(currX + 2, currY).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 2, currY).PieceState = PieceState.OffNorth;
                                        }
                                        break;
                                    case 2:
                                        activeGuards[x].Taunted = false;
                                        activeGuards[x].Confused = false;
                                        break;
                                    case 3:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 4, currY).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.North;
                                        if (level.GetGamePiece(currX + 4, currY).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 4, currY).PieceState = PieceState.OffNorth;
                                        }
                                        break;
                                }


                            }
                            else if ((activeGuards[x].PositionRect.Y < level.Assault.PositionRect.Y) //Guards above Assault
                                && ((activeGuards[x].PositionRect.X < level.Assault.PositionRect.X + (GlobalVar.TILESIZE))
                                && (activeGuards[x].PositionRect.X > level.Assault.PositionRect.X - (GlobalVar.TILESIZE))))
                            {
                                currX = (activeGuards[x].PositionRect.X - camX) / GlobalVar.TILESIZE;
                                currY = (activeGuards[x].PositionRect.Y - camY) / GlobalVar.TILESIZE;

                                activeGuards[x].Taunted = true;
                                activeGuards[x].Confused = true;
                                usedAssault = true;
                                activeGuards[x].StartMove();

                                switch (activeGuards[x].Direction)
                                {
                                    case 0:
                                        activeGuards[x].Taunted = false;
                                        activeGuards[x].Confused = false;
                                        break;
                                    case 1:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 2, currY).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.South;
                                        if (level.GetGamePiece(currX + 2, currY).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 2, currY).PieceState = PieceState.OffSouth;
                                        }
                                        break;
                                    case 2:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 3, currY - 1).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.South;
                                        if (level.GetGamePiece(currX + 3, currY - 1).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 3, currY - 1).PieceState = PieceState.OffSouth;
                                        }
                                        break;
                                    case 3:
                                        tauntDir = activeGuards[x].Direction;
                                        temp = level.GetGamePiece(currX + 3, currY).PieceState;
                                        temp2 = level.GetGamePiece(currX + 4, currY).PieceState;
                                        level.GetGamePiece(currX + 3, currY).PieceState = PieceState.South;
                                        if (level.GetGamePiece(currX + 4, currY).PieceState != PieceState.Wall)
                                        {
                                            level.GetGamePiece(currX + 4, currY).PieceState = PieceState.OffSouth;
                                        }
                                        break;
                                }


                        //    }
                        //}
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
            hudFont = this.Content.Load<SpriteFont>("HudFont");

            menuPos = headerFont.MeasureString(title);
            gameOverPos = headerFont.MeasureString("GAME OVER");
            levelCompPos = headerFont.MeasureString("LEVEL COMPLETE");

            music = new List<Song>();
            music.Add(this.Content.Load<Song>("TheoreticalTheme"));

            effects = new List<SoundEffect>();
            effects.Add(this.Content.Load<SoundEffect>("AlertWav"));
            effects.Add(this.Content.Load<SoundEffect>("shout"));
            effects.Add(this.Content.Load<SoundEffect>("spark"));


            introVideo = this.Content.Load<Video>("Intro_V2");
            creditsVideo = this.Content.Load<Video>("TT_Credits");

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
            goalTextures.Add(this.Content.Load<Texture2D>("Default_Close_Ladder"));
            goalTextures.Add(this.Content.Load<Texture2D>("Default_Open_Ladder"));

            //Single Money texture.
            moneyTextures = new List<Texture2D>();
            moneyTextures.Add(this.Content.Load<Texture2D>("Default_Intel"));

            //Ability Textures
            abilityTextures = new List<Texture2D>();
            abilityTextures.Add(this.Content.Load<Texture2D>("Sword"));
            abilityTextures.Add(this.Content.Load<Texture2D>("Taunt"));
            abilityTextures.Add(this.Content.Load<Texture2D>("Stun_Gun"));

            // Order player textures from lowest to highest. (Alex wishes this was a robot.)
            playerTextures = new List<Texture2D>();
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Body"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Vest"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Backpack"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Head"));
            playerTextures.Add(this.Content.Load<Texture2D>("Default_Bandana"));

            //Guard Textures.
            guardTextures = new List<Texture2D>();
            guardTextures.Add(this.Content.Load<Texture2D>("Default_Guard"));
            guardTextures.Add(this.Content.Load<Texture2D>("Default_Guard_2_New"));
            guardTextures.Add(this.Content.Load<Texture2D>("!"));
            guardTextures.Add(this.Content.Load<Texture2D>("Question_mark3"));

            //Boss Textures
            bossTextures = new List<Texture2D>();
            bossTextures.Add(this.Content.Load<Texture2D>("Tank"));
            
            //Hud Texture
            hud = this.Content.Load<Texture2D>("Status_Bar_1");

            //Tutorial messages
            tutorial = new Tutorial(GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, menuFont);

            //
            titleLevel = this.LoadUnobtrusiveLevel(1, "S");

            //Load all levels.
            for (int b = 1; b <= GlobalVar.levelsOpening; ++b) //Opening levels
            {
                levels.Add(this.LoadLevel(b, "B"));
            }

            for (int n = 1; n <= GlobalVar.levelsNinja; ++n) //Ninja levels
            {
                levels.Add(this.LoadLevel(n, "N"));
            }

            for (int r = 1; r <= GlobalVar.levelsRecon; ++r) //Recon levels
            {
                levels.Add(this.LoadLevel(r, "R"));
            }

            for (int a = 1; a <= GlobalVar.levelsAssault; ++a) //Assault levels
            {
                levels.Add(this.LoadLevel(a, "T"));
            }

            for (int f = 1; f <= GlobalVar.levelsBoss; ++f) //Boss levels
            {
                levels.Add(this.LoadLevel(f, "F"));
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

            //Press exit to leave.
            if(keyboard.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

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
                        this.InitializeTitleScreen();
                    }

                    // Can skip Intro Animation
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter)) //Press enter to play.
                    {
                        videoPlayer.IsLooped = false;
                        videoPlayer.Stop();
                        this.InitializeTitleScreen();
                    }
                    break;
                
                
                //TITLE SCREEN
                case GameState.Title:
                    titleLevel.Ninja.Move((int)(
                        100 *
                        GlobalVar.TILESIZE *
                        (float)gameTime.ElapsedGameTime.TotalSeconds /
                        GlobalVar.SpeedLevel));
                    titleLevel.Recon.Move((int)(
                        100 *
                        GlobalVar.TILESIZE *
                        (float)gameTime.ElapsedGameTime.TotalSeconds /
                        GlobalVar.SpeedLevel));
                    titleLevel.CheckCollisions();
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter)) //Press enter to play.
                    {
                        //Play sound. Do this only to type change.
                        //MediaPlayer.Play(music[0]);
                        //MediaPlayer.IsRepeating = true;

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
                    if (level.IsNinja && level.Ninja.Active)
                    {
                        level.Ninja.Move((int)(
                            100 * 
                            GlobalVar.TILESIZE *
                            (float)gameTime.ElapsedGameTime.TotalSeconds / 
                            GlobalVar.SpeedLevel));
                    }

                    //Rotate Ninja sword if the Ability is active
                    if (level.IsNinja)
                    {
                        if (level.Ninja.AbilityActive)
                        {
                            level.Ninja.ThisGear.Rotate((float)(gameTime.ElapsedGameTime.TotalSeconds / GlobalVar.SpeedLevel));
                        }
                    }

                    //Move Recon
                    if (level.IsRecon && level.Recon.Active)
                    {
                        level.Recon.Move((int)(
                            100 *
                            GlobalVar.TILESIZE *
                            (float)gameTime.ElapsedGameTime.TotalSeconds /
                            GlobalVar.SpeedLevel));
                    }

                    //Move Assault
                    if (level.IsAssault && level.Assault.Active)
                    {
                        level.Assault.Move((int)(
                            100 * 
                            GlobalVar.TILESIZE *
                            (float)gameTime.ElapsedGameTime.TotalSeconds /
                            GlobalVar.SpeedLevel));
                    }

                    //Move Boss
                    if (level.IsBoss == true)
                    {
                        if (bulldozer.Moving == true)
                        {
                            bulldozer.Move((int)(
                                100 *
                                GlobalVar.TILESIZE *
                                (float)gameTime.ElapsedGameTime.TotalSeconds /
                                GlobalVar.SpeedLevel));
                        }

                        if(bulldozer.PositionRect.Intersects(level.Ninja.PositionRect) ||
                            bulldozer.PositionRect.Intersects(level.Recon.PositionRect) ||
                            bulldozer.PositionRect.Intersects(level.Assault.PositionRect))
                        {
                            this.Fail();
                            current = GameState.LevelFail;
                        }
                            
                    }

                    this.UpdateGuards(gameTime.ElapsedGameTime.TotalSeconds);  //Updates and checks guards

                    level.OpenGate(); //Checks to see if all intel is collected

                    if (stunned == true) //Guard hit by stun
                    {
                        timer2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        GuardStunned();
                    }

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
                            if(level.CheckPlayerActivity())
                            {
                                if (level.GetMoneyCount() == 0 && (GlobalVar.ParCount <= level.Par || level.Par == 0))
                                {
                                    current = GameState.LevelComp;
                                }
                                else
                                {
                                    current = GameState.LevelFail;
                                }
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

                    if (level.IsNinja && level.Ninja.AbilityActive)
                    {
                        foreach (Enemy guard in activeGuards)
                        {
                            if (level.Ninja.ThisGear.CheckCollision(guard))
                            {
                                guard.Dead();
                                usedNinja = true;
                            }
                        }
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

                        //Last 3 levels are ignored.
                        if (indexLevel + 3 > levels.Count)
                        {
                            current = GameState.Credits;
                        }

                    }
                    break;

                //LEVEL INCOMPLETE
                case GameState.LevelFail:
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter))
                    {
                        //Increment level
                        this.Fail();
                        this.Success();

                        current = GameState.Playing;
                    }
                    break;

                case GameState.Credits:
                    //Waits total lenght of video (in this case, 6 sec)

                    indexLevel = -1;
                    guardIndex = 0;
                    guardCount = 0;

                    if (startTime == 0)
                    {
                        videoPlayer.Play(creditsVideo);
                        videoPlayer.IsLooped = false;
                        startTime = gameTime.TotalGameTime.Seconds;
                    }
                    else if (gameTime.TotalGameTime.Seconds > startTime + 20)
                    {
                        videoPlayer.Stop();
                        startTime = 0;
                        this.InitializeTitleScreen();
                    }

                    // Can skip ending animation?
                    if (keyboard.IsKeyDown(Keys.Enter) && keyboardPrev.IsKeyUp(Keys.Enter)) //Press enter to play.
                    {
                        videoPlayer.IsLooped = false;
                        videoPlayer.Stop();
                        startTime = 0;
                        this.InitializeTitleScreen();
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
                    spriteBatch.Draw(
                        videoPlayer.GetTexture(),
                        new Rectangle(
                            0,
                            -50,
                            introVideo.Width,
                            introVideo.Height),
                        Color.White);
                    break;
                               
                //TITLE SCREEN
                case GameState.Title:
                    //DRAW LEVEL
                    titleLevel.Draw(spriteBatch, camX, camY);
                    titleLevel.Ninja.Draw(spriteBatch, camX, camY);
                    titleLevel.Recon.Draw(spriteBatch, camX, camY);
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
                    //Draw Press Enter Prompt
                    spriteBatch.DrawString(menuFont,
                        "Press enter to continue.",
                        new Vector2(25, GraphicsDevice.Viewport.Height - 26),
                        Color.White);
                   
                    break;

                //PLAYING
                case GameState.Playing:
                    //DRAW LEVEL
                    level.Draw(spriteBatch, camX, camY);


                    //DRAW INDICATOR
                    indicator.Draw(spriteBatch, camX, camY);
                    
                    //DRAW GUARDS
                    if (isGuard == true) //Guard in level?
                    {
                        for (int i = 0; i < guardCount; i++)  //Draw all guards
                        {
                            activeGuards[i].Draw(spriteBatch, camX, camY);
                        }
                    }

                    if (level.IsBoss == true)
                    {
                        bulldozer.Draw(spriteBatch, camX, camY);
                    }

                    //DRAW PLAYER ABILITIES
                    if (level.IsNinja)
                    {
                        if (level.Ninja.AbilityActive)
                            level.Ninja.ThisGear.Draw(spriteBatch, camX, camY);
                    }
                    if (level.IsAssault)
                    {
                        if (level.Assault.AbilityActive)
                            level.Assault.ThisGear.Draw(spriteBatch, camX, camY);
                    }
                    if (level.IsRecon)
                    {
                        if (level.Recon.AbilityActive)
                            level.Recon.ThisGear.Draw(spriteBatch, camX, camY);
                    }

                    //DRAW PLAYERS
                    if(level.IsNinja)
                        level.Ninja.Draw(spriteBatch, camX, camY);
                    if (level.IsRecon)
                        level.Recon.Draw(spriteBatch, camX, camY);
                    if(level.IsAssault)
                        level.Assault.Draw(spriteBatch, camX, camY);

                    //Draw Tutorial Messages
                    tutorial.Draw(spriteBatch, camX, camY, indexLevel);

                    //DRAW UI
                    //Draw Hud
                    spriteBatch.Draw(
                        hud,
                        new Rectangle(
                            0,
                            GraphicsDevice.Viewport.Height - hud.Height,
                            GraphicsDevice.Viewport.Width,
                            hud.Height),
                        Color.White);
                    //Draw Par UI Element.
                    if (level.Par != 0)
                    {
                        spriteBatch.DrawString(hudFont,
                            String.Format("Par: {0} of {1}", GlobalVar.ParCount, level.Par),
                            new Vector2(25, GraphicsDevice.Viewport.Height - 35),
                            Color.White);

                        //Draw maxed out Par UI Element. Ternary expression stops display from going above par.
                        if (GlobalVar.ParCount > level.Par)
                        {
                            spriteBatch.DrawString(hudFont,
                                "OVER PAR!",
                                new Vector2(GraphicsDevice.Viewport.Width - 170, GraphicsDevice.Viewport.Height - 35),
                                Color.Red);
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

                case GameState.Credits:
                    spriteBatch.Draw(videoPlayer.GetTexture(), new Rectangle(0, 0, creditsVideo.Width, creditsVideo.Height), Color.White);
                    break;
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void GuardRemake(int gNum, int dir) //Remakes the guard for better repositioning
        {
            activeGuards[gNum] = new Enemy(
                                        activeGuards[gNum].PositionRect.X,
                                        activeGuards[gNum].PositionRect.Y,
                                        dir, guardTextures,
                                        activeGuards[gNum].Difficulty,
                                        activeGuards[gNum].InitialDirection,
                                        activeGuards[gNum].InitialX,
                                        activeGuards[gNum].InitialY,
                                        activeGuards[gNum].PrevDir);
        }

        public void GuardFail() //When the player failed due to a guard
        {
            if (alerted == false)  //For alert sound effect
            {
                effects[0].Play();
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


                //Guards detection
                for (int j = 0; j < level.Height; ++j)
                {
                    for (int i = 0; i < level.Width; ++i)
                    {
                        //Check if enemy collided with it.
                        for (int x = 0; x < guardCount; x++)
                        {
                            if (level.GetGamePiece(i, j).CheckCollision(activeGuards[x]))
                            {
                                switch (level.GetGamePiece(i, j).PieceState)
                                {
                                    case PieceState.Wall: //Touching a wall tile
                                        activeGuards[x].Patrol(0);
                                        break;
                                    case PieceState.Goal: //Touching a goal tile
                                        activeGuards[x].Patrol(0);
                                        break;
                                }
                                if (level.GetGamePiece(i, j).PositionRect.Center == activeGuards[x].PositionRect.Center //For guards being effected by tiles
                                    && (activeGuards[x].Difficulty == 1 || activeGuards[x].Taunted == true))
                                {
                                    switch (level.GetGamePiece(i, j).PieceState)
                                    {
                                        case PieceState.Floor: //Standard floor tile
                                            activeGuards[x].PrevDir = 4;
                                            break;
                                        case PieceState.North: //North direction tile
                                            if (activeGuards[x].Direction != 2 && activeGuards[x].PrevDir != 2)
                                            {
                                                if (activeGuards[x].Taunted == true)
                                                {
                                                    GuardRemake(x, 0);
                                                    activeGuards[x].Taunted = true;
                                                }
                                                else
                                                {
                                                    GuardRemake(x, 0);
                                                    activeGuards[x].Taunted = false;
                                                }
                                                activeGuards[x].Patrol(1);
                                                activeGuards[x].PrevDir = 2;
                                                if (activeGuards[x].Taunted == true)
                                                {
                                                    level.GetGamePiece(i, j).PieceState = temp;
                                                    activeGuards[x].Taunted = false;
                                                    switch (tauntDir)
                                                    {
                                                        case 0:
                                                            level.GetGamePiece(i, j + 1).PieceState = temp2;
                                                            break;
                                                        case 1:
                                                            level.GetGamePiece(i - 1, j).PieceState = temp2;
                                                            break;
                                                        case 3:
                                                            level.GetGamePiece(i + 1, j).PieceState = temp2;
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case PieceState.East: //East direction tile
                                            if (activeGuards[x].Direction != 3 && activeGuards[x].PrevDir != 3)
                                            {
                                                if (activeGuards[x].Taunted == true)
                                                {
                                                    GuardRemake(x, 1);
                                                    activeGuards[x].Taunted = true;
                                                }
                                                else
                                                {
                                                    GuardRemake(x, 1);
                                                    activeGuards[x].Taunted = false;
                                                }
                                                activeGuards[x].Patrol(1);
                                                activeGuards[x].PrevDir = 3;
                                                if (activeGuards[x].Taunted == true)
                                                {
                                                    level.GetGamePiece(i, j).PieceState = temp;
                                                    activeGuards[x].Taunted = false;
                                                    switch (tauntDir)
                                                    {
                                                        case 0:
                                                            level.GetGamePiece(i, j + 1).PieceState = temp2;
                                                            break;
                                                        case 1:
                                                            level.GetGamePiece(i - 1, j).PieceState = temp2;
                                                            break;
                                                        case 2:
                                                            level.GetGamePiece(i, j - 1).PieceState = temp2;
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case PieceState.South: //South direction tile
                                            if (activeGuards[x].Direction != 0 && activeGuards[x].PrevDir != 0)
                                            {
                                                if (activeGuards[x].Taunted == true)
                                                {
                                                    GuardRemake(x, 2);
                                                    activeGuards[x].Taunted = true;
                                                }
                                                else
                                                {
                                                    GuardRemake(x, 2);
                                                    activeGuards[x].Taunted = false;
                                                }
                                                activeGuards[x].Patrol(1);
                                                activeGuards[x].PrevDir = 0;
                                                if (activeGuards[x].Taunted == true)
                                                {
                                                    level.GetGamePiece(i, j).PieceState = temp;
                                                    activeGuards[x].Taunted = false;
                                                    switch (tauntDir)
                                                    {
                                                        case 1:
                                                            level.GetGamePiece(i - 1, j).PieceState = temp2;
                                                            break;
                                                        case 2:
                                                            level.GetGamePiece(i, j - 1).PieceState = temp2;
                                                            break;
                                                        case 3:
                                                            level.GetGamePiece(i + 1, j).PieceState = temp2;
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case PieceState.West: //West direction tile
                                            if (activeGuards[x].Direction != 1 && activeGuards[x].PrevDir != 1)
                                            {
                                                if (activeGuards[x].Taunted == true)
                                                {
                                                    GuardRemake(x, 3);
                                                    activeGuards[x].Taunted = true;
                                                }
                                                else
                                                {
                                                    GuardRemake(x, 3);
                                                    activeGuards[x].Taunted = false;
                                                }
                                                activeGuards[x].Patrol(1);
                                                activeGuards[x].PrevDir = 1;
                                                if (activeGuards[x].Taunted == true)
                                                {
                                                    level.GetGamePiece(i, j).PieceState = temp;
                                                    activeGuards[x].Taunted = false;
                                                    switch (tauntDir)
                                                    {
                                                        case 0:
                                                            level.GetGamePiece(i, j + 1).PieceState = temp2;
                                                            break;
                                                        case 2:
                                                            level.GetGamePiece(i, j - 1).PieceState = temp2;
                                                            break;
                                                        case 3:
                                                            level.GetGamePiece(i + 1, j).PieceState = temp2;
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                        case PieceState.OffEast: //Taunt east tile
                                            GuardRemake(x, 1);
                                            activeGuards[x].Patrol(1);
                                            level.GetGamePiece(i, j).PieceState = temp2;
                                            activeGuards[x].Taunted = false;
                                            switch (tauntDir)
                                            {
                                                case 0:
                                                    level.GetGamePiece(i, j - 1).PieceState = temp;
                                                    break;
                                                case 1:
                                                    level.GetGamePiece(i + 1, j).PieceState = temp;
                                                    break;
                                                case 2:
                                                    level.GetGamePiece(i, j + 1).PieceState = temp;
                                                    break;
                                            }
                                            break;
                                        case PieceState.OffNorth: //Taunt north tile
                                            GuardRemake(x, 0);
                                            activeGuards[x].Patrol(1);
                                            level.GetGamePiece(i, j).PieceState = temp2;
                                            activeGuards[x].Taunted = false;
                                            switch (tauntDir)
                                            {
                                                case 0:
                                                    level.GetGamePiece(i, j - 1).PieceState = temp;
                                                    break;
                                                case 1:
                                                    level.GetGamePiece(i + 1, j).PieceState = temp;
                                                    break;
                                                case 3:
                                                    level.GetGamePiece(i - 1, j).PieceState = temp;
                                                    break;
                                            }
                                            break;
                                        case PieceState.OffWest: //Taunt west tile
                                            GuardRemake(x, 3);
                                            activeGuards[x].Patrol(1);
                                            level.GetGamePiece(i, j).PieceState = temp2;
                                            activeGuards[x].Taunted = false;
                                            switch (tauntDir)
                                            {
                                                case 0:
                                                    level.GetGamePiece(i, j - 1).PieceState = temp;
                                                    break;
                                                case 2:
                                                    level.GetGamePiece(i, j + 1).PieceState = temp;
                                                    break;
                                                case 3:
                                                    level.GetGamePiece(i - 1, j).PieceState = temp;
                                                    break;
                                            }
                                            break;
                                        case PieceState.OffSouth: //Taunt south tile
                                            GuardRemake(x, 2);
                                            activeGuards[x].Patrol(1);
                                            level.GetGamePiece(i, j).PieceState = temp2;
                                            activeGuards[x].Taunted = false;
                                            switch (tauntDir)
                                            {
                                                case 1:
                                                    level.GetGamePiece(i + 1, j).PieceState = temp;
                                                    break;
                                                case 2:
                                                    level.GetGamePiece(i, j + 1).PieceState = temp;
                                                    break;
                                                case 3:
                                                    level.GetGamePiece(i - 1, j).PieceState = temp;
                                                    break;
                                            }
                                            break;
                                        case PieceState.SpecialNorth: //North rotating tile
                                            GuardRemake(x, 0);
                                            activeGuards[x].Patrol(1);
                                            level.GetGamePiece(i, j).IncrementType();
                                            break;
                                        case PieceState.SpecialEast: //East rotating tile
                                            GuardRemake(x, 1);
                                            activeGuards[x].Patrol(1);
                                            level.GetGamePiece(i, j).IncrementType();
                                            break;
                                        case PieceState.SpecialSouth: //South rotating tile
                                            GuardRemake(x, 2);
                                            activeGuards[x].Patrol(1);
                                            level.GetGamePiece(i, j).IncrementType();
                                            break;
                                        case PieceState.SpecialWest: //West rotating tile
                                            GuardRemake(x, 3);
                                            activeGuards[x].Patrol(1);
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
    }
}
