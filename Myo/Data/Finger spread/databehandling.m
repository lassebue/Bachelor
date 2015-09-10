clc
close all

format long
time = timestamp;
format long
offset = time(1);
time = time - offset;
% 
% plot(time,emg1)
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

plot(timestamp,emg1);
soundsc(emg1,850)

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
%  emg = abs(emg);
% 
% x =[];
% y = []; 
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
% 
% 
 power = [ 11 12 18 21 22 24 28 32 ...
                   37 39 41 46 48 50 51 ];
