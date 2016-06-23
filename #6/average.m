function [ X ] = average( A,B,C,D )
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here
fa=fft(A);
fb=fft(B);
fc=fft(C);
fd=fft(D);
fx=fa+fb+fc+fd;
X=ifft(fx);
end

