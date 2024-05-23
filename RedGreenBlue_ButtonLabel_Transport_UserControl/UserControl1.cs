using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedGreenBlue_ButtonLabel_Transport_UserControl
{
    public partial class UserControl1 : UserControl
    {
        static Random myRand = new Random();
        public Control[] arrControls;
        public UserControl1(int width, int maxCounter, string fullEmpty)
        {
            InitializeComponent();
            arrControls = new Control[maxCounter];
            this.Width = width;

            int commonWidth = 2;
            for (int i = 0; i < maxCounter; i++)
            {
                int tempWidth = 30;
                arrControls[i] = new Label();
                arrControls[i].Size = new Size(30, 30);
                if (fullEmpty == "Full")
                {
                  
                    arrControls[i].Text = myRand.Next(1, 3).ToString();
                   
                    switch (myRand.Next(3))
                    {
                        case 0: arrControls[i].BackColor = Color.FromArgb(220, 0, 0); break;
                        case 1: arrControls[i].BackColor = Color.FromArgb(0, 200, 0); break;
                        case 2: arrControls[i].BackColor = Color.FromArgb(0, 0, 255); break;
                    }
                }
                else
                {
                    arrControls[i].BackColor = Color.White;
                }

                arrControls[i].Location = new Point(commonWidth, 3);
                commonWidth += tempWidth + 2;
                this.Controls.Add(arrControls[i]);
            }
        }
    }
}
