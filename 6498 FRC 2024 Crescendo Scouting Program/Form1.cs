using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _6498_FRC_2024_Crescendo_Scouting_Program
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.BackColor == Color.Green)
            {
                btn.BackColor = Color.Red;
                btn.Text = "X";
                btn.Tag = "False";
            }
            else
            {
                btn.BackColor = Color.Green;
                btn.Text = "✔";
                btn.Tag = "True";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (btn.Tag.ToString() == "hidden")
            {
                btn.Text = "Hide Notes";
                this.Height = 718;
                btn.Tag = "shown";
            }
            else
            {
                btn.Text = "Show Notes";
                this.Height = 565;
                btn.Tag = "hidden";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Top -= 75;
            cmbInfoPosition.SelectedIndex = 0;
            cmbStageParkClimb.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
