using NITHemulation.Modules.Keyboard;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Nith.Module;
using NITHlibrary.Nith.PortDetector;
using NITHlibrary.Nith.Preprocessors;
using NITHlibrary.Nith.Wrappers.NithWebcamWrapper;
using NITHlibrary.Tools.Ports;
using NITHmouseController.Behaviors;
using NITHmouseController.Elements;
using NITHmouseController.Setups;
using System.Windows.Media;

namespace NITHmouseController.Modules
{
    /// <summary>
    /// This will contain all the modules
    /// </summary>
    internal static class Rack
    {
        public static readonly SolidColorBrush EmptyBrush = new SolidColorBrush(Colors.Transparent);
        public static readonly SolidColorBrush FullBrush = new SolidColorBrush(Colors.White);
        public static readonly SolidColorBrush NoBrush = new SolidColorBrush(Colors.DarkRed);
        public static readonly SolidColorBrush YesBrush = new SolidColorBrush(Colors.DarkGreen);
        public static BNithHeadTrackerPlotter B_HeadTrackerPlotter { get; set; }
        public static BindableGauge BindableGauge1 { get; internal set; }
        public static BindableGauge BindableGauge2 { get; internal set; }
        public static BindableGauge BindableGauge3 { get; internal set; }
        public static BindableGauge BindableGauge4 { get; internal set; }
        public static BindableGauge BindableGauge5 { get; internal set; }
        public static List<BindableGauge> BindableGauges { get; internal set; }
        public static BBlinkEmulateClicks BlinkEmulator { get; set; }
        public static BNithEmulateCursorCenterMethod BNith_Head_EmulateCursor { get; set; }
        public static BNith_Head_StanceSelector BNith_Head_StanceSelector { get; internal set; }
        public static DataManagerModule DataManagerModule { get; set; }
        public static KeyboardModuleWPF KeyboardModule { get; set; }
        public static MappingModule MappingModule { get; set; }
        public static MouseCalibratorCenterMethod MouseCalibratorCenterMethod { get; set; }
        public static MouseCalibratorTwoEdgesMethod MouseCalibratorTwoEdgesMethod { get; set; }
        public static NithModule NithModuleUDP { get; set; }
        public static NithModule NithModuleUSB { get; set; }
        public static NithPreprocessor_HeadTrackerCalibrator NithPreprocessorHeadTrackerCalibratorUDP { get; internal set; }
        public static NithPreprocessor_HeadTrackerCalibrator NithPreprocessorHeadTrackerCalibratorUSB { get; set; }
        public static NithPreprocessor_WebcamWrapper NithPreprocessorWebcamWrapper { get; set; }
        public static List<NithParameterValue> NithValues { get; set; }

        public static RenderingModule RenderingModule { get; set; }

        public static UDPreceiver UDPreceiver { get; set; }
        public static NithUSBportDetector USBportDetector { get; set; }
        public static USBreceiver USBreceiver { get; set; }
        public static UserSettings UserSettings { get; set; }
    }
}