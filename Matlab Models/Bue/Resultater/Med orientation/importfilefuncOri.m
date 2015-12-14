function [emg1Collection,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection,padding,cate,orientation,testPersons] =...
    importfilefuncOri(filename, startRow,emg1Collection...
,emg2Collection,emg3Collection,emg4Collection,emg5Collection,emg6Collection,emg7Collection,emg8Collection...
,padding,cate,orientation,testPersons)
%IMPORTFILE Import numeric data from a text file as column vectors.
%   [TIME,EMG1,EMG2,EMG3,EMG4,EMG5,EMG6,EMG7,EMG8,HAND,POSE,ORIENTATION,TESTPERSON]
%   = IMPORTFILE(FILENAME) Reads data from text file FILENAME for the
%   default selection.
%
%   [TIME,EMG1,EMG2,EMG3,EMG4,EMG5,EMG6,EMG7,EMG8,HAND,POSE,ORIENTATION,TESTPERSON]
%   = IMPORTFILE(FILENAME, STARTROW, ENDROW) Reads data from rows STARTROW
%   through ENDROW of text file FILENAME.
%
% Example:
%   [time,emg1,emg2,emg3,emg4,emg5,emg6,emg7,emg8,hand,pose,orientation,testPerson] = importfile('newerbibi.csv',2, 1103);
%
%    See also TEXTSCAN.

% Auto-generated by MATLAB on 2015/10/17 14:28:34

%% Initialize variables.
delimiter = ',';
% if nargin<=2
%     startRow = 2;
%     endRow = inf;
% end

%% Format string for each line of text:
%   column1: double (%f)
%	column2: double (%f)
%   column3: double (%f)
%	column4: double (%f)
%   column5: double (%f)
%	column6: double (%f)
%   column7: double (%f)
%	column8: double (%f)
%   column9: double (%f)
%	column10: text (%s)
%   column11: text (%s)
%	column12: text (%s)
%   column13: text (%s)
% For more information, see the TEXTSCAN documentation.
formatSpec = '%f%f%f%f%f%f%f%f%f%s%s%s%s%[^\n\r]';

%% Open the text file.
fileID = fopen(filename,'r');

%% Read columns of data according to format string.
% This call is based on the structure of the file used to generate this
% code. If an error occurs for a different file, try regenerating the code
% from the Import Tool.
% dataArray = textscan(fileID, formatSpec, endRow(1)-startRow(1)+1, 'Delimiter', delimiter, 'HeaderLines', startRow(1)-1, 'ReturnOnError', false);
% for block=2:length(startRow)
%     frewind(fileID);
%     dataArrayBlock = textscan(fileID, formatSpec, endRow(block)-startRow(block)+1, 'Delimiter', delimiter, 'HeaderLines', startRow(block)-1, 'ReturnOnError', false);
%     for col=1:length(dataArray)
%         dataArray{col} = [dataArray{col};dataArrayBlock{col}];
%     end
% end


%% Read columns of data according to format string.
% This call is based on the structure of the file used to generate this
% code. If an error occurs for a different file, try regenerating the code
% from the Import Tool.
dataArray = textscan(fileID, formatSpec, 'Delimiter', delimiter, 'HeaderLines' ,startRow-1, 'ReturnOnError', false);


%% Close the text file.
fclose(fileID);

%% Post processing for unimportable data.
% No unimportable data rules were applied during the import, so no post
% processing code is included. To generate code which works for
% unimportable data, select unimportable cells in a file and regenerate the
% script.

%% Allocate imported array to column variable names
time = dataArray{:, 1};
windowSize = 128;
emg1 = dataArray{:, 2};
[data,paddingTmp] = vec2mat(transpose(emg1),windowSize);


emg1Collection = [emg1Collection ; data];
% Variable to check if the data sets has zero padding, if so it will be
% remove later
padding = [padding ; paddingTmp];


emg2 = dataArray{ :, 3};
[data] = vec2mat(transpose(emg2),windowSize);
emg2Collection = [emg2Collection ; data];

emg3 = dataArray{:, 4};
[data] = vec2mat(transpose(emg3),windowSize);
emg3Collection = [emg3Collection ; data];

emg4 = dataArray{:, 5};
[data] = vec2mat(transpose(emg4),windowSize);
emg4Collection = [emg4Collection ; data];

emg5 = dataArray{:, 6};
[data] = vec2mat(transpose(emg5),windowSize);
emg5Collection = [emg5Collection ; data];

emg6 = dataArray{:, 7};
[data] = vec2mat(transpose(emg6),windowSize);
emg6Collection = [emg6Collection ; data];

emg7 = dataArray{:, 8};
[data] = vec2mat(transpose(emg7),windowSize);
emg7Collection = [emg7Collection ; data];

emg8 = dataArray{:, 9};
[data] = vec2mat(transpose(emg8),windowSize);
emg8Collection = [emg8Collection ; data];


hand = cell2mat ( dataArray{:, 10});
pose = cell2mat (dataArray{:, 11});

orientationNew = char(dataArray{:, 12});
orientationNew = str2num(orientationNew(1,:));

testPersonNew = char((dataArray{:, 13}));

hand = str2num(hand(1));
pose = str2num(pose(1));

% Data windows collected
antal = length(data(:,1));

% Remove data window with  padding.
if(paddingTmp ~= 0)
    emg1Collection = emg1Collection(2:end,:);
    emg2Collection = emg2Collection(2:end,:);
    emg3Collection = emg3Collection(2:end,:);
    emg4Collection = emg4Collection(2:end,:);
    emg5Collection = emg5Collection(2:end,:);
    emg6Collection = emg6Collection(2:end,:);
    emg7Collection = emg7Collection(2:end,:);
    emg8Collection = emg8Collection(2:end,:);
    antal = antal-1;
end

testPersonNew(1,:);
% categorical(A,catValues,catNames,'Ordinal',true)
for k=1:antal
    cate = [cate; pose];
    orientation = [ orientation; orientationNew];
    testPersons = [testPersons ; testPersonNew(1,:)];
end




% x = data(2:end,:);
% x

