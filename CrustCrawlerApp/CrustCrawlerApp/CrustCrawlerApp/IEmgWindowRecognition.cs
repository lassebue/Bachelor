using System;
namespace CrustCrawlerApp
{
    public interface IEmgWindowRecognition
    {
        void SaveEmgData(EmgDataSample emgSample);
    }
}
