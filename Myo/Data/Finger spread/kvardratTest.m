clc
close all
clear
format long
load('finger spread data.mat')

dataSize = length(emg1)
totalTime = 5* dataSize
time = linspace(0,totalTime,dataSize);


% Making the intervals - 50 ms 

% Sample frequensy
fs = 200;               %----------------- change sample freq here.


% The periode of the samples 
sPeriode = 1/fs;
% Samples in interval
intervalS = 10;         %----------------- Change interval size

% Interval time in ms
intervalT = sPeriode * intervalS*1000;


sampleCounter = 1;
intervalAmount = ceil(length(emg1)/intervalS);
intervalCounter = 1;
intervalValue = 0;

allIntervals = zeros(1,intervalAmount);

emg1 = emg1.^2;
emg2 = emg2.^2;
emg3 = emg3.^2;
emg4 = emg4.^2;
emg5 = emg5.^2;
emg6 = emg6.^2;
emg7 = emg7.^2;
emg8 = emg8.^2;

%Construckting intevals
for k=1:length(emg1)
    intervalValue =intervalValue + emg1(k);
    sampleCounter = sampleCounter+1;
    
    if(sampleCounter == intervalS)
        
        allIntervals(intervalCounter) = intervalValue/10;
        
        intervalCounter = intervalCounter+1;
        sampleCounter = 1;
        
        intervalValue = 0;
        
    end
    if (intervalCounter == intervalAmount )
        break;
    end
end


%allIntervals

% 


allData = [emg1,emg2,emg3,emg4,emg5,emg6,emg7,emg8];

% Setting the time of the intervals
intervalTime = linspace(0,intervalT*intervalAmount,intervalAmount);
% intervalTime+25;
intervalTime;

figure(1)
plot(time,emg1)%'b-*'

% hold on
% plot(time,emg2)
% hold on
% plot(time,emg3)
% hold on
% plot(time,emg4)
% hold on
% plot(time,emg5)
% hold on
% plot(time,emg6)
% hold on
% plot(time,emg7)
% hold on
% plot(time,emg8)
hold on
x = 0:50:100700;
length(x)

allIntervals(2015) = 0;
y = allIntervals;
length(y)
bar(x,y)
% plot(intervalTime,allIntervals,'-*')
% 
% figure(2)
% plot(timestamp,emg1);
% % soundsc(emg1,850)
% 
% data = spread1;
% 
% b = [];
% time =  data(:,1);
% for k=1:length(time)
%     if isempty(b)
%         b(1) = time(k);
%     else if mod(k,2) == 0
%         b(end+1) = time(k)+1;
%         else
%         b(end+1) = time(k);
%         end
%     end
% %     if((mod(k,2)==0)))
% end
% b = transpose(b);
% 
% offset = b(1);
% time = b - offset;
% 
% emg  = data(:,2);
% %  emg = abs(emg);
% 
% x =[];
% y = [];
% figure(3)
% plot(time,emg)
% % [y,x] = findpeaks(emg,time);
% % 
% % 
% % [y,x] = findpeaks(y,x);
% % 
% % plot(x,y)
% % 
% 


% 