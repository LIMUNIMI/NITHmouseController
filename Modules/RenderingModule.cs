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
            AppWindow.txtConnectedHT.Text = Rack.USBreceiver.IsConnected ? "Connected" : "Not Connected";
            AppWindow.txtConnectedFC.Text = Rack.UDPreceiver.IsConnected ? "Connected" : "Not Connected";
            AppWindow.txtPortHT.Text = Rack.USBreceiver.Port.ToString();
            AppWindow.txtValuesHT.Text = Rack.DataManagerModule.HeadTrackerArgumentsString;
            AppWindow.txtValuesFC.Text = Rack.DataManagerModule.FaceCamArgumentsString;
            AppWindow.indHTconnected.Fill = Rack.USBreceiver.IsConnected ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.indEmulateClicks.Fill = Rack.UserSettings.BlinkClicking ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.indEmulateCursor.Fill = Rack.UserSettings.EmulateCursor ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.ellEyeLeft.Fill = IsLeftEyeOpen ? Rack.EmptyBrush: Rack.FullBrush;
            AppWindow.ellEyeRight.Fill = IsRightEyeOpen ? Rack.EmptyBrush : Rack.FullBrush;
            AppWindow.ellMouth.Fill = IsMouthOpen ? Rack.EmptyBrush : new SolidColorBrush(Colors.DarkRed);
            AppWindow.txtSensitivity.Text = Rack.UserSettings.HtSensitivity.ToString("F0");
            AppWindow.indFCconnected.Fill = Rack.UDPreceiver.IsConnected ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.indMouthClicking.Fill = Rack.UserSettings.MouthClicking ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.txtFilter.Text = Rack.BNith_Head_EmulateCursor.FilterXAlpha.ToString("F2");

            AppWindow.indHeadSourceUDP.Fill = Rack.MappingModule.HeadSource == HeadSources.UDP ? Rack.YesBrush : Rack.NoBrush;
            AppWindow.indHeadSourceUSB.Fill = Rack.MappingModule.HeadSource == HeadSources.USB ? Rack.YesBrush : Rack.NoBrush;

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
            Rack.B_HeadTrackerPlotter.UpdateGraphics();
        }
    }
}