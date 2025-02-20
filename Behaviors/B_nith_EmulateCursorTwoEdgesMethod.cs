using NITHmouseController.Modules;
using System.Windows;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Tools.Filters.ValueFilters;
using NITHemulation.Modules.Mouse;

namespace NITHmouseController.Behaviors
{
    public class BNithEmulateCursorTwoEdgesMethod : INithSensorBehavior
    {
        private IDoubleFilter _filterX = new DoubleFilterMAexpDecaying(0.9f);
        private IDoubleFilter _filterY = new DoubleFilterMAexpDecaying(0.9f);

        private List<NithParameters> _requiredArgs = new List<NithParameters>
        {
            NithParameters.head_pos_pitch, NithParameters.head_pos_yaw
        };

        public void HandleData(NithSensorData nithData)
        {
            if (nithData.ContainsParameters(_requiredArgs))
            {
                _filterX.Push(nithData.GetParameterValue(NithParameters.head_pos_yaw).Value.BaseAsDouble);
                _filterY.Push(nithData.GetParameterValue(NithParameters.head_pos_pitch).Value.BaseAsDouble);

                Rack.MouseCalibratorTwoEdgesMethod.CurrentX = (float)_filterX.Pull();
                Rack.MouseCalibratorTwoEdgesMethod.CurrentY = -(float)_filterY.Pull();

                Point mapped = Rack.MouseCalibratorTwoEdgesMethod.Map();

                if(Rack.UserSettings.EmulateCursor && Rack.MouseCalibratorTwoEdgesMethod.Calibrated)
                {
                    MouseSender.SetCursorPosition(new System.Drawing.Point((int)mapped.X, (int)mapped.Y));
                }
            }
        }
    }
}