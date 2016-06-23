%main
clc;
clear all;
disp('hi');
N=16384;
M=32;
[step_origin,fs]=audioread('C:\Users\Zero Weight\Documents\GitHub\Signals-System\music\step.wav');
left_origin=step_origin(:,1);
left_step=left_origin(1:M*N,1);
left_mat=reshape(left_step,N,M);
right_origin=step_origin(:,1);
right_step=right_origin(1:M*N,1);
right_mat=reshape(right_step,N,M);
standard=zeros(N,M/2);
for i=1:1:M/2
    standard(:,i)=average(right_mat(:,i*2),right_mat(:,i*2-1),left_mat(:,i*2),left_mat(:,i*2-1));
end
[music_origin,fs]=audioread('C:\Users\Zero Weight\Documents\GitHub\Signals-System\music\school_song_double_C.wav');
left_music_origin=music_origin(:,1);
n=floor(length(left_music_origin)/N);
left_music_step=left_music_origin(1:n*N,1);
left_music_mat=reshape(left_music_step,N,n);
right_music_origin=music_origin(:,1);
right_music_step=right_music_origin(1:n*N,1);
right_music_mat=reshape(right_music_step,N,n);
music=zeros(N,n);
mat=zeros(16);
format=zeros(16);
tao=0.0;
for i=1:1:n
    music(:,i)=combine(right_music_mat(:,i),left_music_mat(:,i));
end
for I=1:1:n
    for i=1:1:16
        mat(i)=ave(match(standard(:,i),music(:,I)))*ave(match(standard(:,i),music(:,I)))/ave(match(music(:,I),music(:,I)))/ave(match(standard(:,i),standard(:,i)));
    end
    sound(music(:,I),fs); 
    plot(mat-format*tao);
    format=mat;
    pause()
end

