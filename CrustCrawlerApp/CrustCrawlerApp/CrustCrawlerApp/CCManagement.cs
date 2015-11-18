namespace CrustCrawlerApp
{
    public class CCManagement
    {
        private readonly Matlab matlab = new Matlab();
        private readonly int speed = 120;

        public void OpenClaw()
        {
            matlab.MatlabOneParam("OpenClaw", 0, speed);
        }

        public void CloseClaw()
        {
            matlab.MatlabOneParam("CloseClaw", 0, speed);
        }
    }
}