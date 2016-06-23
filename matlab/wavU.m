% ��wav�ļ�ת��Ϊ����
% ���񿵣�2016.5.27
% ����Matlab2014����

% ע�⣬�������еĸ������Ѿ���������ÿ����80.7�ģ�����ӦFFT����Ҫ
% ��������֡���ֱ���и���Ҷ�任
    % ��ȡ��Ƶ�� f0=44100Hz ����
    % һ֡����16384������ʱ��������0.372s��Ƶ�ʾ���Ϊ2.69Hz
    % ���ļ����е��ǵ�����ʶ��ȡ�÷�Χ����ǿƵ�ʣ������²���2~4���ҵ���Ƶ

% �������������ļ���
file = 'piano';

wav_file = strcat(file,'.wav');
output_file_name = strcat(file,'.txt');
[SIGNAL,FREQ]=audioread(wav_file); 

LEN=16384;  %����Ҷ�任ȡ�����ȣ���һ֡������������
MAXF=700;   %���ʶ��Ƶ��
MINF=180;   %���ʶ��Ƶ��
MINA=10;    %������

FREQ_LIST=linspace(0,FREQ/2,LEN/2); %���0��FREQ/2, ��LEN/2�����ֵĵȲ�����

n=int32(length(SIGNAL)/LEN); %��֡��

notes_list = zeros(1, n);       
freqs_list = zeros(1, n);
maxA_list = zeros(1, n);    %��¼������

MAX_INDEX=int32(LEN/FREQ*MAXF); %���ʶ��Ƶ�ʶ�Ӧ�ĸ���
MIN_INDEX=int32(LEN/FREQ*MINF); %���ʶ��Ƶ�ʶ�Ӧ�ĸ���

key=['G3 ';'G3#';'A3 ';'A3#';'B3 '];
key=[key;'C4 ';'C4#';'D4 ';'D4#';'E4 ';'F4 ';'F4#';'G4 ';'G4#';'A4 ';'A4#';'B4 '];
key=[key;'C5 ';'C5#';'D5 ';'D5#';'E5 ';'F5 ';'F5#';'G5 ';'G5#';'A5 ';'A5#';'B5 '];
key=[key;'C6 ';'C6#';'D6 ';'D6#';'E6 ';'F6 ';'F6#';'G6 ';'G6#';'A6 ';'A6#';'B6 '];

%�ֶν��и���Ҷ����
for i = 1:n, 
    lX = fft( SIGNAL( (i-1)*LEN+1 : i*LEN ) );  %��ȡ����Ϊl��һ�����ݵĿ��ٸ���Ҷ�任    
    lX_cut = lX(1:MAX_INDEX);                   %��Ƶ��ֹ
    
    lA = sqrt(lX_cut.*conj(lX_cut));      %ȡģ
    if lA > 0
        lA = 20*log10(lA);        %dB
    end
    
    max_index = MIN_INDEX;
    maxA_list(i) = lA(MIN_INDEX);
    
    for t=MIN_INDEX:MAX_INDEX
        if (lA(t)>maxA_list(i))
            maxA_list(i) = lA(t);
            max_index = t;
        end
    end
    
    best_index = max_index;
    for mult=2:4,                  %�����²���2~4���ҵ���Ƶ
        test_index = int32(max_index/mult);
        if (test_index>MIN_INDEX)
            if (lA(test_index) > maxA_list(i)*0.9) 
                best_index = int32(max_index/mult);
            end
        else
            break;            
        end
    end
    freqs_list(i) = FREQ_LIST( best_index );    %������ȶ�Ӧ��Ƶ��
    notes_list(i) = log(freqs_list(i)/220) / log(2) * 12 + 3; %��������
    
    if maxA_list(i)<MINA || notes_list(i)<-12
        notes_list(i)=NaN;
    end
end

% ȥ�����̫�͵ĵ�
for i = 1:n,
    if maxA_list(i)<MINA || notes_list(i)<-12
        notes_list(i)=NaN;
    end
end

% ������ļ�
output_file = fopen(output_file_name,'w');
for i=1:n
    if notes_list(i)>0
        fprintf(output_file,'%d\r\n',round(notes_list(i)));
    else
        fprintf(output_file,'0\r\n');
    end
end
fclose(output_file);

K=[0,0,0,0,0,0,0,0,0,0,0,0];  %��������1

%���ԭ����
fprintf('��������\n');
for i=1:n,
    %try
        if notes_list(i)==notes_list(i) % not NaN
            p = round(notes_list(i));
            if p>0
                for j = 1:3
                        fprintf('%c',key(p,j));
                end
            else
                fprintf('%d ',p);
            end
            fprintf(' ');
            K(mod(p-1,12)+1)=K(mod(p-1,12)+1)+1;
        else
            fprintf('??? ');
        end
    if mod(i,16)==0 
        fprintf('\n'); 
    end
end

h7=[0,2,4,5,7,9,11];
w7=[3,2,3,1,3,3,1];
maxSc=0;  % ����ͳ�ƶ���
bestMc=6;
for mc=6:17,
    sc=0;
    for i=1:length(h7)
        sc=sc+K(mod(mc+h7(i)-1,12)+1)*w7(i);
    end
    if maxSc<sc
        maxSc=sc;
        bestMc=mc;
    end
end

%����޶�����
fprintf('\n\n���ף�\n1=');
for j=1:3
    fprintf('%c',key(bestMc,j));
end
fprintf('\n');
for i=1:n,
    try
        if notes_list(i)==notes_list(i) 
            p=notes_list(i);            
            p=p-bestMc;
            
            while p<-0.5
                fprintf('_');
                p=p+12;
            end
            while p>=11.5
                fprintf('^');
                p=p-12;
            end            
            
            if p<1
                fprintf('1');                
            elseif p<3
                fprintf('2');
            elseif p<4.5
                fprintf('3');
            elseif p<6
                fprintf('4');
            elseif p<8
                fprintf('5');
            elseif p<10
                fprintf('6');
            else
                fprintf('7');
            end
            
            fprintf('\t');
            
            K(mod(p-1,12)+1)=K(mod(p-1,12)+1)+1;
        else
            fprintf('??\t');
        end
    catch
    end
    if mod(i,16)==0 
        fprintf('\n'); 
    end
end

fprintf('\n��Ƶ��������ɡ�\n\n');

%subplot(2,2,1)
%plot(notes_list)
%subplot(2,2,3)
%plot(SIGNAL)
%subplot(2,2,2)
%hist(notes_list)