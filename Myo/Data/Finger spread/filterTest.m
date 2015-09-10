clc
close all
format long
time = timestamp;
format long
offset = time(1);
time = time - offset;
fs = 200;
plot(time,emg1)
% hold on
% plot(time,emg2)
% hold on
% plot(time,emg3)
% hold on
% plot(time,emg4)
% hold onc
% plot(time,emg5)
% hold on
% plot(time,emg6)
% hold on
% plot(time,emg7)
% hold on
% plot(time,emg8)
plot(timestamp,emg1);
data = spread1;

format long
time = timestamp;
format long
offset = time(1);
time = time - offset;
figure(1)

plot(time,emg1)
figure(2)

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
% figure(1)
% plot(timestamp,emg1);
% data = spread1;

b = [];
time =  data(:,1);
for k=1:length(time)
    if isempty(b)
        b(1) = time(k);
    else if mod(k,2) == 0
        b(end+1) = time(k)+1;
        else
        b(end+1) = time(k);
        end
    end
%     if((mod(k,2)==0)))
end
b = transpose(b);

offset = b(1);
time = b - offset;

emg  = data(:,2);
plot(time,emg)


% fft_data = fft(emg);
% plot(fft_data)
% plot(abs(fft_data), 'magenta')

% filter stuff ------------------------
order_low = 6;
cutoff_frequency_lowpass = 40 / (fs/2);

[coef_1_low, coef_2_low] = butter(order_low, cutoff_frequency_lowpass, 'low');

 fvtool(coef_1_low, coef_2_low, 'Fs', fs)

low_filtered_e = filter(coef_1_low, coef_2_low, data);

plot( low_filtered_e)
title('Lavpas 30Hz - 4. Orden')
xlabel('tid')

% soundsc(low_filtered_e)
% % 
low_filtered_fft = fft(low_filtered_e, 200);
% % 
% % figure('Name', 'Lowpass filtered EMG signal')
figure(3)
plot(abs(low_filtered_fft), 'r')
figure(4)
absfilter = abs(ifft(low_filtered_fft));
plot(absfilter)
% peaks = findpeaks(absfilter);
% figure(5)
% plot(peaks)

% % title('EKG signal lavpasfiltreret')


% axis([-10 350 -1000 35000])
% xlabel('Frekvens (Hz)')







