% m = [3 6 9 12 15; 5 10 15 20 25; ...
%      7 14 21 28 35; 11 22 33 44 55];
% 
% a = [3,342,23,55,34,5,3,4,6]; 
% b = [7,12,3445,64,56,4,56,456,45];
% csvwrite('csvlist.csv',data )
% type csvlist.csv

[FileName,PathName,FilterIndex] = uigetfile('*.csv','MultiSelect','on');


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

    if(ischar(FileName))
    [emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
        emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate,orientation] ...
    = importfilefuncOri( strcat(char(PathName),FileName), 2 ,emg1Collection...
        ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
    ,padding,cate,orientation);
    else
        for k=1:length(FileName);
            [emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
                emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate] ...
            = importfilefuncOri( strcat(char(PathName),char(FileName(k))), 2,emg1Collection...
                ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
            ,padding,cate,orientation);
        end
    end
    
summary(cate)


% [emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
%     emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate] ...
% = importfilefunc('/Users/lassebuesvendsen/Documents/Bachelor/Bachelor/Matlab import/newerbibi.csv', 2,emg1Collection...
% ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
% ,padding,cate);
% 
% [emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
%     emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate] ...
% = importfilefunc('/Users/lassebuesvendsen/Documents/Bachelor/Bachelor/Matlab import/newerbibi.csv', 2,emg1Collection...
% ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
% ,padding,cate);

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

poseTrainingData = [meanData stdData cpaData,table(orientation)];
poseTrainingData.pose = cate;

classificationLearner

% [time,emg1Collection,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding] ...
% = importfilefunc('/Users/lassebuesvendsen/Desktop/newerbibi.csv', 2,emg1Collection...
% ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
% ,padding);