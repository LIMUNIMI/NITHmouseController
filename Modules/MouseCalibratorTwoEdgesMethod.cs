using System.Windows;
using System.Windows.Forms;
using NITHlibrary.Tools.Mappers;

namespace NITHmouseController.Elements
{
    internal class MouseCalibratorTwoEdgesMethod
    {
        private SegmentMapper _xmapper;
        private SegmentMapper _ymapper;

        private bool _leftUpCalibrated = false;
        private bool _rightUpCalibrated = false;

        public bool Calibrated
        {
            get { return _leftUpCalibrated && _rightUpCalibrated; }
        }
        
        public MouseCalibratorTwoEdgesMethod()
        {
            var screen = Screen.PrimaryScreen;
            ScreenWidth = screen.Bounds.Width;
            ScreenHeight = screen.Bounds.Height;
            Remap();
        }

        public void SetAsNotCalibrated()
        {
            _leftUpCalibrated = false;
            _rightUpCalibrated = false;
        }

        public void CalibrateLeftUp()
        {
            CalibX1 = CurrentX;
            CalibY1 = CurrentY;
            Remap();
            _leftUpCalibrated = true;
        }

        public void CalibrateRightDown()
        {
            CalibX2 = CurrentX;
            CalibY2 = CurrentY;
            Remap();
            _rightUpCalibrated = true;
        }

        public float CalibX1 { get; set; } = 0;
        public float CalibX2 { get; set; } = 1;

        public float CalibY1 { get; set; } = 0;
        public float CalibY2 { get; set; } = 1;

        public float CurrentX { get; set; } = 0;
        public float CurrentY { get; set; } = 0;

        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        public Point Map()
        {
            return new Point(_xmapper.Map(CurrentX), _ymapper.Map(CurrentY));
        }

        public void Remap()
        {
            _xmapper = new SegmentMapper(CalibX1, CalibX2, 0, ScreenWidth);
            _ymapper = new SegmentMapper(CalibY1, CalibY2, 0, ScreenHeight);
        }
    }
}