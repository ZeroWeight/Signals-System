function [ X ] = match( Y,A )
B=A;
N=length(Y);
for i=N/1:1:N
    B(i)=0;
end;
yf=fft(Y);
sf=fft(B);
f=zeros(N,1);
f(1)=sf(1);
for j=1:1:N-1
    f(j+1)=sf(N-j+1)*exp(2*pi*1i*j/N);
end
xf=zeros(N,1);
for i=1:1:N
    xf(i)=f(i)*yf(i);
end
X=abs(ifft(xf));
% sum=0;
% for i=1:1:N
%     sum=sum+X(i);
% end
% sum=sum/N;
% X=X-sum;
% end

