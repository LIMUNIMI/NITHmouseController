using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Tools.Filters.ValueFilters;

namespace NITHmouseController.Elements
{
    internal class BNithHeadTrackerPlotter : INithSensorBehavior
    {
        private const int DefaultRadius = 180;
        private double _accPitch = 0;
        private double _accRoll = 0;
        private double _accYaw = 0;
        private Canvas _cnvPlot;
        private Ellipse _dotPr;
        private Ellipse _dotPy;
        private double _normalizedPosPitch = 0;
        private double _normalizedPosRoll = 0;
        private double _normalizedPosYaw = 0;
        private double _posPitch = 0;
        private double _posRoll = 0;
        private double _posYaw = 0;

        private List<NithParameters> _requiredArgsAcc = new List<NithParameters>
        {
            NithParameters.head_acc_pitch, NithParameters.head_acc_yaw, NithParameters.head_acc_roll
        };

        private List<NithParameters> _requiredArgsPos = new List<NithParameters>
        {
            NithParameters.head_pos_pitch, NithParameters.head_pos_yaw, NithParameters.head_pos_roll
        };

        public BNithHeadTrackerPlotter(Canvas cnvPlot, Ellipse dotPy, Ellipse dotPr)
        {
            this._cnvPlot = cnvPlot;
            this._dotPr = dotPr;
            this._dotPy = dotPy;

            // Create lines
            DrawCenterLines(cnvPlot);
        }

        public bool Enabled { get; set; } = false;
        public IDoubleFilter PitchFilter { get; set; } = new DoubleFilterBypass();
        public float PitchMultiplier { get; set; } = 1;
        public string RawAccelString { get; private set; }
        public string RawPositionString { get; private set; }
        public IDoubleFilter RollFilter { get; set; } = new DoubleFilterBypass();
        public float RollMultiplier { get; set; } = 1;
        public HTvisualizationModes VisualizationMode { get; set; } = HTvisualizationModes.Position;
        public IDoubleFilter YawFilter { get; set; } = new DoubleFilterBypass();
        public float YawMultiplier { get; set; } = 1;

        public void HandleData(NithSensorData nithData)
        {
            switch (VisualizationMode)
            {
                case HTvisualizationModes.Position:
                    if (nithData.ContainsParameters(_requiredArgsPos) && Enabled)
                    {
                        // Get positions
                        _posPitch = nithData.GetParameterValue(NithParameters.head_pos_pitch).Value.BaseAsDouble;
                        _posYaw = nithData.GetParameterValue(NithParameters.head_pos_yaw).Value.BaseAsDouble;
                        _posRoll = nithData.GetParameterValue(NithParameters.head_pos_roll).Value.BaseAsDouble;

                        // Filter position
                        PitchFilter.Push(_posPitch);
                        YawFilter.Push(_posYaw);
                        RollFilter.Push(_posRoll);
                        _posPitch = -PitchFilter.Pull(); // Inverted because... I don't know. It works.
                        _posYaw = YawFilter.Pull();
                        _posRoll = RollFilter.Pull();

                        // Compute normalized positions relative to screen
                        _normalizedPosPitch = (_cnvPlot.ActualHeight / 2) + (_posPitch / (DefaultRadius * 2) * _cnvPlot.ActualHeight * PitchMultiplier);
                        _normalizedPosYaw = (_cnvPlot.ActualWidth / 2) + (_posYaw / (DefaultRadius * 2) * _cnvPlot.ActualWidth * YawMultiplier);
                        _normalizedPosRoll = (_cnvPlot.ActualWidth / 2) + (_posRoll / (DefaultRadius * 2) * _cnvPlot.ActualWidth * RollMultiplier);
                        
                        // Set height subtracting from height
                        _normalizedPosPitch = _cnvPlot.Height - _normalizedPosPitch;
                    }
                    break;

                case HTvisualizationModes.Acceleration:
                    if (nithData.ContainsParameters(_requiredArgsAcc) && Enabled)
                    {
                        // Get accelerations
                        _accPitch = nithData.GetParameterValue(NithParameters.head_acc_pitch).Value.BaseAsDouble;
                        _accYaw = nithData.GetParameterValue(NithParameters.head_acc_yaw).Value.BaseAsDouble;
                        _accRoll = nithData.GetParameterValue(NithParameters.head_acc_roll).Value.BaseAsDouble;
                    }
                    break;
            }
        }

        public void UpdateGraphics()
        {
            if (Enabled)
            {
                // Plot dots based on visualization mode
                switch (VisualizationMode)
                {
                    case HTvisualizationModes.Position:
                        // Pitch + Yaw dot
                        Canvas.SetTop(_dotPy, _normalizedPosPitch - _dotPy.ActualHeight / 2);
                        Canvas.SetLeft(_dotPy, _normalizedPosYaw - _dotPy.ActualWidth / 2);
                        // Pitch + Roll dot
                        Canvas.SetTop(_dotPr, _normalizedPosPitch - _dotPr.ActualHeight / 2);
                        Canvas.SetLeft(_dotPr, _normalizedPosRoll - _dotPr.ActualWidth / 2);
                        break;

                    case HTvisualizationModes.Acceleration:
                        // Pitch + Yaw dot
                        Canvas.SetTop(_dotPy, _accPitch - _dotPy.ActualHeight / 2);
                        Canvas.SetLeft(_dotPy, _accYaw - _dotPy.ActualWidth / 2);
                        // Pitch + Roll dot
                        Canvas.SetTop(_dotPr, _accPitch - _dotPr.ActualHeight / 2);
                        Canvas.SetLeft(_dotPr, _accRoll - _dotPr.ActualWidth / 2);
                        break;
                }

                // Fill the raw data strings
                RawPositionString =
                    "Y: " + _posYaw.ToString("F2") + "\n" +
                    "P: " + _posPitch.ToString("F2") + "\n" +
                    "R: " + _posRoll.ToString("F2");
                RawAccelString =
                    "Y: " + _accYaw.ToString("F2") + "\n" +
                    "P: " + _accPitch.ToString("F2") + "\n" +
                    "R: " + _accRoll.ToString("F2");
            }
        }

        private void DrawCenterLines(Canvas cnvPlot)
        {
            // Create the horizontal line
            Line horizontalLine = new Line
            {
                Stroke = new SolidColorBrush(Color.FromRgb(0x22, 0x22, 0x22)),
                StrokeThickness = 2,
                X1 = 0,
                Y1 = cnvPlot.Height / 2,
                X2 = cnvPlot.Width,
                Y2 = cnvPlot.Height / 2
            };

            // Create the vertical line
            Line verticalLine = new Line
            {
                Stroke = new SolidColorBrush(Color.FromRgb(0x22, 0x22, 0x22)),
                StrokeThickness = 2,
                X1 = cnvPlot.Width / 2,
                Y1 = 0,
                X2 = cnvPlot.Width / 2,
                Y2 = cnvPlot.Height
            };

            // Put them in the backgrounds
            Canvas.SetZIndex(horizontalLine, -1);
            Canvas.SetZIndex(verticalLine, -1);

            // Add the lines to the canvas
            cnvPlot.Children.Add(horizontalLine);
            cnvPlot.Children.Add(verticalLine);
        }
    }
}