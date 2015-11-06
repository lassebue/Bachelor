function MoveServo(deg,id,speed)
    % Dynamixel Hello World
    % Light the LED on ID 1

    
    % theta is angle in degrees
    
    % setup the dynamixel API
    % Load dynamixel.dll

    fullPathToHeader = 'dynamixel.h';
    fullPathToLibrary = 'dynamixel';
    res = calllib(fullPathToLibrary,'dxl_initialize',4,1);

    if res == 1         
        goalPosition = 512+(deg/0.29);
        
        %dynamixel 1
        calllib('dynamixel','dxl_set_txpacket_id',254);
        %length = number of parameter + 2 (2 + 2)
        calllib('dynamixel','dxl_set_txpacket_length',9);
        %writing
        calllib('dynamixel','dxl_set_txpacket_instruction',131);
        %Build instruction parameters (30 = Goal Position)
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',0, 30);
        
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',1, 4);
        
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',2, id);
        
        %Position
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', goalPosition(1,1));
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', goalPosition(1,1));
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',3, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',4, highByte);
        
        %Speed = 512
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte', speed);
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', speed);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',5, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',6, highByte);
         
        %transmit
        calllib('dynamixel','dxl_tx_packet');
    else
        disp('Failed to open USB2Dynamixel!');
    end
    
    calllib('dynamixel','dxl_terminate');

end

