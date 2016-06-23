function [ x ] = ave( A )
%UNTITLED5 Summary of this function goes here
%   Detailed explanation goes here
x=0;
for i=1:1:length(A)
    x=x+A(i);
end
x=x/length(A);
y=0;
for i=1:1:length(A)
    y=y+A(i)*A(i);
end
y=y/length(A);
x=y-x*x;
% x=A(length(A));
end

