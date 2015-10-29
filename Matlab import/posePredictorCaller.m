clear 
clc

[FileName,PathName,FilterIndex] = uigetfile('*.csv');    

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

[emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
        emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate] ...
    = importfilefunc( strcat(char(PathName),FileName), 2,emg1Collection...
        ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
    ,padding,cate);


posePredictor(emg1Collection(1,:),...
    emg2Collection(2,:),...
    emg2Collection(3,:),...
    emg2Collection(4,:),...
    emg2Collection(5,:),...
    emg2Collection(6,:),...
    emg2Collection(7,:),...
    emg2Collection(8,:))
