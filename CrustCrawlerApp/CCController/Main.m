% Loading dynamixel.dll library    
LoadLib()

id  =6;
speed = 100;
 
MoveServo(90,1,speed)
    
MoveServo(90,6,speed)
    
MoveServo(55,7,speed)
    
% clean up
unloadlibrary('dynamixel');