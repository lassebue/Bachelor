function plotRawEMGSensorData(rawSensor1,rawSensor2,rawSensor3...
    ,rawSensor4,rawSensor5,rawSensor6,rawSensor7,rawSensor8,activity,n_obs)
% Copyright (c) 2015, MathWorks, Inc.

if nargin < 5 
    n_obs = size(rawSensor1,1);
end

g = 1;

rawTS(:,1) = reshape(rawSensor1(1:n_obs,:)',[],1);
rawTS(:,2) = reshape(rawSensor2(1:n_obs,:)',[],1);
rawTS(:,3) = reshape(rawSensor3(1:n_obs,:)',[],1);
rawTS(:,4) = reshape(rawSensor4(1:n_obs,:)',[],1);
rawTS(:,5) = reshape(rawSensor5(1:n_obs,:)',[],1);
rawTS(:,6) = reshape(rawSensor6(1:n_obs,:)',[],1);
rawTS(:,7) = reshape(rawSensor7(1:n_obs,:)',[],1);
rawTS(:,8) = reshape(rawSensor8(1:n_obs,:)',[],1);

activityType = repelem(activity(1:n_obs),128*ones(numel(activity(n_obs)),1))

% 
% hFig = figure(1);
% set(hFig, 'Position', [x y width height])


fig = figure('Name','Pose Detection','NumberTitle','off',...
    'Units','Normalized','Visible','off');
fig.Position(3:4) = [0.5,0.7]; 
movegui('center')
fig.Visible = 'on';

ax(1) = subplot(8,1,1,'Parent',fig,'Xgrid','on','Ygrid','on',...
    'YLim',[-3*g 2*g]);
ax(2) = subplot(8,1,2,'Parent',fig,'Xgrid','on','Ygrid','on',...
    'YLim',[-3*g 2*g]); 
ax(3) = subplot(8,1,3,'Parent',fig,'Xgrid','on','Ygrid','on',...
    'YLim',[-3*g 2*g]);


ax(4) = subplot(8,1,4,'Parent',fig,'Xgrid','on','Ygrid','on',...
    'YLim',[-3*g 2*g]);
ax(5) = subplot(8,1,5,'Parent',fig,'Xgrid','on','Ygrid','on',...
    'YLim',[-3*g 2*g]);
ax(6) = subplot(8,1,6,'Parent',fig,'Xgrid','on','Ygrid','on',...
    'YLim',[-3*g 2*g]);
ax(7) = subplot(8,1,7,'Parent',fig,'Xgrid','on','Ygrid','on',...
    'YLim',[-3*g 2*g]);
ax(8) = subplot(8,1,8,'Parent',fig,'Xgrid','on','Ygrid','on',...
    'YLim',[-3*g 2*g]);


clr = get(groot,'DefaultAxesColorOrder');
axes(ax(1)), gscatter(1:size(rawTS,1),rawTS(:,1),activityType,clr,'.',8,'off')
axes(ax(2)), gscatter(1:size(rawTS,1),rawTS(:,2),activityType,clr,'.',8,'off')
axes(ax(3)), gscatter(1:size(rawTS,1),rawTS(:,3),activityType,clr,'.',8,'off')

axes(ax(4)), gscatter(1:size(rawTS,1),rawTS(:,4),activityType,clr,'.',8,'on')
axes(ax(5)), gscatter(1:size(rawTS,1),rawTS(:,5),activityType,clr,'.',8,'off')
axes(ax(6)), gscatter(1:size(rawTS,1),rawTS(:,6),activityType,clr,'.',8,'off')

axes(ax(7)), gscatter(1:size(rawTS,1),rawTS(:,7),activityType,clr,'.',8,'off')
axes(ax(8)), gscatter(1:size(rawTS,1),rawTS(:,8),activityType,clr,'.',8,'off')


title(ax(1),'Sensor 1','Interpreter','none')
title(ax(2),'Sensor 2','Interpreter','none')
title(ax(3),'Sensor 3','Interpreter','none')

title(ax(4),'Sensor 4','Interpreter','none')
title(ax(5),'Sensor 5','Interpreter','none')
title(ax(6),'Sensor 6','Interpreter','none')
title(ax(7),'Sensor 7','Interpreter','none')
title(ax(8),'Sensor 8','Interpreter','none')


linkaxes(ax,'x')

grid(ax(1),'on')
grid(ax(2),'on')
grid(ax(3),'on')

grid(ax(4),'on')
grid(ax(5),'on')
grid(ax(6),'on')
grid(ax(7),'on')
grid(ax(8),'on')


a = 'Amplitude';
b = '';

ylabel(ax(1),b)
ylabel(ax(2),b)
ylabel(ax(3),b)

ylabel(ax(4),a)
ylabel(ax(5),b)
ylabel(ax(6),b)
ylabel(ax(7),b)
ylabel(ax(8),b)

xlabel(ax(1),b), ax(1).XTickLabel = [];
xlabel(ax(2),b), ax(2).XTickLabel = [];
xlabel(ax(3),b), ax(3).XTickLabel = [];

xlabel(ax(4),b), ax(4).XTickLabel = [];
xlabel(ax(5),b), ax(5).XTickLabel = [];
xlabel(ax(6),b), ax(6).XTickLabel = [];
xlabel(ax(7),b), ax(7).XTickLabel = [];
xlabel(ax(8),b), ax(8).XTickLabel = [];

% xlabel(ax(1),'Tid'), ax(1).XTickLabel = [];
% xlabel(ax(2),'Tid'), ax(2).XTickLabel = [];
% xlabel(ax(3),'Tid'), ax(3).XTickLabel = [];
% 
% xlabel(ax(4),'Tid'), ax(4).XTickLabel = [];
% xlabel(ax(5),'Tid'), ax(5).XTickLabel = [];
% xlabel(ax(6),'Tid'), ax(6).XTickLabel = [];
% xlabel(ax(7),'Tid'), ax(7).XTickLabel = [];
% xlabel(ax(8),'Tid'), ax(8).XTickLabel = [];
% 

end
