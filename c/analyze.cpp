/* Created by sugar10w, 2016.06.22
 *
 * 从傅里叶变换的结果中分析谐波关系，确定音符
 *
 * 输入文件：fft.txt
 *   44100Hz，N=16384的FFT的前2000项
 * 输出文件：notes.txt
 *   (相对)音高的数值，一帧一行
 * 
 */

#include<cmath>

#include<iostream>
#include<fstream>
#include<vector>

using namespace std;

// length of one set of data
const int LENGTH = 2000;
// from index of fft to frequency
const double RATIO = 44100.0/16394.0;
// base frequency which refer to 0 in output
const double BASE_FREQ = 55.0;

// minium data of the notes detected
const double MIN_DATA = 10.0;

// read in 2000 data; 
//  return false if failed to read 2000 numbers
bool loadData(double *data, ifstream& in_file)
{
    double * p = data;
    int i = 0;
    while (!in_file.eof() && i++<LENGTH ) in_file  >> *p++;
    return i > LENGTH;
}

// analyze the data, return vector of notes detected
//  return false if no notes detected.
bool analyzeData(double *data, vector<int>& notes)
{
    // 最基本的方法,找到最大值之后，按2-4谐波查找
    double max_data = MIN_DATA;
    int max_index = -1, sub_max_index = -1;

    for (int i=0; i<LENGTH; ++i)
    {
        if (data[i]>max_data)
        {
            max_data = data[i];
            max_index = i;
        }
    }

    if (max_index == -1) return false;

    sub_max_index = max_index;

    for (int k=2; k<=4; ++k)
    {
        if (data[max_index/k] > max_data/4)
        {
            sub_max_index = max_index/k;
        }
    }

    
    int note = 
        int( log( RATIO * sub_max_index / BASE_FREQ ) / log (2) * 12.0 );

    notes.push_back(note);

    return true;
}

int main()
{
    // IO
    ifstream in_file("fft.txt");
    ofstream out_file("notes.txt");

    // 2000 double
    double data[LENGTH];

    // results
    vector<int> notes;

    int i = 0; 

    while (loadData(data, in_file))
    {
        notes.clear();

        if (!analyzeData(data, notes))
        {
            // TODO: output what?
            out_file << "0" << endl;
        }
        else
        {
            for (vector<int>::iterator note = notes.begin(); note!=notes.end(); ++note)
                out_file << *note << ' ' ; 
            out_file << endl;
        }

        cout << "========== frame "<< i++ << " finished, " 
            << notes.size() <<" notes detected. ==========" << endl;
    }

    system("pause");

    return 0;
}