﻿using NITHmouseController.Modules;
using System.Windows;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Tools.Filters.ValueFilters;

namespace NITHmouseController.Behaviors
{
    public class BNithEmulateCursorCenterMethod : INithSensorBehavior
    {
        private IDoubleFilter _filterX = new DoubleFilterMAExpDecaying(0.9f);
        private float _filterXAlpha = 0.9f;
        private IDoubleFilter _filterY = new DoubleFilterMAExpDecaying(0.9f);
        private float _filterYAlpha = 0.9f;

        private readonly List<NithParameters> _requiredArgs = [NithParameters.head_pos_pitch, NithParameters.head_pos_yaw];

        public float FilterXAlpha
        {
            get => _filterXAlpha;
            set
            {
                _filterXAlpha = Math.Max(0.1f, Math.Min(1f, value));
                _filterX = new DoubleFilterMAExpDecaying(_filterXAlpha); // Assuming this class can accept floats
            }
        }

        public float FilterYAlpha
        {
            get => _filterYAlpha;

            set
            {
                _filterYAlpha = Math.Max(0.1f, Math.Min(1f, value));
                _filterY = new DoubleFilterMAExpDecaying(_filterYAlpha); // Assuming this class can accept floats
            }
        }

        public void HandleData(NithSensorData nithData)
        {
            if (nithData.ContainsParameters(_requiredArgs))
            {
                _filterX.Push(nithData.GetParameter(NithParameters.head_pos_yaw)!.Value.BaseAsDouble * Rack.UserSettings.HtSensitivity);
                _filterY.Push(nithData.GetParameter(NithParameters.head_pos_pitch)!.Value.BaseAsDouble * Rack.UserSettings.HtSensitivity);

                Rack.MouseCalibratorCenterMethod.CurrentX = (float)_filterX.Pull();
                Rack.MouseCalibratorCenterMethod.CurrentY = -(float)_filterY.Pull();

                Point mapped = Rack.MouseCalibratorCenterMethod.Map();

                if (Rack.UserSettings.EmulateCursor)
                {
                    Rack.MouseModule.SetCursorPosition(new System.Drawing.Point((int)mapped.X, (int)mapped.Y));
                }
            }
        }
    }
}