    fullPathToHeader = 'dynamixel.h';
    fullPathToLibrary = 'dynamixel';
    loadlibrary(fullPathToLibrary,fullPathToHeader);
    libfunctions('dynamixel');

    id  =6;
    speed = 100;
    
     MoveServo(90,1,speed)
    
     MoveServo(90,6,speed)
    
     MoveServo(55,7,speed)
    
    % clean up
    unloadlibrary('dynamixel');
% CloseClaw(50)
% OpenClaw(50)
% MoveUpperPair(110,30)
% MoveLowerPair(-110,30)