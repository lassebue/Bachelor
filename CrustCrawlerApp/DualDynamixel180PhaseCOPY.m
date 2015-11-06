function DualDynamixel180PhaseCOPY(id, theta, speed)
%{  
    DualDynamixel180Phase will rotate two dynamixels
    in opposite directions.
    
    id is a two element array that contains the
    Dynamixel IDs

    theta is the angle in radians to move from center

    speed is the moving speed 0-1023
    
    DualDynamixel180Phase(id, sind(0), 256)

    Sample SyncWrite command
    FF FF FE 0E 83 1E 04 01 00 02 00 02 02 00 01 00 01 45
%}

    %Load the dynamixel library
    fullPathToHeader = 'dynamixel.h';
    fullPathToLibrary = 'dynamixel';
    
    loadlibrary(fullPathToLibrary,fullPathToHeader);
    m = libfunctions(fullPathToLibrary);
    
    response = calllib(fullPathToLibrary,'dxl_initialize',4,1)
    pause on
    
    %There should only be 2 Dynamixels total
    numberOfDynamixels = length(id);
    
    if response == 1
        
        % Phase allows us to position the servos
        % in some relationship to theta.
        phase = zeros(1,2);
        for i = 1:numberOfDynamixels
            phase(1,i) = (2 * pi) * (i)/numberOfDynamixels;
        end

        goalPosition = zeros(1,2);
        % Convert theta + phase to goal position
        for i = 1:numberOfDynamixels
            goalPosition(1, i) = int16((sin(theta + phase(1,i)) + 1) * 512);
        end
        
        %Broadcast ID
        calllib(fullPathToLibrary,'dxl_set_txpacket_id', 254);
        
        %Length is 14
        %That handles position and speed for two dynamixels
        calllib(fullPathToLibrary,'dxl_set_txpacket_length',14);
        
        %SyncWrite instruction
        calllib(fullPathToLibrary,'dxl_set_txpacket_instruction',131);
        
        %Starting address (goal position)
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',0, 30);
        
        %length of data to write to each dynamixel
        %We're writing position and speed = 4 bytes
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',1, 4);
        
        %Parameters for syncwrite
        % id | position | speed
        %ID 
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',2, 1);
        
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', goalPosition(1,1));
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', goalPosition(1,1));
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',3, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',4, highByte);
        
        %Speed = 512
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', speed);
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', speed);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',5, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',6, highByte);
        
        %Parameters for syncwrite dynamixel id = 2
        % id | position | speed
        %ID = 2
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',7, 2);
        
        %Position = 512
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', goalPosition(1,2));
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', goalPosition(1,2));
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',8, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',9, highByte);
        
        %Speed = 512
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', speed);
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', speed);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',10, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',11, highByte);
       
        
        %transmit
        calllib(fullPathToLibrary,'dxl_tx_packet');      
        
    else
        disp('Failed to open USB2Dynamixel!');
    end
    calllib(fullPathToLibrary,'dxl_terminate');
    unloadlibrary(fullPathToLibrary);
end