function SerialLed()
   % This examples shows how to you can use native MatLab fucntions
   % to control an AX-12.  If have problems getting this to work, 
   % make sure that the serial port is closed!

    % Set the port paramenter
    s=serial('COM3', 'BaudRate', 1000000, 'Parity', 'none', 'DataBits', 8, 'StopBits', 1);
    
    % open the port
    fopen(s);
    
    % display the com port resources
    com = instrfind;
    disp(com);
    
    %-------- [LED 1 On ] -------------------------------
    %FF FF 01 04 03 19 01 DD 
    a = [255, 255, 1, 4, 3, 25, 01, 221];
    %----------------------------------------------------
    
    %-------- [LED 1 Off ] -----------------------------
    %FF FF 01 04 03 19 00 DE 
    %a = [255, 255, 1, 4, 3, 25, 00, 222];
    %---------------------------------------------------
    
    % display the values in a
    disp(a)
    
    % binary write
    fwrite(s, a);
    
    % Expecting a 6 byte status packet
    out=fread(s, 6);
    
    % Display status packet
    disp(out);
    
    % Clean up
    fclose(s);
    delete(s);
    clear s;
end