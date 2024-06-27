using NITHdmis.Modules.Mouse;
using NITHlibrary.Nith.Behaviors;
using NITHmouseController.Modules;

namespace NITHmouseController.Behaviors
{
    internal class BBlinkEmulateClicks : ANithBlinkEventBehavior
    {

        public BBlinkEmulateClicks()
        {
            DcThresh = 8;
            DoThresh = 8;
            RcThresh = 3;
            RoThresh = 3;
            LcThresh = 3;
            LoThresh = 3;
        }

        protected override void Event_doubleClose() // Send double click
        {
            if (Rack.UserSettings.BlinkClicking)
            {

                Rack.MouseModule.SendMouseButtonEvent(MouseButtonFlags.LeftDown);
                Rack.MouseModule.SendMouseButtonEvent(MouseButtonFlags.LeftUp);
                Rack.MouseModule.SendMouseButtonEvent(MouseButtonFlags.LeftDown);
                Rack.MouseModule.SendMouseButtonEvent(MouseButtonFlags.LeftUp);
            }
        }

        protected override void Event_doubleOpen()
        {

        }

        protected override void Event_leftClose()
        {
            if (Rack.UserSettings.BlinkClicking)
            {
                Rack.MouseModule.SendMouseButtonEvent(MouseButtonFlags.LeftDown);
            }
        }

        protected override void Event_leftOpen()
        {
            if (Rack.UserSettings.BlinkClicking)
            {
                Rack.MouseModule.SendMouseButtonEvent(MouseButtonFlags.LeftUp);
            }
        }

        protected override void Event_rightClose()
        {
            if (Rack.UserSettings.BlinkClicking)
            {
                Rack.MouseModule.SendMouseButtonEvent(MouseButtonFlags.RightDown);
            }
        }

        protected override void Event_rightOpen()
        {
            if (Rack.UserSettings.BlinkClicking)
            {
                Rack.MouseModule.SendMouseButtonEvent(MouseButtonFlags.RightUp);
            }
        }
    }
}