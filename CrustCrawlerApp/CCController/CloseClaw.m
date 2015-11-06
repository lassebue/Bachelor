function CloseClaw(speed)
% closes the claw on the CrustCrawler
% Speed is the speed on the rotation
id = 7;
deg = 55;
MoveServo(id,deg,speed) 
end

