using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class GameOver : Form
    {
        public GameOver()
        {
            InitializeComponent();
        }

        public int s { get; set; }
        public int b { get; set; }

        private void GameOver_Load(object sender, EventArgs e)
        {
            label2.Text += s;
            label3.Text += b;
            this.TopLevel = true;
        }
    }
}
