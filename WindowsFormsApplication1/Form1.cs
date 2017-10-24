using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Class1 c;
        Bitmap bit = new Bitmap(400, 400);

        private void MusicBtn(object sender, EventArgs e)  //音乐开关按钮
        {
            SoundPlayer player = new SoundPlayer(Directory.GetCurrentDirectory() + @"\music.wav");
            if (button1.Text == "MUSIC ON")
            {
                player.Stop();
                button1.Text = "MUSIC OFF";
            }
            else
            {
                player.PlayLooping();
                button1.Text = "MUSIC ON";
            }
        }

        private void RestartBtn(object sender, EventArgs e)  //重新开始按钮
        {
            c = new Class1();
            c.Restart();
            pictureBox1.Refresh();
            draw();
            pictureBox1.Refresh();
            score.Text = c.score.ToString();
        }

        private void HelpBtn(object sender, EventArgs e)  //帮助菜单按钮
        {
            message mes = new message();
            mes.a = "Help：";
            mes.b = "  玩家可以选择上下左右四个方向，若棋盘内的数字出现位移或合并，视为有效移动。\r\n  玩家选择的方向上若有相同的数字则合并，每次有效移动可以同时合并，但不可以连续合并；合并所得的所有新生成数字相加即为该步的有效得分；玩家选择的方向行或列前方有空格则出现位移。\r\n  棋盘被数字填满，无法进行有效移动，判负，游戏结束；棋盘上出现2048，判胜。\r\n  W、S、A、D：控制方块上下左右移动\r\n  F1：截图并保存\r\n  ESC：退出";
            mes.StartPosition = FormStartPosition.CenterParent;
            mes.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("D:\\2048记录"))  //检测是否有存档
            {
                ToLoad();
            }
            else                              //否则为首次进入游戏
            {
                message mes1 = new message();
                mes1.a = "Welcome！";
                mes1.b = "玩家可以选择上下左右四个方向，若棋盘内的数字出现位移或合并，视为有效移动。\r\n  玩家选择的方向上若有相同的数字则合并，每次有效移动可以同时合并，但不可以连续合并；合并所得的所有新生成数字相加即为该步的有效得分；玩家选择的方向行或列前方有空格则出现位移。\r\n  棋盘被数字填满，无法进行有效移动，判负，游戏结束；棋盘上出现2048，判胜。\r\n  W、S、A、D：控制方块上下左右移动\r\n  F1：截图并保存\r\n  ESC：退出";
                mes1.StartPosition = FormStartPosition.CenterParent;
                mes1.ShowDialog();
                c = new Class1();
                c.Restart();
            }
            
            //载入音乐与界面
            SoundPlayer player = new SoundPlayer(Directory.GetCurrentDirectory() + @"\music.wav");
            player.PlayLooping();
            draw();
            pictureBox1.Refresh();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)  //各按键对应的响应，每次按键后刷新界面、判断胜负
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    c.Up();
                    if (c.change)        //成功移动便随机一个位置添加数字，下同
                        c.Add();
                    break;
                case Keys.S:
                    c.Down();
                    if (c.change)
                        c.Add();
                    break;
                case Keys.A:
                    c.Left();
                    if (c.change)
                        c.Add();
                    break;
                case Keys.D:
                    c.Right();
                    if (c.change)
                        c.Add();
                    break;
                case Keys.F1:
                    Screen();
                    message mes3 = new message();
                    mes3.a = "Saved successfully！";
                    mes3.b = "保存位置：" + Directory.GetCurrentDirectory() + "\\screenshot.png";
                    mes3.StartPosition = FormStartPosition.CenterParent;
                    mes3.ShowDialog();
                    break;
                case Keys.Escape:
                    this.Close();
                    break;
            }
            draw();
            pictureBox1.Refresh();
            score.Text = c.score.ToString();
            for (int x = 1; x <= 4; x++)
                for (int y = 1; y <= 4; y++)
                {
                    if (c.i[x,y]==2048&&c.First2048==true)
                    { 
                        isWin();
                        c.First2048 = false;
                    }
                }
            if (c.die)
            {
                Gameover();
            }
        }

        private string ss = Directory.GetCurrentDirectory() + "\\screenshot";   //成绩截图的保存位置
        private void Screen()  //截图
        {
            Bitmap b = new Bitmap(this.Width, this.Height);
            Graphics gr = Graphics.FromImage(b);
            gr.CopyFromScreen(this.Location, new Point(0, 0), this.Size);
            gr.Dispose();
            if (!File.Exists(ss))  //检测截图是否存在，如已存在，为避免覆盖，将重命名
                ss += "1";
            b.Save(ss + ".png");

        }

        private void display(int m, Point dian)   //设置方块上数字字体及对应的背景颜色
        {
            Graphics gra = Graphics.FromImage(bit);
            switch (m)
            {
                case 0:
                    { gra.FillRectangle(new SolidBrush(Color.SlateGray), dian.X, dian.Y, 90, 90); } break;
                case 2:
                    { gra.FillRectangle(new SolidBrush(Color.DarkKhaki), dian.X, dian.Y, 90, 90); } break;
                case 4:
                    { gra.FillRectangle(new SolidBrush(Color.DarkSeaGreen), dian.X, dian.Y, 90, 90); } break;
                case 8:
                    { gra.FillRectangle(new SolidBrush(Color.OliveDrab), dian.X, dian.Y, 90, 90); } break;
                case 16:
                    { gra.FillRectangle(new SolidBrush(Color.SeaGreen), dian.X, dian.Y, 90, 90); } break;
                case 32:
                    { gra.FillRectangle(new SolidBrush(Color.DarkSlateGray), dian.X, dian.Y, 90, 90); } break;
                case 64:
                    { gra.FillRectangle(new SolidBrush(Color.Peru), dian.X, dian.Y, 90, 90); } break;
                case 128:
                    { gra.FillRectangle(new SolidBrush(Color.Sienna), dian.X, dian.Y, 90, 90); } break;
                case 256:
                    { gra.FillRectangle(new SolidBrush(Color.DarkOrange), dian.X, dian.Y, 90, 90); } break;
                case 512:
                    { gra.FillRectangle(new SolidBrush(Color.DarkCyan), dian.X, dian.Y, 90, 90); } break;
                case 1024:
                    { gra.FillRectangle(new SolidBrush(Color.DarkRed), dian.X, dian.Y, 90, 90); } break;
                case 2048:
                    { gra.FillRectangle(new SolidBrush(Color.PaleVioletRed), dian.X, dian.Y, 90, 90); } break;
                case 4096:
                    { gra.FillRectangle(new SolidBrush(Color.MidnightBlue), dian.X, dian.Y, 90, 90); } break;
                case 8192:
                    { gra.FillRectangle(new SolidBrush(Color.HotPink), dian.X, dian.Y, 90, 90); } break;
            }
            switch (m)
            {
                case 2:
                case 4:
                case 8:
                    gra.DrawString(m.ToString(), new Font("黑体", 40.5f, FontStyle.Bold), new SolidBrush(Color.White), dian.X + 22, dian.Y + 17);
                    break;
                case 16:
                case 32:
                case 64:
                    gra.DrawString(m.ToString(), new Font("黑体", 40.5f, FontStyle.Bold), new SolidBrush(Color.White), dian.X + 8, dian.Y + 17);
                    break;
                case 128:
                case 256:
                case 512:
                    gra.DrawString(m.ToString(), new Font("黑体", 35.5f, FontStyle.Bold), new SolidBrush(Color.White), dian.X + 0, dian.Y + 20);
                    break;
                case 1024:
                case 2048:
                case 4096:
                case 8192:
                    gra.DrawString(m.ToString(), new Font("黑体", 30.5f, FontStyle.Bold), new SolidBrush(Color.White), dian.X - 4, dian.Y + 23);
                    break;

            }
        }

        private void draw()  //绘制方块
        {
            for (int x = 1; x <= 4; x++)
            {
                for (int y = 1; y <= 4; y++)
                {
                    Point p = new Point(x * 100 - 95, y * 100 - 95);
                    display(c.i[x, y], p);
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)  //重绘方块，同时刷新当前成绩
        {
            pictureBox1.BackgroundImage = bit;
            score.Text = c.score.ToString();
        }

        private void Gameover()   //游戏结束
        {
            if (c.best < c.score)                                    //判断本次成绩是否刷新纪录
            {
                c.best = c.score;
                best.Text = c.best.ToString();
                Screen();
                message mes4 = new message();
                mes4.a = "Congratulations！";
                mes4.b = "新记录！自动为您保存截图。\r\n保存位置：" + ss;
                mes4.StartPosition = FormStartPosition.CenterParent;
                mes4.ShowDialog();
                c.Restart();
                draw();
                pictureBox1.Refresh();

            }
            else
            {
                GameOver g = new GameOver();
                g.b = c.best;
                g.s = c.score;
                DialogResult d = g.ShowDialog();
                switch (d)
                {
                    case DialogResult.Retry:
                        c.Restart();
                        draw();
                        pictureBox1.Refresh();
                        score.Text = c.score.ToString();
                        best.Text = c.best.ToString();
                        break;
                    case DialogResult.Abort:
                        Screen();
                        message mes3 = new message();
                        mes3.a = "Saved successfully！";
                        mes3.b = "保存位置：" + ss;
                        mes3.StartPosition = FormStartPosition.CenterParent;
                        mes3.ShowDialog();
                        c.Restart();
                        classSave();
                        draw();
                        pictureBox1.Refresh();
                        break;
                    case DialogResult.No:
                        c.Restart();
                        this.Close();
                        break;
                }
            }
        }

        private void isWin()  //第一次出现2048，即获胜
        {
            Win w = new Win();
            w.StartPosition = FormStartPosition.CenterParent;
            DialogResult d1 = w.ShowDialog();
            switch (d1)
            {
                case DialogResult.Abort:
                    break;
                case DialogResult.No:
                    Screen();
                    message mes5 = new message();
                    mes5.a = "Saved successfully！";
                    mes5.b = "保存位置：" + ss;
                    mes5.StartPosition = FormStartPosition.CenterParent;
                    mes5.ShowDialog();
                    c.Restart();
                    this.Close();
                    break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)  //关闭窗口后的操作
        {
            classSave();
        }

        private void classSave()  //序列化Class1类，并存档
        {
            FileStream fw = new FileStream("D:\\2048记录", FileMode.Create, FileAccess.Write);
            BinaryFormatter formatter_w = new BinaryFormatter();
            formatter_w.Serialize(fw, c);
            fw.Close();
        }

        private void ToLoad()  //加载上次退出时的记录
        {
            FileStream fr = new FileStream("D:\\2048记录", FileMode.Open, FileAccess.Read);
            BinaryFormatter formatter_r = new BinaryFormatter();
            c = (Class1)formatter_r.Deserialize(fr);
            score.Text = c.score.ToString();
            best.Text = c.best.ToString();
            fr.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)  //确认是否退出
        {
            DialogResult res = MessageBox.Show("是否退出？（将为您保存进度）", "注意",
                               MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
            if (res == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
