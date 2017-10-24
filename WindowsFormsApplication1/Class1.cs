using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    [Serializable]
    class Class1
    {
        public int[,] i = new int[6, 6];
        public int score { get; private set; }     //存储当前成绩
        public int best { get ;set;}               //存储最好成绩    
        private int quantity { get; set; }         //当前不为0的方块个数
        private Random r = new Random();
        public bool die = false;                   //是否游戏结束
        public bool First2048 = true;              //是否第一次出现2048
        public bool change = false;                //记录方块可否移动

        public void Restart()          //重新开始游戏
        {
            for (int x = 0; x <= 5; x++)
                for (int y = 0; y <= 5; y++)
                    i[x, y] = 0;
            quantity = 0;
            die = false;
            First2048=true;
            if (score > best)
                best = score;
            score = 0;
            Add();
            Add();
        }

        public void Add()             //随机一个位置添加2或4，如该位置已有数字，再随机一个位置，直到添加成功
        {
            int x = r.Next(1, 5);
            int y = r.Next(1, 5);
            if (i[x, y] == 0)
            {
                if (r.Next(1, 101) >= 90)
                    i[x, y] = 4;
                else i[x, y] = 2;
                quantity++;
                Die();
            }
            else Add();
        }

        //上下左右移动后合并相同数字、修改分数
        public void Down()
        {
            change = false;
            down();
            for (int x = 1; x <= 4; x++)
            {
                if (i[x, 4] == i[x, 3] && i[x, 4] + i[x, 3] != 0)      //相邻两个方块的数字相同且都不为空
                {
                    if (i[x, 2] == i[x, 1])
                    {
                        i[x, 4] *= 2;
                        i[x, 3] = i[x, 2] * 2;
                        i[x, 2] = 0;
                        i[x, 1] = 0;
                        score += i[x, 4] + i[x, 3];
                    }
                    else
                    {
                        i[x, 4] *= 2;
                        i[x, 3] = i[x, 2];
                        i[x, 2] = i[x, 1];
                        i[x, 1] = 0;
                        score += i[x, 4];
                    }
                    change = true;
                }
                else if (i[x, 3] == i[x, 2] && i[x, 3] + i[x, 2] != 0)
                {
                    i[x, 3] *= 2;
                    i[x, 2] = i[x, 1];
                    i[x, 1] = 0;
                    change = true;
                    score += i[x, 3];
                }
                else if (i[x, 2] == i[x, 1] && i[x, 2] + i[x, 1] != 0)
                {
                    i[x, 2] *= 2;
                    i[x, 1] = 0;
                    change = true;
                    score += i[x, 2];
                }
            }
            GetQuantity();
        }

        public void Up()
        {
            change = false;
            up();
            for (int x = 1; x <= 4; x++)
            {
                if (i[x, 1] == i[x, 2] && i[x, 1] + i[x, 2] != 0)
                {
                    if (i[x, 3] == i[x, 4])
                    {
                        i[x, 1] *= 2;
                        i[x, 2] = i[x, 3] * 2;
                        i[x, 3] = 0;
                        i[x, 4] = 0;
                        score += i[x, 1] + i[x, 2];
                    }
                    else
                    {
                        i[x, 1] *= 2;
                        i[x, 2] = i[x, 3];
                        i[x, 3] = i[x, 4];
                        i[x, 4] = 0;
                        score += i[x, 1];
                    }
                    change = true;
                }
                else if (i[x, 2] == i[x, 3] && i[x, 2] + i[x, 3] != 0)
                {
                    i[x, 2] *= 2;
                    i[x, 3] = i[x, 4];
                    i[x, 4] = 0;
                    change = true;
                    score += i[x, 2];
                }
                else if (i[x, 3] == i[x, 4] && i[x, 3] + i[x, 4] != 0)
                {
                    i[x, 3] *= 2;
                    i[x, 4] = 0;
                    change = true;
                    score += i[x, 3];
                }
            }
            GetQuantity();
        }

        public void Left()
        {
            change = false;
            left();
            for (int y = 1; y <= 4; y++)
            {
                if (i[1, y] == i[2, y] && i[1, y] + i[2, y] != 0)
                {
                    if (i[3, y] == i[4, y])
                    {
                        i[1, y] *= 2;
                        i[2, y] = i[3, y];
                        i[3, y] = 0;
                        i[4, y] = 0;
                        score += i[1, y] + i[2, y];

                    }
                    else
                    {
                        i[1, y] *= 2;
                        i[2, y] = i[3, y];
                        i[3, y] = i[4, y];
                        i[4, y] = 0;
                        score += i[1, y];
                    }
                    change = true;
                }
                else if (i[2, y] == i[3, y] && i[2, y] + i[3, y] != 0)
                {
                    i[2, y] *= 2;
                    i[3, y] = i[4, y];
                    i[4, y] = 0;
                    change = true;
                    score += i[2, y];
                }
                else if (i[3, y] == i[4, y] && i[3, y] + i[4, y] != 0)
                {
                    i[3, y] *= 2;
                    i[4, y] = 0;
                    change = true;
                    score += i[3, y];
                }
            }
            GetQuantity();
        }

        public void Right()
        {
            change = false;
            right();
            for (int y = 1; y <= 4; y++)
            {
                if (i[4, y] == i[3, y] && i[4, y] + i[3, y] != 0)
                {
                    if (i[2, y] == i[1, y])
                    {
                        i[4, y] *= 2;
                        i[3, y] = i[2, y];
                        i[2, y] = 0;
                        i[1, y] = 0;
                        score += i[4, y] + i[3, y];
                    }
                    else
                    {
                        i[4, y] *= 2;
                        i[3, y] = i[2, y];
                        i[2, y] = i[1, y];
                        i[1, y] = 0;
                        score += i[4, y];
                    }
                    change = true;
                }
                else if (i[3, y] == i[2, y] && i[3, y] + i[2, y] != 0)
                {
                    i[3, y] *= 2;
                    i[2, y] = i[1, y];
                    i[1, y] = 0;
                    change = true;
                    score += i[3, y];
                }
                else if (i[2, y] == i[1, y] && i[3, y] + i[1, y] != 0)
                {
                    i[2, y] *= 2;
                    i[1, y] = 0;
                    change = true;
                    score += i[2, y];
                }
            }
            GetQuantity();
        }

        //上下左右移动后空位的替补和增加
        private void down()
        {
            for (int x = 1; x <= 4; x++)
            {
                if (i[x, 4] == 0 && i[x, 1] + i[x, 2] + i[x, 3] != 0)
                {
                    i[x, 4] = i[x, 3];
                    i[x, 3] = i[x, 2];
                    i[x, 2] = i[x, 1];
                    i[x, 1] = 0;
                    change = true;
                    down();
                }
                else if (i[x, 3] == 0 && i[x, 1] + i[x, 2] != 0)
                {
                    i[x, 3] = i[x, 2];
                    i[x, 2] = i[x, 1];
                    i[x, 1] = 0;
                    change = true;
                    down();
                }
                else if (i[x, 2] == 0 && i[x, 1] != 0)
                {
                    i[x, 2] = i[x, 1];
                    i[x, 1] = 0;
                    change = true;
                }
            }
        }

        private void up()
        {
            for (int x = 1; x <= 4; x++)
            {
                if (i[x, 1] == 0 && i[x, 4] + i[x, 3] + i[x, 2] != 0)
                {
                    i[x, 1] = i[x, 2];
                    i[x, 2] = i[x, 3];
                    i[x, 3] = i[x, 4];
                    i[x, 4] = 0;
                    change = true;
                    up();
                }
                else if (i[x, 2] == 0 && i[x, 4] + i[x, 3] != 0)
                {
                    i[x, 2] = i[x, 3];
                    i[x, 3] = i[x, 4];
                    i[x, 4] = 0;
                    change = true;
                    up();
                }
                else if (i[x, 3] == 0 && i[x, 4] != 0)
                {
                    i[x, 3] = i[x, 4];
                    i[x, 4] = 0;
                    change = true;
                }
            }
        }
        
        private void left()
        {
            for (int y = 1; y <= 4; y++)
            {
                if (i[1, y] == 0 && i[4, y] + i[3, y] + i[2, y] != 0)
                {
                    i[1, y] = i[2, y];
                    i[2, y] = i[3, y];
                    i[3, y] = i[4, y];
                    i[4, y] = 0;
                    change = true;
                    left();
                }
                else if (i[2, y] == 0 && i[4, y] + i[3, y] != 0)
                {
                    i[2, y] = i[3, y];
                    i[3, y] = i[4, y];
                    i[4, y] = 0;
                    change = true;
                    left();
                }
                else if (i[3, y] == 0 && i[4, y] != 0)
                {
                    i[3, y] = i[4, y];
                    i[4, y] = 0;
                    change = true;
                }
            }
        }
        
        private void right()
        {
            for (int y = 1; y <= 4; y++)
            {
                if (i[4, y] == 0 && i[1, y] + i[2, y] + i[3, y] != 0)
                {
                    i[4, y] = i[3, y];
                    i[3, y] = i[2, y];
                    i[2, y] = i[1, y];
                    i[1, y] = 0;
                    change = true;
                    right();
                }
                else if (i[3, y] == 0 && i[1, y] + i[2, y] != 0)
                {
                    i[3, y] = i[2, y];
                    i[2, y] = i[1, y];
                    i[1, y] = 0;
                    change = true;
                    right();
                }
                else if (i[2, y] == 0 && i[1, y] != 0)
                {
                    i[2, y] = i[1, y];
                    i[1, y] = 0;
                    change = true;
                }
            }
        }

        private void GetQuantity()       //统计当前不为0的方块个数
        {
            int count = 0;
            for (int x = 1; x <= 4; x++)
                for (int y = 1; y <= 4; y++)
                {
                    if (i[x, y] != 0)
                        count++;
                }
            quantity = count;
        }

        private void Die()   //检测游戏是否结束，结束的标志：16个方块已满且互不相邻的8个方块与其自上下左右的方块数字都不相等
        {
            int count = 0;
            if (quantity == 16)
            {
                for (int x = 1; x <= 3; x += 2)
                    for (int y = 1; y <= 3; y += 2)
                        if (!GetEqual(x, y))
                            count++;
                for (int x = 2; x <= 4; x += 2)
                    for (int y = 2; y <= 4; y += 2)
                        if (!GetEqual(x, y))
                            count++;
                if (count == 8)
                    die = true;
            }
        }

        private bool GetEqual(int x, int y)   //判断某方块与其上下左右的方块数字是否相等
        {

            if (i[x, y] == i[x - 1, y])
                return true;
            else if (i[x, y] == i[x + 1, y])
                return true;
            else if (i[x, y] == i[x, y - 1])
                return true;
            else if (i[x, y] == i[x, y + 1])
                return true;
            else return false;

        }
    }
}
