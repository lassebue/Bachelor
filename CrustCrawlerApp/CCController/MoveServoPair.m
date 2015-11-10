function MoveServoPair(id, deg, speed)
%{  
    rotates a dual angle with two servos
    
    id is a two element array that contains the servos IDs
    deg is the angle in degrees
    speed is the speed of the rotation
%}
    % Setup Dynamixel API
    fullPathToLibrary = 'dynamixel';
    res = calllib(fullPathToLibrary,'dxl_initialize',4,1);

    if res == 1
        goalPosition = zeros(1,2);
        goalPosition(1,1)= 512+(-deg/0.29);
        goalPosition(1,2)= 512+(deg/0.29);

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
        %ID 2
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',2, id(:,1));
        
        %Position
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', goalPosition(1,1));
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', goalPosition(1,1));
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',3, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',4, highByte);
        
        %Speed
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', speed);
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', speed);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',5, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',6, highByte);
        
        %Parameters for servo 2
        % id | position | speed
        %ID 2
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',7, id(:,2));
        
        %Position
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', goalPosition(1,2));
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', goalPosition(1,2));
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',8, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',9, highByte);
        
        %Speed
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
end