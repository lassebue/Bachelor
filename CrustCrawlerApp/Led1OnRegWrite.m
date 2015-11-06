function Led1OnRegWrite()

    loadlibrary('dynamixel','dynamixel.h');
    libfunctions('dynamixel');
    res = calllib('dynamixel','dxl_initialize');
    pause on

    if res == 1
        %dynamixel 1
        calllib('dynamixel','dxl_set_txpacket_id',1);
        %length = number of parameter + 2 (2 + 2)
        calllib('dynamixel','dxl_set_txpacket_length',4);
        %reg writing
        calllib('dynamixel','dxl_set_txpacket_instruction',4);
        %Build instruction parameters
        %Parameter 0 = Address
        calllib('dynamixel','dxl_set_txpacket_parameter',0, 25);
        %Parameter 1 = value
        calllib('dynamixel','dxl_set_txpacket_parameter',1, 1);
        %Transmit
        calllib('dynamixel','dxl_tx_packet');
        
        
        %Zero out previous parameters
        calllib('dynamixel','dxl_set_txpacket_parameter',0, 0);
        calllib('dynamixel','dxl_set_txpacket_parameter',1, 0);
        
        %Action Instruction
        calllib('dynamixel','dxl_initialize');
        calllib('dynamixel','dxl_set_txpacket_id',254);
        calllib('dynamixel','dxl_set_txpacket_length',2);
        calllib('dynamixel','dxl_set_txpacket_instruction',5);
        
        %Transmit
        calllib('dynamixel','dxl_tx_packet');
    else
        disp('Failed to open USB2Dynamixel!');
    end
    calllib('dynamixel','dxl_terminate');
    unloadlibrary('dynamixel');
    
    %Action();
end