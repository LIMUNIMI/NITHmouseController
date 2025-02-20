using NITHmouseController.Behaviors;
using NITHmouseController.Elements;
using NITHmouseController.Setups;
using System.Windows.Media;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Nith.Module;
using NITHlibrary.Nith.PortDetector;
using NITHlibrary.Nith.Preprocessors;
using NITHlibrary.Tools.Ports;
using NITHlibrary.Nith.Wrappers.NithWebcamWrapper;
using NITHemulation.Modules.Keyboard;

namespace NITHmouseController.Modules
{
    /// <summary>
    /// This will contain all the modules
    /// </summary>
    internal static class Rack
    {
        public static readonly SolidColorBrush NoBrush = new SolidColorBrush(Colors.DarkRed);
        public static readonly SolidColorBrush YesBrush = new SolidColorBrush(Colors.DarkGreen);
        public static readonly SolidColorBrush FullBrush = new SolidColorBrush(Colors.White);
        public static readonly SolidColorBrush EmptyBrush = new SolidColorBrush(Colors.Transparent);
        private static int _port = 1;

        public static BindableGauge BindableGauge1 { get; internal set; }
        public static BindableGauge BindableGauge2 { get; internal set; }
        public static BindableGauge BindableGauge3 { get; internal set; }
        public static BindableGauge BindableGauge4 { get; internal set; }
        public static BindableGauge BindableGauge5 { get; internal set; }
        public static List<BindableGauge> BindableGauges { get; internal set; }
        public static DataManagerModule DataManagerModule { get; set; }
        public static NithPreprocessor_HeadTrackerCalibrator NithPreprocessorHeadTrackerCalibrator { get; set; }
        public static NithPreprocessor_WebcamWrapper NithPreprocessorFaceCam { get; set; }
        public static BNithHeadTrackerPlotter HeadTrackerPlotterNoElements { get; set; }
        public static BBlinkEmulateClicks BlinkEmulator { get; set; }
        public static KeyboardModuleWPF KeyboardModule { get; set; }
        public static MappingModule MappingModule { get; set; }
        public static CalibrationStatus MouseCalibrationStatus { get; set; }
        public static MouseCalibratorTwoEdgesMethod MouseCalibratorTwoEdgesMethod { get; set; }
        public static MouseCalibratorCenterMethod MouseCalibratorCenterMethod { get; set; }
        public static NithModule NithModuleFaceCam { get; set; }
        public static NithModule NithModuleHeadTracker { get; set; }
        public static List<NithParameterValue> NithValues { get; set; }
        public static bool Paused { get; set; } = false;
        public static Statuses PauseStatus { get; set; } = Statuses.ConnectedPlaying;

        public static int Port
        {
            get { return _port; }
            set
            {
                if (value < 1) _port = 1;
                else _port = value;
            }
        }

        public static RenderingModule RenderingModule { get; set; }
        public static string StrMouseCalibration { get; set; } = "";
        public static string TestString { get; set; } = "";
        public static UDPreceiver UDPreceiverFaceCam { get; set; }
        public static NithUSBportDetector UsBportDetector { get; set; }
        public static USBreceiver UsBreceiver { get; set; }
        public static UserSettings UserSettings { get; set; }
        public static BNithEmulateCursorCenterMethod EmulateMouseCursorBehavior { get; set; }
        public static object MouseSender { get; internal set; }
    }
}