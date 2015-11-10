function MoveLowerPair(deg, speed)
% Rotates the lower dual angle
% deg is the angle of the rotation i degrees
% speed is the speed of the rotation
id = [2,3];
MoveServoPair(id, deg, speed);
end