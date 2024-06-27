using System.Windows.Media;
using System.Windows.Threading;

namespace NITHmouseController.Modules
{
    public class RenderingModule : IDisposable
    {
        public bool IsLeftEyeOpen { get; set; } = true;
        public bool IsRightEyeOpen { get; set; } = true;
        public bool IsMouthOpen { get; set; } = true;

        public RenderingModule(AppWindow appWindow)
        {
            AppWindow = appWindow;
            DispatcherTimer = new DispatcherTimer();
            DispatcherTimer.Interval = new TimeSpan(10000); // 10000 nanoseconds = one millisecond
            DispatcherTimer.Tick += DispatcherUpdate;
        }

        private DispatcherTimer DispatcherTimer { get; set; }
        private AppWindow AppWindow { get; set; }

        public void Dispose()
        {
            DispatcherTimer.Stop();
        }

        /// <summary>
        /// Starts the rendering timer
        /// </summary>
        public void StartRendering()
        {
            DispatcherTimer.Start();
        }

        /// <summary>
        /// Stops the rendering timer
        /// </summary>
        public void StopRendering()
        {
            DispatcherTimer.Stop();
        }

        /// <summary>
        /// This method will be called every time the dispatcher timer is triggered, to update graphics.
        /// </summary>
        private void DispatcherUpdate(object sender, EventArgs e)
        {
            AppWindow.txtConnectedHT.Text = Rack.UsBreceiver.IsConnected ? "Connected" : "Not Connected";
            AppWindow.txtConnectedFC.Text = Rack.UDPreceiverFaceCam.IsConnected ? "Connected" : "Not Connected";
            AppWindow.txtPortHT.Text = Rack.UsBreceiver.Port.ToString();
            AppWindow.txtValuesHT.Text = Rack.DataManagerModule.HeadTrackerArgumentsString;
            AppWindow.txtValuesFC.Text = Rack.DataManagerModule.FaceCamArgumentsString;
            AppWindow.indCalibrated.Fill = Rack.MouseCalibratorTwoEdgesMethod.Calibrated ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.indEmulateClicks.Fill = Rack.UserSettings.BlinkClicking ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.indEmulateCursor.Fill = Rack.UserSettings.EmulateCursor ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.ellEyeLeft.Fill = IsLeftEyeOpen ? Rack.EmptyBrush: Rack.FullBrush;
            AppWindow.ellEyeRight.Fill = IsRightEyeOpen ? Rack.EmptyBrush : Rack.FullBrush;
            AppWindow.ellMouth.Fill = IsMouthOpen ? Rack.EmptyBrush : new SolidColorBrush(Colors.DarkRed);
            AppWindow.txtSensitivity.Text = Rack.UserSettings.HtSensitivity.ToString("F0");
            AppWindow.indFCconnected.Fill = Rack.UDPreceiverFaceCam.IsConnected ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.indMouthClicking.Fill = Rack.UserSettings.MouthClicking ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.txtFilter.Text = Rack.EmulateMouseCursorBehavior.FilterXAlpha.ToString("F2");

            switch (Rack.MappingModule.CurrentClickButton)
            {
                case ClickButton.Left:
                    AppWindow.txtMouseButton.Text = "L";
                    break;
                case ClickButton.Right:
                    AppWindow.txtMouseButton.Text = "R";
                    break;
            }

            // Update graphics of Head tracker plotter
            Rack.HeadTrackerPlotterNoElements.UpdateGraphics();
        }
    }
}