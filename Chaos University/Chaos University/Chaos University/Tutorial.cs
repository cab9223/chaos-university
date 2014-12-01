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

            index = 0;

            tutorials.Add(new string[1] { "Press Enter to start moving." });
            tutorials.Add(new string[1] { "PLACE HOLDER TUT 2" });
            tutorials.Add(new string[1] { "PLACE HOLDER TUT 3" });
            tutorials.Add(new string[1] { "PLACE HOLDER TUT 4" });
            tutorials.Add(new string[1] { "PLACE HOLDER TUT 5" });

            locations.Add(new int[2] { 20, 20 });
            locations.Add(new int[2] { 20, 20 });
            locations.Add(new int[2] { 20, 20 });
            locations.Add(new int[2] { 20, 20 });
            locations.Add(new int[2] { 20, 20 });
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
                    tutorials[index][1],
                    new Vector2(locations[index][1] + offX, locations[index][2] + offY),
                    Color.White);
            }
        }
    }
}
