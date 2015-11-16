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
    emg2Collection(1,:),...
    emg3Collection(1,:),...
    emg4Collection(1,:),...
    emg5Collection(1,:),...
    emg6Collection(1,:),...
    emg7Collection(1,:),...
    emg8Collection(1,:))

