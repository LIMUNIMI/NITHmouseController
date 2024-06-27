using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Nith.Preprocessors;
using NITHlibrary.Tools.Filters.ValueFilters;

namespace NITHmouseController.Elements
{
    internal class HeadTrackerPlotter : INithSensorBehavior
    {
        private const int DefaultRadius = 180;
        private Button _btnCalibrate;
        private ComboBox _cbxMode;
        private CheckBox _chkEnabled;
        private Canvas _cnvPlot;
        private Ellipse _dotPr;
        private Ellipse _dotPy;
        private NithPreprocessor_HeadTrackerCalibrator _preprocessor;
        private TextBlock _txtHTrawAccel;
        private TextBlock _txtHTrawPosition;
        private TextBox _txtPitchMultiplier;
        private TextBox _txtRollMultiplier;
        private TextBox _txtYawMultiplier;
        private HTvisualizationModes _visualizationMode = HTvisualizationModes.Position;
        private double _accYaw = 0;
        private double _accPitch = 0;
        private double _accRoll = 0;
        private double _posYaw = 0;
        private double _posPitch = 0;
        private double _posRoll = 0;

        private readonly List<NithParameters> _requiredArgsAcc = new()
        {
            NithParameters.head_acc_pitch, NithParameters.head_acc_yaw, NithParameters.head_acc_roll
        };

        private readonly List<NithParameters> _requiredArgsPos = new()
        {
            NithParameters.head_pos_pitch, NithParameters.head_pos_yaw, NithParameters.head_pos_roll
        };

        public HeadTrackerPlotter(NithPreprocessor_HeadTrackerCalibrator preprocessor, Canvas cnvPlot, CheckBox chkEnabled, TextBox txtYawMultiplier, TextBox txtPitchMultiplier, TextBox txtRollMultiplier, Button btnCalibrate, TextBlock txtHTrawPosition, TextBlock txtHTrawAccel, ComboBox cbxMode, Ellipse dotPy, Ellipse dotPr)
        {
            this._cnvPlot = cnvPlot;
            this._chkEnabled = chkEnabled;
            this._txtYawMultiplier = txtYawMultiplier;
            this._txtPitchMultiplier = txtPitchMultiplier;
            this._txtRollMultiplier = txtRollMultiplier;
            this._btnCalibrate = btnCalibrate;
            this._txtHTrawPosition = txtHTrawPosition;
            this._txtHTrawAccel = txtHTrawAccel;
            this._cbxMode = cbxMode;
            this._dotPr = dotPr;
            this._dotPy = dotPy;

            this._preprocessor = preprocessor;

            chkEnabled.Checked += ChkEnabled_Checked;
            chkEnabled.Unchecked += ChkEnabled_Unchecked;
            txtYawMultiplier.TextChanged += TxtYawMultiplier_TextChanged;
            txtPitchMultiplier.TextChanged += TxtPitchMultiplier_TextChanged;
            txtRollMultiplier.TextChanged += TxtRollMultiplier_TextChanged;
            btnCalibrate.Click += BtnCalibrate_Click;
            cbxMode.SelectionChanged += CbxMode_SelectionChanged;

            // Populate Mode ComboBox
            foreach (string item in Enum.GetNames(typeof(HTvisualizationModes)))
            {
                cbxMode.Items.Add(item);
            }

            // Create lines
            DrawCenterLines(cnvPlot);

            // Initialize values
            cbxMode.SelectedIndex = 0; // Select first element as default
            txtYawMultiplier.Text = YawMultiplier.ToString("F0");
            txtPitchMultiplier.Text = PitchMultiplier.ToString("F0");
            txtRollMultiplier.Text = RollMultiplier.ToString("F0");
        }

        public bool Enabled { get; private set; } = false;
        public IDoubleFilter PitchFilter { get; set; } = new DoubleFilterBypass();
        public float PitchMultiplier { get; private set; } = 1;
        public IDoubleFilter RollFilter { get; set; } = new DoubleFilterBypass();
        public float RollMultiplier { get; private set; } = 1;
        public IDoubleFilter YawFilter { get; set; } = new DoubleFilterBypass();
        public float YawMultiplier { get; private set; } = 1;

        public void UpdateGraphics()
        {
            if (Enabled)
            {
                double pitch = 0;
                double yaw = 0;
                double roll = 0;

                // Decide which are to plot based on visualization mode
                switch (_visualizationMode)
                {
                    case HTvisualizationModes.Position:
                        PitchFilter.Push(_posPitch);
                        YawFilter.Push(_posYaw);
                        RollFilter.Push(_posRoll);
                        pitch = PitchFilter.Pull();
                        yaw = YawFilter.Pull();
                        roll = RollFilter.Pull();
                        break;

                    case HTvisualizationModes.Acceleration:
                        pitch = _accPitch;
                        yaw = _accYaw;
                        roll = _accRoll;
                        break;
                }

                double normalizedPitch = (_cnvPlot.ActualHeight / 2) + (pitch / (DefaultRadius * 2) * _cnvPlot.ActualHeight * PitchMultiplier);
                double normalizedYaw = (_cnvPlot.ActualWidth / 2) + (yaw / (DefaultRadius * 2) * _cnvPlot.ActualWidth * YawMultiplier);
                double normalizedRoll = (_cnvPlot.ActualWidth / 2) + (roll / (DefaultRadius * 2) * _cnvPlot.ActualWidth * RollMultiplier);

                // Invert the Y axis because in WPF, the Y coordinate increases downwards
                normalizedPitch = _cnvPlot.Height - normalizedPitch;

                // Set the position of the tracker dots
                // Pitch + Yaw dot
                Canvas.SetTop(_dotPy, normalizedPitch - _dotPy.ActualHeight / 2);
                Canvas.SetLeft(_dotPy, normalizedYaw - _dotPy.ActualWidth / 2);
                // Pitch + Roll dot
                Canvas.SetTop(_dotPr, normalizedPitch - _dotPr.ActualHeight / 2);
                Canvas.SetLeft(_dotPr, normalizedRoll - _dotPr.ActualWidth / 2);

                // Fill the raw data TextBlocks
                _txtHTrawPosition.Text =
                    "Y: " + _posYaw.ToString("F2") + "\n" +
                    "P: " + _posPitch.ToString("F2") + "\n" +
                    "R: " + _posRoll.ToString("F2");
                _txtHTrawAccel.Text =
                    "Y: " + _posYaw.ToString("F2") + "\n" +
                    "P: " + _posPitch.ToString("F2") + "\n" +
                    "R: " + _posRoll.ToString("F2");
            }
        }

        private void BtnCalibrate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _preprocessor.SetCenterToCurrentPosition();
        }

        private void CbxMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _visualizationMode = (HTvisualizationModes)Enum.Parse(typeof(HTvisualizationModes), _cbxMode.SelectedItem.ToString());
        }

        private void ChkEnabled_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            Enabled = true;
        }

        private void ChkEnabled_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            Enabled = false;
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
            Panel.SetZIndex(horizontalLine, -1);
            Panel.SetZIndex(verticalLine, -1);

            // Add the lines to the canvas
            cnvPlot.Children.Add(horizontalLine);
            cnvPlot.Children.Add(verticalLine);
        }

        private void TxtPitchMultiplier_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                PitchMultiplier = int.Parse(_txtPitchMultiplier.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                PitchMultiplier = DefaultRadius;
            }
        }

        private void TxtRollMultiplier_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                RollMultiplier = int.Parse(_txtRollMultiplier.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                RollMultiplier = DefaultRadius;
            }
        }

        private void TxtYawMultiplier_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                YawMultiplier = int.Parse(_txtYawMultiplier.Text, CultureInfo.InvariantCulture);
            }
            catch
            {
                YawMultiplier = DefaultRadius;
            }
        }

        public void HandleData(NithSensorData nithData)
        {
            if (nithData.ContainsParameters(_requiredArgsPos))
            {
                _posPitch = nithData.GetParameter(NithParameters.head_pos_pitch).Value.BaseAsDouble;
                _posYaw = nithData.GetParameter(NithParameters.head_pos_yaw).Value.BaseAsDouble;
                _posRoll = nithData.GetParameter(NithParameters.head_pos_roll).Value.BaseAsDouble;
            }

            if (nithData.ContainsParameters(_requiredArgsAcc))
            {
                _accPitch = nithData.GetParameter(NithParameters.head_acc_pitch).Value.BaseAsDouble;
                _accYaw = nithData.GetParameter(NithParameters.head_acc_yaw).Value.BaseAsDouble;
                _accRoll = nithData.GetParameter(NithParameters.head_acc_roll).Value.BaseAsDouble;
            }
        }
    }
}