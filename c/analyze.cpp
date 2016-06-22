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
const double MIN_DATA = 20.0;

// read in 2000 data; 
//  return false if failed to read 2000 numbers
bool loadData(double *data, ifstream& in_file)
{
    double * p = data;
    int i = 0;
    while (!in_file.eof() && i++<LENGTH ) in_file  >> *p++;
    return i > LENGTH;
}


//------------------------------------------------------------------

vector<int> importantIndex;
const int STRAT_NOTE = 15;
const int LAST_NOTE = 48;

// index of fft and note
int indexToNote(int index)
{
    return int( log( RATIO * index / BASE_FREQ ) / log (2) * 12.0 + 0.5 );
}
int noteToIndex(int note)
{
    return  int ( BASE_FREQ * pow(2, (double)note/12.0) / RATIO + 0.5 );
}

void initAnalyzeData()
{
    for (int note = STRAT_NOTE; note < LAST_NOTE; ++note) importantIndex.push_back(noteToIndex(note));

}

// analyze the data, return vector of notes detected
//  return false if no notes detected.
bool analyzeData(double *data, vector<int>& notes)
{
    for (int i=0; i<importantIndex.size(); ++i)
    {
        int index = importantIndex[i];
        if (data[index]>MIN_DATA) notes.push_back(i+STRAT_NOTE);
        
        //主动衰减高次谐波
        //double decline = data[index] / 2;
        //data[index*2-1] = data[index+12] - decline;
        //data[index*2  ] = data[index+12] - decline;
        //data[index*2+1] = data[index+12] - decline;
        //data[index*3-1] = data[index+12] - decline;
        //data[index*3  ] = data[index+12] - decline;
        //data[index*3+1] = data[index+12] - decline;
        //data[index*4-1] = data[index+12] - decline;
        //data[index*4  ] = data[index+12] - decline;
        //data[index*4+1] = data[index+12] - decline;

    }

    if (notes.size()==0) return false;
    return true;
}

//------------------------------------------------------------------

int main()
{
    // IO
    ifstream in_file("fft.txt");
    ofstream out_file("notes.txt");

    // 2000 double
    double data[LENGTH];

    // results
    vector<int> notes;

    //for (int i=10; i<50; ++i)
    //{
    //    cout<< i<< " "
    //        << noteToIndex
    //}


    int i = 0; 



    initAnalyzeData();
    for (int i=0; i<importantIndex.size(); ++i)
    {
        cout<<"idx "<<i+10<<"  "<<importantIndex[i]<<endl;
    }

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


    return 0;
}