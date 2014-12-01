using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;

namespace TheCreator
{
    public partial class CharacterCreator : Form
    {
        public CharacterCreator()
        {
            path = Directory.GetCurrentDirectory() + "/../../../../../../Colors.txt";
            StreamReader ogColorFile = new StreamReader(path);
            ogColors = ogColorFile.ReadLine();
            ogColorFile.Close();
            InitializeComponent();
        }

        // might use later
        //Color customHead;
        //Color customBody;
        //Color customGear;

        //orginal colors from globalvar before edit
        string path;
        string ogColors;

        // holds ints for each RGB value for each piece.
        int headR;
        int headG;
        int headB;

        int bodyR;
        int bodyG;
        int bodyB;

        int gearR;
        int gearG;
        int gearB;


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;

            switch (index)
            {
                case 0:
                    textBox1.Text = "255";
                    textBox2.Text = "0";
                    textBox3.Text = "0";
                    break;

                case 1:
                    textBox1.Text = "0";
                    textBox2.Text = "255";
                    textBox3.Text = "0";
                    break;

                case 2:
                    textBox1.Text = "0";
                    textBox2.Text = "0";
                    textBox3.Text = "255";
                    break;

                case 3:
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox2.SelectedIndex;

            switch (index)
            {
                case 0:
                    textBox4.Text = "255";
                    textBox5.Text = "0";
                    textBox6.Text = "0";
                    break;

                case 1:
                    textBox4.Text = "0";
                    textBox5.Text = "255";
                    textBox6.Text = "0";
                    break;

                case 2:
                    textBox4.Text = "0";
                    textBox5.Text = "0";
                    textBox6.Text = "255";
                    break;

                case 3:
                    break;
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox3.SelectedIndex;

            switch (index)
            {
                case 0:
                    textBox7.Text = "255";
                    textBox8.Text = "0";
                    textBox9.Text = "0";
                    break;

                case 1:
                    textBox7.Text = "0";
                    textBox8.Text = "255";
                    textBox9.Text = "0";
                    break;

                case 2:
                    textBox7.Text = "0";
                    textBox8.Text = "0";
                    textBox9.Text = "255";
                    break;

                case 3:
                    break;
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempR = Int32.Parse(textBox1.Text);

                // checks to make sure it is in range for RGV values.
                if (tempR > 255) tempR = 255;
                if (tempR < 0) tempR = 0;

                headR = tempR;

            }
            catch (Exception)
            {
                textBox1.Text = "";
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempG = Int32.Parse(textBox2.Text);

                if (tempG > 255) tempG = 255;
                if (tempG < 0) tempG = 0;

                headG = tempG;
            }
            catch (Exception)
            {
                textBox2.Text = "";
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempB = Int32.Parse(textBox3.Text);

                if (tempB > 255) tempB = 255;
                if (tempB < 0) tempB = 0;

                headB = tempB;
            }

            catch (Exception)
            {
                textBox3.Text = "";
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempR = Int32.Parse(textBox4.Text);

                if (tempR > 255) tempR = 255;
                if (tempR < 0) tempR = 0;

                bodyR = tempR;
            }

            catch (Exception)
            {
                textBox4.Text = "";
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempG = Int32.Parse(textBox5.Text);

                if (tempG > 255) tempG = 255;
                if (tempG < 0) tempG = 0;

                bodyG = tempG;
            }

            catch (Exception)
            {
                textBox5.Text = "";
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempB = Int32.Parse(textBox6.Text);

                if (tempB > 255) tempB = 255;
                if (tempB < 0) tempB = 0;

                bodyB = tempB;
            }

            catch (Exception)
            {
                textBox6.Text = "";
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempR = Int32.Parse(textBox7.Text);

                if (tempR > 255) tempR = 255;
                if (tempR < 0) tempR = 0;

                gearR = tempR;
            }

            catch (Exception)
            {
                textBox7.Text = "";
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempG = Int32.Parse(textBox8.Text);

                if (tempG > 255) tempG = 255;
                if (tempG < 0) tempG = 0;

                gearG = tempG;
            }

            catch (Exception)
            {
                textBox8.Text = "";
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int tempB = Int32.Parse(textBox9.Text);

                if (tempB > 255) tempB = 255;
                if (tempB < 0) tempB = 0;

                gearB = tempB;
            }

            catch (Exception)
            {
                textBox9.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // overwrite previous values in file
            StreamWriter writer = new StreamWriter(path, false);

            writer.WriteLine(headR + "," + headG + "," + headB + "," +
                bodyR + "," + bodyG + "," + bodyB + "," +
                gearR + "," + gearG + "," + gearB);

            writer.Close();

            
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void CharacterCreator_Load(object sender, EventArgs e)
        {

        }
    }
}
