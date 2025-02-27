using NITHlibrary.Nith.Internals;
using NITHmouseController.Modules;

namespace NITHmouseController.Behaviors
{
    public class BNith_Head_StanceSelector : INithSensorBehavior
    {
        private readonly List<NithParameters> _requiredParameters = [NithParameters.head_pos_roll];

        public void HandleData(NithSensorData nithData)
        {
            if(nithData.ContainsParameters(_requiredParameters))
            {
                var roll = nithData.GetParameterValue(NithParameters.head_pos_roll).Value.BaseAsDouble;
                
                if(roll > Rack.MappingModule.StanceThreshold)
                {
                    Rack.MappingModule.CurrentStance = Stances.Right;
                    Rack.MappingModule.CurrentClickButton = ClickButton.Right;
                }
                else if(roll < -Rack.MappingModule.StanceThreshold)
                {
                    Rack.MappingModule.CurrentStance = Stances.Left;
                    Rack.MappingModule.CurrentClickButton = ClickButton.Left;
                }
                else
                {
                    Rack.MappingModule.CurrentStance = Stances.Center;
                }
            }
        }
    }
}
