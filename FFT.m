% to get the FFT vector
music='C:\Users\Zero Weight\Desktop\school_song_single_Eb.wav';
txt='C:\Users\Zero Weight\Desktop\fft.txt';
f=fopen(txt,'w');
[y_,fs]=audioread(music);
y=y_(:,1);
N=16384;
for i=1:N:(length(y)-N)
    arr_time=y(i:N+i-1);
    arr_fft=fft(arr_time);
    for j=1:1:2000
        fprintf(f,'%.18f\n',abs(arr_fft(j)));
    end
end
fclose(f);