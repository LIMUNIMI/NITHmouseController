﻿using NITHmouseController.Modules;
using NITHlibrary.Nith.Internals;

namespace NITHmouseController.Behaviors
{
    internal class BNithFcSaveBlinkDataSomewhere : INithSensorBehavior
    {
        public List<NithParameters> RequiredParams = [NithParameters.eyeLeft_isOpen, NithParameters.eyeRight_isOpen];

        public void HandleData(NithSensorData nithData)
        {
            if (nithData.ContainsParameters(RequiredParams))
            {
                Rack.RenderingModule.IsLeftEyeOpen = bool.Parse(nithData.GetParameterValue(NithParameters.eyeLeft_isOpen).Value.Base);
                Rack.RenderingModule.IsRightEyeOpen = bool.Parse(nithData.GetParameterValue(NithParameters.eyeRight_isOpen).Value.Base);
                Rack.RenderingModule.IsMouthOpen = bool.Parse(nithData.GetParameterValue(NithParameters.mouth_isOpen).Value.Base);
            }
        }
    }
}
