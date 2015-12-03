clc
% getModelPoseIds()
[FileName,PathName,FilterIndex] = uigetfile('*.csv','MultiSelect','on');

n_obs = 1500;


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
            = importfilefuncOri( strcat(char(PathName),char(FileName(k))), 2,emg1Collection...
                ,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
            ,padding,cate,orientation,testPersons);
        end
    end
    
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
    
    
plotRawEMGSensorData(emg1Collection,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection,theCategories,n_obs)