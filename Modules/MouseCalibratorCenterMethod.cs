using System.Windows;

namespace NITHmouseController.Elements
{
    internal class MouseCalibratorCenterMethod
    {
        public MouseCalibratorCenterMethod()
        {
            ScreenWidth = (int)SystemParameters.PrimaryScreenWidth;
            ScreenHeight = (int)SystemParameters.PrimaryScreenHeight;
        }

        public float CurrentX { get; set; } = 0;

        public float CurrentY { get; set; } = 0;

        public int ScreenHeight { get; private set; }

        public int ScreenWidth { get; private set; }

        public Point Map()
        {
            return new Point((CurrentX + (ScreenWidth / 2)), (CurrentY + (ScreenHeight / 2)));
        }
    }
}