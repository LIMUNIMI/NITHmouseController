using NITHemulation.Modules.Keyboard;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Nith.Module;
using NITHlibrary.Nith.PortDetector;
using NITHlibrary.Nith.Preprocessors;
using NITHlibrary.Nith.Wrappers.NithWebcamWrapper;
using NITHlibrary.Tools.Ports;
using NITHmouseController.Behaviors;
using NITHmouseController.Elements;
using NITHmouseController.Modules;
using RawInputProcessor;

namespace NITHmouseController.Setups
{
    public class DefaultSetup
    {
        public DefaultSetup(AppWindow window)
        {
            Window = window;
            Disposables = new List<IDisposable>();
        }

        private List<IDisposable> Disposables { get; set; }
        private AppWindow Window { get; set; }

        /// <summary>
        /// This method will dispose all the disposable modules. Call on program exit.
        /// </summary>
        public void Dispose()
        {
            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Launches the setup actions for the instrument
        /// </summary>
        public void Setup()
        {
            // usersettings
            Rack.UserSettings = new UserSettings();

            // Make modules
            Rack.MappingModule = new MappingModule();
            Rack.RenderingModule = new RenderingModule(Window);
            Rack.NithModuleUSB = new NithModule();
            Rack.NithModuleUDP = new NithModule();
            Rack.DataManagerModule = new DataManagerModule();
            Rack.MouseCalibratorTwoEdgesMethod = new MouseCalibratorTwoEdgesMethod();
            Rack.MouseCalibratorCenterMethod = new MouseCalibratorCenterMethod();
            Rack.KeyboardModule = new KeyboardModuleWPF(Window, RawInputCaptureMode.ForegroundAndBackground);

            // Preprocessors
            Rack.NithPreprocessorWebcamWrapper = new NithPreprocessor_WebcamWrapper(NithWebcamCalibrationModes.Manual);
            Rack.NithModuleUDP.Preprocessors.Add(Rack.NithPreprocessorWebcamWrapper);

            List<NithParameters> eyeApertureParams = new List<NithParameters>() { NithParameters.eyeLeft_ape, NithParameters.eyeRight_ape };
            Rack.NithModuleUDP.Preprocessors.Add(new NithPreprocessor_MAfilterParams(eyeApertureParams, 0.8f));

            Rack.NithPreprocessorHeadTrackerCalibratorUDP = new NithPreprocessor_HeadTrackerCalibrator();
            Rack.NithPreprocessorHeadTrackerCalibratorUSB = new NithPreprocessor_HeadTrackerCalibrator();
            Rack.NithModuleUDP.Preprocessors.Add(Rack.NithPreprocessorHeadTrackerCalibratorUDP);
            Rack.NithModuleUSB.Preprocessors.Add(Rack.NithPreprocessorHeadTrackerCalibratorUSB);

            // Make port managers and connect NithModules
            Rack.USBreceiver = new USBreceiver();
            Rack.USBreceiver.Listeners.Add(Rack.NithModuleUSB);
            Rack.USBportDetector = new NithUSBportDetector();
            Rack.USBportDetector.Behaviors.Add(new BUSBreceiver_ConnectToPort(Rack.USBreceiver, "NithHT_BNO055"));

            Rack.UDPreceiver = new UDPreceiver(20100);
            Rack.UDPreceiver.Listeners.Add(Rack.NithModuleUDP);
            Rack.UDPreceiver.Connect();

            // Make UDP webcam wrapper behaviors
            Rack.BlinkEmulator = new BBlinkEmulateClicks();
            Rack.NithModuleUDP.SensorBehaviors.Add(Rack.BlinkEmulator);
            Rack.NithModuleUDP.SensorBehaviors.Add(new BNith_UDP_ReadParametersToPrint());
            Rack.NithModuleUDP.SensorBehaviors.Add(new BNithFcSaveBlinkDataSomewhere());
            Rack.NithModuleUDP.SensorBehaviors.Add(new BNithFcMouthClick());
            Rack.NithModuleUDP.SensorBehaviors.Add(new BBlinkCalibrateWithDoubleBlink());
            Rack.NithModuleUDP.ErrorBehaviors.Add(new BNithFcErrorHandler(Rack.NithModuleUDP));

            
            // Make USB HeadTracker behaviors
            Rack.NithModuleUSB.SensorBehaviors.Add(new BNith_USB_ReadParametersToPrint());
            Rack.NithModuleUSB.ErrorBehaviors.Add(new BNithHtErrorHandler(Rack.NithModuleUSB));

            // Instantiate movements behaviors (which can be moved from one module to the other) and call assignment on Mapping Module
            Rack.BNith_Head_EmulateCursor = new BNithEmulateCursorCenterMethod();
            Rack.BNith_Head_StanceSelector = new BNith_Head_StanceSelector();

            // Keyboard behaviors
            Rack.KeyboardModule.KeyboardBehaviors.Add(new BKeyboardKillEmulateCursor()); // Emergency stop

            // HEAD TRACKER PLOTTER
            // Instantiate head tracker plotter behaviors
            Rack.B_HeadTrackerPlotter = new BNithHeadTrackerPlotter
                (
                    Window.cnvHeadTrackerPlot,
                    Window.dotPitchYaw,
                    Window.dotPitchRoll
                );
            
            // Enable HeadTrackerPlotter
            Rack.B_HeadTrackerPlotter.Enabled = true;

            Rack.MappingModule.SetHeadTrackingModule();

            // Add disposables to list
            Disposables.Add(Rack.RenderingModule);
            Disposables.Add(Rack.NithModuleUDP);
            Disposables.Add(Rack.NithModuleUSB);
            Disposables.Add(Rack.USBreceiver);
            Disposables.Add(Rack.UDPreceiver);

            // You will probably want to leave this at the end!
            Rack.RenderingModule.StartRendering();
        }
    }
}