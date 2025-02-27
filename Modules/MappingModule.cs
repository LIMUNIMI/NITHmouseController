using NITHmouseController.Modules;

namespace NITHmouseController
{
    public class MappingModule
    {
        public double StanceThreshold { get; set; } = 15f;
        public Stances CurrentStance { get; set; } = Stances.Center;
        public ClickButton CurrentClickButton { get; set; } = ClickButton.Left;
        public CalibrationStatus MouseCalibrationStatus { get; set; }
        public Statuses PauseStatus { get; set; } = Statuses.ConnectedPlaying;
        public bool Paused { get; set; } = false;
        public string StrMouseCalibration { get; set; } = "";
        public string TestString { get; set; } = "";
        public HeadSources HeadSource = HeadSources.USB;

        public void SetHeadTrackingModule()
        {
            switch (Rack.MappingModule.HeadSource)
            {
                case HeadSources.USB:
                    Rack.NithModuleUDP.SensorBehaviors.Remove(Rack.BNith_Head_StanceSelector);
                    Rack.NithModuleUDP.SensorBehaviors.Remove(Rack.BNith_Head_EmulateCursor);
                    Rack.NithModuleUDP.SensorBehaviors.Remove(Rack.B_HeadTrackerPlotter);

                    Rack.NithModuleUSB.SensorBehaviors.Add(Rack.BNith_Head_StanceSelector);
                    Rack.NithModuleUSB.SensorBehaviors.Add(Rack.BNith_Head_EmulateCursor);
                    Rack.NithModuleUSB.SensorBehaviors.Add(Rack.B_HeadTrackerPlotter);
                    break;
                case HeadSources.UDP:
                    Rack.NithModuleUSB.SensorBehaviors.Remove(Rack.BNith_Head_StanceSelector);
                    Rack.NithModuleUSB.SensorBehaviors.Remove(Rack.BNith_Head_EmulateCursor);
                    Rack.NithModuleUSB.SensorBehaviors.Remove(Rack.B_HeadTrackerPlotter);

                    Rack.NithModuleUDP.SensorBehaviors.Add(Rack.BNith_Head_StanceSelector);
                    Rack.NithModuleUDP.SensorBehaviors.Add(Rack.BNith_Head_EmulateCursor);
                    Rack.NithModuleUDP.SensorBehaviors.Add(Rack.B_HeadTrackerPlotter);
                    break;
            }
        }

        private int _port = 1;

        public int Port
        {
            get { return _port; }
            set
            {
                if (value < 1) _port = 1;
                else _port = value;
            }
        }
    }

    public enum Stances
    {
        Left,
        Center,
        Right
    }

    public enum ClickButton
    {
        Left,
        Right
    }

    public enum CalibrationStatus
    {
        Idle,
        LeftUp,
        RightDown
    }

    public enum Statuses
    {
        ConnectedPlaying,
        ConnectedPaused,
        NotConnected
    }

    public enum HeadSources
    {
        USB,
        UDP
    }
}