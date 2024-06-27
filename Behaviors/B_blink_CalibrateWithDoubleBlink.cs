using NITHlibrary.Nith.Behaviors;
using NITHmouseController.Modules;

namespace NITHmouseController.Behaviors
{
    internal class BBlinkCalibrateWithDoubleBlink : ANithBlinkEventBehavior
    {
        public BBlinkCalibrateWithDoubleBlink()
        {
            DcThresh = 10;
        }

        protected override void Event_doubleClose()
        {
            Rack.NithPreprocessorHeadTrackerCalibrator.SetCenterToCurrentPosition();
        }

        protected override void Event_doubleOpen()
        {
            
        }

        protected override void Event_leftClose()
        {
            
        }

        protected override void Event_leftOpen()
        {
            
        }

        protected override void Event_rightClose()
        {
            
        }

        protected override void Event_rightOpen()
        {
            
        }
    }
}
