m = [3 6 9 12 15; 5 10 15 20 25; ...
     7 14 21 28 35; 11 22 33 44 55];

a = [3,342,23,55,34,5,3,4,6]; 
b = [7,12,3445,64,56,4,56,456,45];
data = [a;b]; 
csvwrite('csvlist.csv',data )
type csvlist.csv

emg1Collection = [];
emg2Collection = [];
emg3Collection = [];
emg4Collection = [];
emg5Collection = [];
emg6Collection = [];
emg7Collection = [];
emg8Collection = [];
padding = [];
cate = [];


[emg1Collection,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate] ...
= importfilefunc('/Users/lassebuesvendsen/Desktop/Matlab/newerbibi.csv', 2,emg1Collection...
,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
,padding,cate);

[emg1Collection,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate] ...
= importfilefunc('/Users/lassebuesvendsen/Desktop/Matlab/newerbibi.csv', 2,emg1Collection...
,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
,padding,cate);

% [time,emg1Collection,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding] ...
% = importfilefunc('/Users/lassebuesvendsen/Desktop/newerbibi.csv', 2,emg1Collection...
% ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
% ,padding);