using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Windows.Forms;

namespace Chaos_University
{
    public partial class CharacterCreator : Form
    {
        public CharacterCreator()
        {
            InitializeComponent();
        }

        // might use later
        //Color customHead;
        //Color customBody;
        //Color customGear;

        //orginal colors from globalvar before edit
        Color ogHead = GlobalVar.headColor;
        Color ogBody = GlobalVar.bodyColor;
        Color ogGear = GlobalVar.gearColor;

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
                    GlobalVar.headColor = Color.Red;
                    break;

                case 1:
                    GlobalVar.headColor = Color.Green;
                    break;

                case 2:
                    GlobalVar.headColor = Color.Blue;
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
                    GlobalVar.bodyColor = Color.Red;
                    break;

                case 1:
                    GlobalVar.bodyColor = Color.Green;
                    break;

                case 2:
                    GlobalVar.bodyColor = Color.Blue;
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
                    GlobalVar.gearColor = Color.Red;
                    break;

                case 1:
                    GlobalVar.gearColor = Color.Green;
                    break;

                case 2:
                    GlobalVar.gearColor = Color.Blue;
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
            if (comboBox1.SelectedIndex == 3)
            {
                GlobalVar.headColor = Color.FromNonPremultiplied(headR, headG, headB, 255);
            }

            if (comboBox2.SelectedIndex == 3)
            {
                GlobalVar.bodyColor = Color.FromNonPremultiplied(bodyR, bodyG, bodyB, 255);
            }

            if (comboBox3.SelectedIndex == 3)
            {
                GlobalVar.gearColor = Color.FromNonPremultiplied(gearR, gearG, gearB, 255);
            }
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GlobalVar.headColor = ogHead;
            GlobalVar.bodyColor = ogBody;
            GlobalVar.gearColor = ogGear;
            this.Hide();
        }

        private void CharacterCreator_Load(object sender, EventArgs e)
        {

        }
    }
}
