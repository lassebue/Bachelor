function MoveUpperPair(deg, speed)
% Rotates the upper dual angle
% deg is the angle of the rotation i degrees
% speed is the speed of the rotation
id = [4,5];
MoveServoPair(id, deg, speed);
end