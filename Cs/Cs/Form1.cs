﻿using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
namespace Cs
{
    
    public partial class Form1 : Form
    {
        private SolidBrush background = new SolidBrush(Color.Wheat);
        private SolidBrush filled = new SolidBrush(Color.FromArgb(71,(0xFF+Color.Blue.R-Color.Yellow.R)&0xFF, (0xFF + Color.Blue.G - Color.Yellow.G) & 0xFF, (0xFF + Color.Blue.B - Color.Yellow.B) & 0xFF));
        private SolidBrush active = new SolidBrush(Color.Blue);
        private SolidBrush blank = new SolidBrush(Color.Gray);
        private SolidBrush going = new SolidBrush(Color.Yellow);
        private Graphics draw;
        System.Media.SoundPlayer player = new SoundPlayer(@"C:\Users\Zero Weight\Documents\GitHub\Signals-System\music\school_song_double_C.wav");
        const int L = 100;
        const int D = 10;
        const int xstat = 10;
        const int ystat = 12;
        const int width = 12;
        const int height = 10;
        int go = 0;
        int row;
        int max;
        int min;
        int[][] map = new int[L][];
        string temp;
        string[] dictionary = new string[D];
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Read_In();
        }
        private void Read_In()
        {
            min = 1000;
            max = 0;
            row = 0;
            StreamReader fileReader = new StreamReader(@"C:\Users\Zero Weight\Documents\GitHub\Signals-System\#6\output.txt");
            for (int i=0;i<L;i++)
            {
                map[i] = new int[D];
                temp = fileReader.ReadLine();
                if (temp == null) break;
                dictionary = temp.Split(' ');
                int counter = 0;
                foreach (string str in dictionary)
                {
                    if (str.Length == 0) continue;
                    map[i][counter] = int.Parse(str);
                    if (map[i][counter] < min&& map[i][counter]!=0) min = map[i][counter];
                    if (map[i][counter] > max) max = map[i][counter];
                    counter++;
                }
                row++;
            }
            for(int i = 0; i < row; i++)
            {
                for(int j = 0; j < D; j++)
                {
                    map[i][j] -= min;
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Paint += new PaintEventHandler(Init);
            button1.Enabled = false;
            button2.Enabled = true;
            this.CreateGraphics().FillRectangle(background, xstat, ystat, width * row, height * (max - min + 1));
            for (int i = 0; i < row; i++)
            {
                if (map[i][0] < 0)
                {
                    leave(i);
                    continue;
                }
                for(int j = 0; j < D; j++)
                {
                    if (map[i][j] < 0) break;
                    paint(i, map[i][j]);
                }
            }
        }
        private void Init(object sender, PaintEventArgs e)
        {
            this.CreateGraphics().FillRectangle(background, xstat, ystat, width * row, height * (max - min + 1));
            for (int i = 0; i < row; i++)
            {
                if (map[i][0] < 0)
                {
                    leave(i);
                    continue;
                }
                for (int j = 0; j < D; j++)
                {
                    if (map[i][j] < 0) break;
                    paint(i, map[i][j]);
                }
            }
        }
        private void leave(int x)
        {
            this.CreateGraphics().FillRectangle(blank, xstat + x * width, ystat, width, height*(max-min+1));
        }
        private void enable(int x)
        {
            this.CreateGraphics().FillRectangle(going, xstat + x * width, ystat, width, height * (max - min + 1));
        }
        private void revert(int x)
        {
            this.CreateGraphics().FillRectangle(background, xstat + x * width, ystat, width, height * (max - min + 1));
        }
        private void fire(int x,int y)
        {
            draw = this.CreateGraphics();
            draw.FillRectangle(active, xstat + x * width, ystat + (max - min - y) * height, width, height);
        }
        private void paint(int x,int y)
        {
            draw = this.CreateGraphics();
            draw.FillRectangle(filled, xstat + x * width, ystat + (max-min - y) * height, width, height);
        }
        private void button2_Click(object sender, EventArgs e)
        {
         
            timer1.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (go == 0)
            {
                player.Play();
                enable(go);
                for(int i = 0; i < D; i++)
                {
                    if (map[go][i] < 0) break;
                    fire(go, map[go][i]);
                }
                go ++;
            }
            else if (go == row)
            {
                if (map[go - 1][0] < 0) leave(go - 1);
                else revert(go - 1);
                timer1.Enabled = false;
                player.Stop();
                for (int i = 0; i < D; i++)
                {
                    if (map[go-1][i] < 0) break;
                    paint(go-1, map[go-1][i]);
                }
                go = 0;
            }
            else
            {
                if (map[go - 1][0] < 0) leave(go - 1);
                else revert(go - 1);
                for (int i = 0; i < D; i++)
                {
                    if (map[go - 1][i] < 0) break;
                    paint(go-1, map[go-1][i]);
                }
                enable(go);
                for (int i = 0; i < D; i++)
                {
                    if (map[go][i] < 0) break;
                    fire(go, map[go][i]);
                }
                go++;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button1.BackgroundImage = Image.FromFile(@"C:\Users\Zero Weight\Documents\GitHub\Signals-System\Cs\Cs\Program Files (x86).jpg");
                button2.BackgroundImage = Image.FromFile(@"C:\Users\Zero Weight\Documents\GitHub\Signals-System\Cs\Cs\Program Files.jpg");
                button1.Text = null;
                button2.Text = null;
            }
            else
            {
                button1.BackgroundImage = null;
                button2.BackgroundImage = null;
                button1.Text = "Analyze";
                button2.Text = "Play";
            }
        }
    }
}
