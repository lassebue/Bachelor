using System;
namespace CrustCrawlerApp.WindControl
{
    public interface IDisplayPose
    {
        string CurrentPose { get; set; }
        int WindowCount { get; set; }
        int CurrentWindow { get; set; }
    }
}
