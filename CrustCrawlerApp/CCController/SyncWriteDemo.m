function SyncWriteDemo()

    fullPathToHeader = 'dynamixel.h';
    fullPathToLibrary = 'dynamixel';

    loadlibrary(fullPathToLibrary,fullPathToHeader);
    %libfunctions('dynamixel');
    res = calllib(fullPathToLibrary,'dxl_initialize',4,1)
    pause on
    
    numberOfDynamixels = 1;
    
    if res == 1
        
        for i = 1:numberOfDynamixels
            id(1,i) = i;
            phase(1,i) = (2 * pi) * i/numberOfDynamixels;
        end
        
        %Broadcast id 0xFE
        calllib(fullPathToLibrary,'dxl_set_txpacket_id',254);
        
        %Length is 14
        %That handles position and speed for two dynamixels
        calllib(fullPathToLibrary,'dxl_set_txpacket_length',14);
        
        %SyncWrite instruction 0x83
        calllib(fullPathToLibrary,'dxl_set_txpacket_instruction',131);
        
        %Starting address
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',0, 30);
        
        %length of data to write to each dynamixel
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',1, 4);
        
        %Parameters for syncwrite dynamixel id = 1
        % id | position | speed
        %ID = 1
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',2, 1);
        
        %Position = 512
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte',512);
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', 512);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',3, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',4, highByte);
        
        %Speed = 512
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte',512);
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', 512);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',5, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',6, highByte);
        
        
        %Parameters for syncwrite dynamixel id = 2
        % id | position | speed
        %ID = 2
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',7, 2);
        
        %Position = 512
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte',512);
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', 512);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',8, lowByte);
        calllib(fullPathToLibrary,'dxl_set_txpacket_parameter',9, highByte);
        
        %Speed = 512
        lowByte = calllib(fullPathToLibrary,'dxl_get_lowbyte',512);
        highByte = calllib(fullPathToLibrary,'dxl_get_highbyte', 512);
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