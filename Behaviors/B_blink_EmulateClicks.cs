using NITHemulation.Modules.Mouse;
using NITHlibrary.Nith.BehaviorTemplates;
using NITHmouseController.Modules;

namespace NITHmouseController.Behaviors
{
    internal class BBlinkEmulateClicks : ANithBlinkEventBehavior
    {

        public BBlinkEmulateClicks()
        {
            DCThresh = 8;
            DOThresh = 8;
            RCThresh = 3;
            ROThresh = 3;
            LCThresh = 3;
            LOThresh = 3;
        }

        protected override void Event_doubleClose() // Send double click
        {
            if (Rack.UserSettings.BlinkClicking)
            {

                MouseSender.SendMouseButtonEvent(MouseButtonFlags.LeftDown);
                MouseSender.SendMouseButtonEvent(MouseButtonFlags.LeftUp);
                MouseSender.SendMouseButtonEvent(MouseButtonFlags.LeftDown);
                MouseSender.SendMouseButtonEvent(MouseButtonFlags.LeftUp);
            }
        }

        protected override void Event_doubleOpen()
        {

        }

        protected override void Event_leftClose()
        {
            if (Rack.UserSettings.BlinkClicking)
            {
                MouseSender.SendMouseButtonEvent(MouseButtonFlags.LeftDown);
            }
        }

        protected override void Event_leftOpen()
        {
            if (Rack.UserSettings.BlinkClicking)
            {
                MouseSender.SendMouseButtonEvent(MouseButtonFlags.LeftUp);
            }
        }

        protected override void Event_rightClose()
        {
            if (Rack.UserSettings.BlinkClicking)
            {
                MouseSender.SendMouseButtonEvent(MouseButtonFlags.RightDown);
            }
        }

        protected override void Event_rightOpen()
        {
            if (Rack.UserSettings.BlinkClicking)
            {
                MouseSender.SendMouseButtonEvent(MouseButtonFlags.RightUp);
            }
        }
    }
}