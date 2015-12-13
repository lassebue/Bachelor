clc
% [FileName,PathName,FilterIndex] = uigetfile('*.csv','MultiSelect','on');
load('FileNames');

PathName = '/Users/lassebuesvendsen/Documents/Bachelor/Plot EMG data/PlotMean/';


n_obs = 400;

emg1Collection = [];
emg2Collection = [];
emg3Collection = [];
emg4Collection = [];
emg5Collection = [];
emg6Collection = [];
emg7Collection = [];
emg8Collection = [];
padding        = [];
cate           = [];
testPersons    = [];
orientation    = [];

    if(ischar(FileName))
    [emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
        emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate,orientation,testPersons] ...
    = importfilefuncOri( strcat(char(PathName),FileName), 2,emg1Collection...
        ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
    ,padding,cate,orientation,testPersons);
    else
        for k=1:length(FileName);
            [emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
                emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate,orientation,testPersons] ...
            = importfilefuncOri( char(FileName(k)), 2,emg1Collection...
                ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
            ,padding,cate,orientation,testPersons);
        end
    end
    % strcat(char(PathName),char(FileName(k)))
    % Finding the unique pose id's from the data    
id = unique(sort(cate)) 

% 
poses = cate;

names = {};
for i=1:length(id)
    names{end+1} = int2str(id(i));
end

theCategories = categorical(poses,id,names,'Ordinal',true);

% abe = getPoseId(theCategories,theCategories(6))

summary(theCategories)

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

pcaData    = varfun(@windPca,rawSenorData);
    
plotPcaSensorData(pcaData{:,1},pcaData{:,2},pcaData{:,3},pcaData{:,4},pcaData{:,5},pcaData{:,6},pcaData{:,7},pcaData{:,8},theCategories,n_obs)