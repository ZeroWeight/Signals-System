%main
clc;
clear all;
disp('hi');
N=16384;
M=32;
[step_origin,fs]=audioread('C:\Users\Zero Weight\Documents\GitHub\Signals-System\music\step.wav');
F=audioread('C:\Users\Zero Weight\Documents\GitHub\Signals-System\music\F#.wav');
left_origin=step_origin(:,1);
left_step=left_origin(1:M*N,1);
left_mat=reshape(left_step,N,M);
right_origin=step_origin(:,1);
right_step=right_origin(1:M*N,1);
right_mat=reshape(right_step,N,M);
standard=zeros(N,M/2);
next=[8,16;9,0;10,0;11,0;13,0;14,0;15,0;1,16;2,0;3,0;4,0;0,0;5,0;6,0;7,0;1,8];
for i=1:1:11
    standard(:,i)=average(right_mat(:,i*2),right_mat(:,i*2-1),left_mat(:,i*2),left_mat(:,i*2-1));
end
standard(:,12)=average(F(1:N,1),F(1:N,2),F(N+1:2*N,1),F(N+1:2*N,1));
for(i=12:1:M/2-1)
    standard(:,i+1)=average(right_mat(:,i*2),right_mat(:,i*2-1),left_mat(:,i*2),left_mat(:,i*2-1));
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
for i=1:1:n
    music(:,i)=combine(right_music_mat(:,i),left_music_mat(:,i));
end
mat=zeros(16,n);
plot(match(standard(:,8),standard(:,1)));
return
for I=1:1:n
    for J=1:1:16
        mat(J,I)=ave(match(standard(:,J),music(:,I)))*ave(match(standard(:,J),music(:,I)))/ave(match(music(:,I),music(:,I)))/ave(match(standard(:,J),standard(:,J)));
    end
%     plot(mat(:,I));
%     pause;
end
answer=zeros(8,n);
div=0.2;
for I=1:1:n
    all=1;
    max=0;
    for J=1:1:16
        if max<mat(J,I) 
            max=mat(J,I);
        end
    end
    while true
        rank=1;
        for J=1:1:16
            if mat(J,I)>mat(rank,I)
                rank=J;
            end
        end
        if mat(rank,I)<div*max
            break;
        end
        answer(all,I)=rank;
        all=all+1;
        mat(rank,I)=0;
        for K=1:1:2
            if next(rank,K)>0
                mat(next(rank,K),I)=0;
            end
        end
    end
end
txt='.\output.txt';
f=fopen(txt,'w');
for i=1:1:n
    for j=1:1:8
        fprintf(f,'%d ',answer(j,i));
    end
     fprintf(f,'\n');
end
fclose(f);