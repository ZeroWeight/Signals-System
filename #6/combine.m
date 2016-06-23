function [ X ] = combine( A,B )
%UNTITLED3 Summary of this function goes here
%   Detailed explanation goes here
fa=fft(A);
fb=fft(B);
fx=fa+fb;
X=ifft(fx);
end

