function poseId = getPoseId(poseList,pose)
    i = length (poseList);
    for k=0:i-1
        if (poseList(k+1) == pose)
            poseId = k;
            return
        end
    end
    poseId = -1;
end