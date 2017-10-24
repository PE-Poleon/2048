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
    public partial class message : Form
    {
        public message()
        {
            InitializeComponent();
        }
        public string a { get; set; }
        public string b { get; set; }

        private void message_MouseClick(object sender, MouseEventArgs e)   //单击退出界面
        {
            this.Close();
        }

        private void message_KeyDown(object sender, KeyEventArgs e)    //按ENTER退出界面
        {
            if (e.KeyCode == Keys.Enter)
                this.Close();
        }

        private void message_Load(object sender, EventArgs e)    //载入对话框
        {
            label1.Text = a;
            textBox1.Text = b;
            this.TopLevel = true;
        }
    }
}
