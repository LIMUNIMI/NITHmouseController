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
using System.Windows.Interop;

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
            Rack.NithModuleHeadTracker = new NithModule();
            Rack.NithModuleFaceCam = new NithModule();
            Rack.DataManagerModule = new DataManagerModule();
            Rack.MouseCalibratorTwoEdgesMethod = new MouseCalibratorTwoEdgesMethod();
            Rack.MouseCalibratorCenterMethod = new MouseCalibratorCenterMethod();
            Rack.KeyboardModule = new KeyboardModuleWPF(Window, RawInputCaptureMode.ForegroundAndBackground);

            // Head tracker plotter
            Rack.HeadTrackerPlotterNoElements = new BNithHeadTrackerPlotter
                (
                    Window.cnvHeadTrackerPlot,
                    Window.dotPitchYaw,
                    Window.dotPitchRoll
                );

            // Preprocessors
            Rack.NithPreprocessorFaceCam = new NithPreprocessor_WebcamWrapper(NithWebcamCalibrationModes.Manual);
            List<NithParameters> eyeApertureParams = new List<NithParameters>() { NithParameters.eyeLeft_ape, NithParameters.eyeRight_ape };

            Rack.NithModuleFaceCam.Preprocessors.Add(new NithPreprocessor_MAfilterParams(eyeApertureParams, 0.8f));
            Rack.NithModuleFaceCam.Preprocessors.Add(Rack.NithPreprocessorFaceCam);
            Rack.NithPreprocessorHeadTrackerCalibrator = new NithPreprocessor_HeadTrackerCalibrator();
            Rack.NithModuleHeadTracker.Preprocessors.Add(Rack.NithPreprocessorHeadTrackerCalibrator);

            // Make port managers and connect NithModules
            Rack.UsBreceiver = new USBreceiver();
            Rack.UsBreceiver.Listeners.Add(Rack.NithModuleHeadTracker);
            Rack.UsBportDetector = new NithUSBportDetector();
            Rack.UsBportDetector.Behaviors.Add(new BUSBreceiver_ConnectToPort(Rack.UsBreceiver, "NithHT_BNO055"));

            Rack.UDPreceiverFaceCam = new UDPreceiver(20100);
            Rack.UDPreceiverFaceCam.Listeners.Add(Rack.NithModuleFaceCam);
            Rack.UDPreceiverFaceCam.Connect();

            // Add disposables to list
            Disposables.Add(Rack.RenderingModule);
            Disposables.Add(Rack.NithModuleFaceCam);
            Disposables.Add(Rack.NithModuleHeadTracker);
            Disposables.Add(Rack.UsBreceiver);
            Disposables.Add(Rack.UDPreceiverFaceCam);

            // Make FaceCam behaviors
            Rack.BlinkEmulator = new BBlinkEmulateClicks();
            Rack.NithModuleFaceCam.SensorBehaviors.Add(Rack.BlinkEmulator);
            Rack.NithModuleFaceCam.SensorBehaviors.Add(new BNithFcReadValuesToPrint());
            Rack.NithModuleFaceCam.SensorBehaviors.Add(new BNithFcSaveBlinkDataSomewhere());
            Rack.NithModuleFaceCam.SensorBehaviors.Add(new BNithFcMouthClick());
            Rack.NithModuleFaceCam.SensorBehaviors.Add(new BBlinkCalibrateWithDoubleBlink());
            Rack.NithModuleFaceCam.ErrorBehaviors.Add(new BNithFcErrorHandler(Rack.NithModuleFaceCam));

            // Make HeadTracker behaviors
            Rack.NithModuleHeadTracker.SensorBehaviors.Add(new BNithHtReadValuesToPrint());
            Rack.NithModuleHeadTracker.SensorBehaviors.Add(new BNithHtStanceSelector());
            // Rack.NithModuleHeadTracker.SensorBehaviors.Add(new B_nith_EmulateCursorTwoEdgesMethod()); // This uses the double edge method
            Rack.EmulateMouseCursorBehavior = new BNithEmulateCursorCenterMethod();
            Rack.NithModuleHeadTracker.SensorBehaviors.Add(Rack.EmulateMouseCursorBehavior); // This uses the center method
            Rack.NithModuleHeadTracker.SensorBehaviors.Add(Rack.HeadTrackerPlotterNoElements);
            Rack.NithModuleHeadTracker.ErrorBehaviors.Add(new BNithHtErrorHandler(Rack.NithModuleHeadTracker));

            Rack.KeyboardModule.KeyboardBehaviors.Add(new BKeyboardKillEmulateCursor());

            // Enable HeadTrackerPlotter
            Rack.HeadTrackerPlotterNoElements.Enabled = true;

            // You will probably want to leave this at the end!
            Rack.RenderingModule.StartRendering();
        }
    }
}