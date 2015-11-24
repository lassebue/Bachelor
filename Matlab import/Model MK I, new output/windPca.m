function y = windPca(x) 
    [coeff,scores,latent,tsquared,explained] = pca(x','NumComponents',1);
    
    y=coeff;
end