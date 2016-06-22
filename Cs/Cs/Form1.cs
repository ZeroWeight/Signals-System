using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cs
{
    
    public partial class Form1 : Form
    {
        const int L = 100;
        const int D = 10;
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
            StreamReader fileReader = new StreamReader("C:/Users/Zero Weight/Documents/GitHub/Signals-System/notes_onekey.txt");
            for (int i=0;i<L;i++)
            {
                map[i] = new int[D];
                temp = fileReader.ReadLine();
                if (temp == null) break;
                dictionary = temp.Split(' ');
                int counter = 0;
                foreach (string str in dictionary)
                {
                    map[i][counter] = int.Parse(str);
                }
            }
        }
    }
}
