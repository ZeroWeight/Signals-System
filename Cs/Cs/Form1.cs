using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
namespace Cs
{
    
    public partial class Form1 : Form
    {
        // 颜色
        private SolidBrush background = new SolidBrush(Color.White);
        private SolidBrush filled = new SolidBrush(Color.FromArgb(71,(0xFF+Color.Blue.R-Color.Yellow.R)&0xFF, (0xFF + Color.Blue.G - Color.Yellow.G) & 0xFF, (0xFF + Color.Blue.B - Color.Yellow.B) & 0xFF));
        private SolidBrush active = new SolidBrush(Color.Red);
        private SolidBrush blank = new SolidBrush(Color.White);
        private SolidBrush going = new SolidBrush(Color.Yellow);
        private SolidBrush rawbackground = new SolidBrush(SystemColors.Control);
        private Graphics draw;

        // 音频
        SoundPlayer player;
        
        // 基本尺寸
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
        
        // 构造函数
        public Form1()
        {
            InitializeComponent();
        }

        // 窗口启动后执行
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        // 读取音符文件
        private void Read_In(string input_file_name)
        {
            try
            {
                min = 1000;
                max = 0;
                row = 0;
                StreamReader fileReader = new StreamReader(input_file_name); 
                for (int i = 0; i < L; i++)
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
                        if (map[i][counter] < min && map[i][counter] != 0) min = map[i][counter];
                        if (map[i][counter] > max) max = map[i][counter];
                        counter++;
                    }
                    row++;
                }
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < D; j++)
                    {
                        map[i][j] -= min;
                    }
                }
            }
            catch
            {
                MessageBox.Show("对不起，音符文件加载出现问题。请检查文件。");
            }
        }

        // 加载音符文件
        private void button1_Click(object sender, EventArgs e)
        {
            // 加载音符文件
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "MATLAB产生的音符文件|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Read_In(ofd.FileName);
            }
            else return;

            pictureBox1.Paint += new PaintEventHandler(Init);

            pictureBox1.CreateGraphics().FillRectangle(rawbackground, 0, 0, Width, Height);
            pictureBox1.CreateGraphics().FillRectangle(background, xstat, ystat, width * row, height * (max - min + 1));
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


            // 加载乐谱
            try
            {
                string file_name = ofd.FileName;
                player = new SoundPlayer(file_name.Replace(".txt", ".wav"));
            }
            catch
            {
                MessageBox.Show("无法加载相应的wav文件。请确认txt文件与wav文件名称相同:");
                button1.Enabled = true;
                button2.Enabled = false;
                return;
            }

            button2.Enabled = true;
        }
        private void Init(object sender, PaintEventArgs e)
        {
            pictureBox1.CreateGraphics().FillRectangle(background, xstat, ystat, width * row, height * (max - min + 1));
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
            pictureBox1.CreateGraphics().FillRectangle(blank, xstat + x * width, ystat, width, height * (max - min + 1));
        }
        private void enable(int x)
        {
            pictureBox1.CreateGraphics().FillRectangle(going, xstat + x * width, ystat, width, height * (max - min + 1));
        }
        private void revert(int x)
        {
            pictureBox1.CreateGraphics().FillRectangle(background, xstat + x * width, ystat, width, height * (max - min + 1));
        }
        private void fire(int x,int y)
        {
            draw = pictureBox1.CreateGraphics();
            draw.FillRectangle(active, xstat + x * width, ystat + (max - min - y) * height, width, height);
        }
        private void paint(int x,int y)
        {
            draw = pictureBox1.CreateGraphics();
            draw.FillRectangle(filled, xstat + x * width, ystat + (max-min - y) * height, width, height);
        }

        // 同步播放音频
        private void button2_Click(object sender, EventArgs e)
        {
            //OpenFileDialog ofd = new OpenFileDialog();
            //ofd.Filter = "wav音频文件|*.wav";
            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    player = new System.Media.SoundPlayer(ofd.FileName);
            //}
            //else return;

            timer1.Enabled = true;
            button1.Enabled = false;
            button2.Enabled = false;
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
                button1.Enabled = true;
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
        
    }
}
