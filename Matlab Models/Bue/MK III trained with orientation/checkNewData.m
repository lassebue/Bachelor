clear
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
orientation = [];
testPersons = [];

[emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
        emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate,orientation,testPersons] ...
    = importfilefuncOri( strcat(char(PathName),FileName), 2,emg1Collection...
        ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
    ,padding,cate,orientation,testPersons);

emg1Collection = emg1Collection.^2;
emg2Collection = emg2Collection.^2;
emg3Collection = emg3Collection.^2;
emg4Collection = emg4Collection.^2;
emg5Collection = emg5Collection.^2;
emg6Collection = emg6Collection.^2;
emg7Collection = emg7Collection.^2;
emg8Collection = emg8Collection.^2;

rawSenorData = table(emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
emg5Collection,emg6Collection,emg7Collection,emg8Collection);

meanData    = varfun(@windMean,rawSenorData);
stdData     = varfun(@windStd,rawSenorData);
cpaData     = varfun(@windPca,rawSenorData); 

newPoseTrainingData = [meanData stdData cpaData table(orientation)];
%poseTrainingData.pose = cate;
load('baggedTreesMkIIICompacted')
abe = baggedTreesMkIIICompacted.predictFcn(newPoseTrainingData);

FileName
summary(abe)

% [x,y] = myfunc(3,6,'world')
