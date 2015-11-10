% Loading dynamixel.dll library    
LoadLib();
%{
    Available functions:
    %OpenClaw(speed)                 Opens the claw on the CrustCrawler
    %CloseClaw(speed)                closes the claw on the CrustCrawler
    %MoveUpperPair(deg,speed)        Rotates the upper dual angle
    %MoveLowerPair(deg,speed)        Rotates the lower dual angle
    %MoveServo(id,deg,speed)         Rotates one servo
    %MoveServoPair(id,deg,speed)     rotates a dual angle with two servos
%}

% Functions
MoveUpperPair(-90,100)
MoveLowerPair(90,100)
MoveServo(1,-90,100)
MoveServo(6,-90,100)
OpenClaw(100)
pause(2)
CloseClaw(100)

    
% clean up
UnloadLib()