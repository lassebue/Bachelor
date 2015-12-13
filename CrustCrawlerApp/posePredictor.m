function abe = posePredictor(emg1CollectionInput,emg2CollectionInput,emg3CollectionInput,...
    emg4CollectionInput,emg5CollectionInput,emg6CollectionInput,emg7CollectionInput,emg8CollectionInput)



emg1Collection = [];
emg2Collection = [];
emg3Collection = [];
emg4Collection = [];
emg5Collection = [];
emg6Collection = [];
emg7Collection = [];
emg8Collection = [];

emg1Collection = [emg1Collection ; (emg1CollectionInput).^2];
emg2Collection = [emg2Collection ; (emg2CollectionInput).^2];
emg3Collection = [emg3Collection ; (emg3CollectionInput).^2];
emg4Collection = [emg4Collection ; (emg4CollectionInput).^2];
emg5Collection = [emg5Collection ; (emg5CollectionInput).^2];
emg6Collection = [emg6Collection ; (emg6CollectionInput).^2];
emg7Collection = [emg7Collection ; (emg7CollectionInput).^2];
emg8Collection = [emg8Collection ; (emg8CollectionInput).^2];



rawSenorData = table(emg1Collection,emg2Collection,emg3Collection,emg4Collection,...
emg5Collection,emg6Collection,emg7Collection,emg8Collection);

meanData    = varfun(@windMean,rawSenorData);
stdData     = varfun(@windStd,rawSenorData);
cpaData     = varfun(@windPca,rawSenorData); 


PoseTrainingData = [meanData stdData cpaData];

load('SubSpaceModel')
predictedCats = subspaceKNN.predictFcn(PoseTrainingData)  %(newPoseTrainingData);

if(predictedCats(1) == 1)
    abe = 0;
elseif(predictedCats(1) == 2)
    abe = 1;
elseif(predictedCats(1) == 3)
    abe = 2;
end