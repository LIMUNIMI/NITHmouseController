using ConsoleEmulation;
using NITHmouseController.Modules;
using NITHmouseController.Setups;
using System.Windows;

namespace NITHmouseController
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AppWindow : Window
    {
        private ConsoleTextToTextBox _consoleRedirector;
        private DefaultSetup _setup;

        public AppWindow()
        {
            InitializeComponent();
            _consoleRedirector = new ConsoleTextToTextBox(txtConsole, scrConsole);
            Console.SetOut(_consoleRedirector);
        }

        private void btnBottomRight_Click(object sender, RoutedEventArgs e)
        {
            Rack.MouseCalibratorTwoEdgesMethod.CalibrateRightDown();
        }

        private void btnCalibrateHT_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnCalibrationClosed_Click(object sender, RoutedEventArgs e)
        {
            Rack.NithPreprocessorWebcamWrapper.Calibrate_Closed();
        }

        private void btnCalibrationOpen_Click(object sender, RoutedEventArgs e)
        {
            Rack.NithPreprocessorWebcamWrapper.Calibrate_Open();
        }

        private void btnCenter_Click(object sender, RoutedEventArgs e)
        {
            // Calibrate both USB and UDP head preprocessors
            Rack.NithPreprocessorHeadTrackerCalibratorUSB.SetCenterToCurrentPosition();
            Rack.NithPreprocessorHeadTrackerCalibratorUDP.SetCenterToCurrentPosition();
        }

        private void btnClickEmulation_Click(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.BlinkClicking = !Rack.UserSettings.BlinkClicking;
        }

        private void btnCursorEmulation_Click(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.EmulateCursor = !Rack.UserSettings.EmulateCursor;
        }

        private void btnFilterMinus_Click(object sender, RoutedEventArgs e)
        {
            Rack.BNith_Head_EmulateCursor.FilterXAlpha -= 0.05f;
            Rack.BNith_Head_EmulateCursor.FilterYAlpha -= 0.05f;
        }

        private void btnFilterPlus_Click(object sender, RoutedEventArgs e)
        {
            Rack.BNith_Head_EmulateCursor.FilterXAlpha += 0.05f;
            Rack.BNith_Head_EmulateCursor.FilterXAlpha += 0.05f;
        }

        private void btnForceReconnectUDP_Click(object sender, RoutedEventArgs e)
        {
            Rack.UDPreceiver.Disconnect();
            Rack.UDPreceiver.Connect();
        }

        private void BtnMouthClicking_OnClick(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.MouthClicking = !Rack.UserSettings.MouthClicking;
        }

        private void btnScanHT_Click(object sender, RoutedEventArgs e)
        {
            Rack.USBportDetector.Scan();
        }

        private void btnSensitivityMinus_Click(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.HtSensitivity -= 1f;
        }

        private void btnSensitivityPlus_Click(object sender, RoutedEventArgs e)
        {
            Rack.UserSettings.HtSensitivity += 1f;
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnTopLeft_Click(object sender, RoutedEventArgs e)
        {
            Rack.MouseCalibratorTwoEdgesMethod.CalibrateLeftUp();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _setup.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _setup = new DefaultSetup(this);
            _setup.Setup();
            Console.WriteLine("Program started.");

            // Rack.USBportDetector.Scan();
        }

        private void btnHeadSourceUSB_Click(object sender, RoutedEventArgs e)
        {
            Rack.MappingModule.HeadSource = HeadSources.USB;
            Rack.MappingModule.SetHeadTrackingModule();
        }

        private void btnHeadSourceUDP_Click(object sender, RoutedEventArgs e)
        {
            Rack.MappingModule.HeadSource = HeadSources.UDP;
            Rack.MappingModule.SetHeadTrackingModule();
        }
    }
}