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
    class Tutorial
    {
        int screenHeight;   //Provided Height of Screen.
        int screenWidth;    //Provided Width of Screen.
        SpriteFont font;    //Font

        int index;          //Index of current level.

        //List of tutorials string.
        private List<string[]> tutorials;

        //List of tutorial string locations.
        private List<int[]> locations;

        //Constructor.
        public Tutorial(int _screenHeight, int _screenWidth, SpriteFont _font)
        {
            screenHeight = _screenHeight;
            screenWidth = _screenWidth;
            font = _font;

            index = -1;

            tutorials = new List<string[]>();
            locations = new List<int[]>();

            tutorials.Add(new string[1] { "" });
            tutorials.Add(new string[1] { "Press Enter to start moving." });
            tutorials.Add(new string[1] { "Press Enter to start moving." });
            tutorials.Add(new string[1] { "Click the floor to place a tile." });
            tutorials.Add(new string[2] { "Use WASD or the arrow keys to move the camera.", "Try to not go over par." });
            tutorials.Add(new string[1] { "Press 'R' to reset." });

            locations.Add(new int[2] { 0, 0 });
            locations.Add(new int[2] { 24, 180 });
            locations.Add(new int[2] { 24, -60 });
            locations.Add(new int[2] { 24, -60 });
            locations.Add(new int[4] { 20, -40, 250, 550 });
            locations.Add(new int[2] { 0, -40 });
        }

        //Increment level.
        public void Increment()
        {
            index++;
        }

        //Draw tutorial strings.
        public void Draw(SpriteBatch spriteBatch, int offX, int offY)
        {
            if (index < tutorials.Count)
            {
                spriteBatch.DrawString(font,
                    tutorials[index][0],
                    new Vector2(locations[index][0] + offX, locations[index][1] + offY),
                    Color.White);

                //Display second message.
                //More messy code. Should also be fixed.
                if(tutorials[index].Length > 1)
                {
                    spriteBatch.DrawString(font,
                        tutorials[index][1],
                        new Vector2(locations[index][2] + offX, locations[index][3] + offY),
                        Color.White);
                }
            }
        }
    }
}
