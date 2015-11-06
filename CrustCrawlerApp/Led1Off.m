 function Led1Off(id)
    %Light LED1

    fullPathToHeader = 'dynamixel.h';
    fullPathToLibrary = 'dynamixel';
    
    loadlibrary(fullPathToLibrary,fullPathToHeader);
    libfunctions(fullPathToLibrary);
    res = calllib(fullPathToLibrary,'dxl_initialize',4,1);
    
    if res == 1
        %dynamixel 1
        calllib('dynamixel','dxl_set_txpacket_id',id);
        %length = number of parameter + 2 (2 + 2)
        calllib('dynamixel','dxl_set_txpacket_length',4);
        %writing
        calllib('dynamixel','dxl_set_txpacket_instruction',3);
        %Build instruction parameters
        %Parameter 0 = Address
        calllib('dynamixel','dxl_set_txpacket_parameter',0, 25);
        %Parameter 1 = value
        calllib('dynamixel','dxl_set_txpacket_parameter',1, 0);
        %transmit
    calllib('dynamixel','dxl_tx_packet');
    else
        disp('Failed to open USB2Dynamixel!');
    end
    calllib('dynamixel','dxl_terminate');
    unloadlibrary('dynamixel');
end
